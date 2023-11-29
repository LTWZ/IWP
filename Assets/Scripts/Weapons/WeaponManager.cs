using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WeaponStorage
{
    public WeaponsSO weaponsSO;
    public GameObject WeaponsPrefab;
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponStorage[] weaponStorages;
    [SerializeField] GameObject weaponDisplayPrefab;
    private static WeaponManager instance;
    private MouseController mouseController;
    private List<Weapons> WeaponList;
    private List<WeaponsSO> WeaponSOList;
    private WeaponsSO CurrentWeaponsSO;

    public WeaponStorage GetWeaponStorage(WeaponsSO weaponsSO)
    {
        for (int i = 0; i < weaponStorages.Length; i++)
        {
            WeaponStorage weaponStorage = weaponStorages[i];
            if (weaponStorage.weaponsSO == weaponsSO)
            {
                return weaponStorage;
            }
        }
        return null;
    }

    public void DropWeapon(Weapons Weapons)
    {
        if (GetCurrentWeapon() == null)
            return;

        WeaponPickup weaponPickup = Instantiate(weaponDisplayPrefab, PlayerMovement.GetInstance().transform.position, Quaternion.identity).GetComponent<WeaponPickup>();
        weaponPickup.SetWeaponsSO(Weapons.GetWeaponsSO());
        WeaponSOList.Remove(GetCurrentWeapon().GetWeaponsSO());
        Destroy(GetCurrentWeapon().gameObject);
        WeaponList.Remove(GetCurrentWeapon());

        OnWeaponSwap(WeaponList.Count);
    }

    public void LoadWeapon()
    {
        WeaponList.Clear();

        for (int i = 0; i < WeaponSOList.Count; i++)
        {
            WeaponStorage weaponStorage = GetWeaponStorage(WeaponSOList[i]);
            GameObject go = Instantiate(weaponStorage.WeaponsPrefab, PlayerMovement.GetInstance().GetHandPivot());
            Weapons weapons = go.GetComponent<Weapons>();
            Debug.Log(weapons);
            AddWeaponsToList(weapons);
        }
        SetCurrentWeaponSO(CurrentWeaponsSO);
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PlayerManager.GetInstance().onPlayerChange += ChangeMouseControllerReference;
        LevelManager.GetInstance().onSceneLoad += LoadWeapon;
        WeaponList = new();
        WeaponSOList = new();
    }

    void OnWeaponSwap(int num)
    {
        int actualVal = num - 1;

        if (actualVal >= 0)
        {
            if (actualVal < WeaponList.Count)
            {
                for (int i = 0; i < WeaponList.Count; i++)
                    WeaponList[i].gameObject.SetActive(false);

                SetCurrentWeaponSO(WeaponList[actualVal].GetWeaponsSO());
                GetCurrentWeapon().gameObject.SetActive(true);
            }
        }
    }

    public static WeaponManager GetInstance()
    {
        return instance;
    }

    private void FireCurrentWeapon()
    {
        Weapons CurrentWeapon = GetCurrentWeapon();
        if (CurrentWeapon != null)
        {
            CurrentWeapon.FireWeapon();
        }
    }

    public void AddWeaponsToList(Weapons weapons)
    {
        WeaponList.Add(weapons);
        OnWeaponSwap(WeaponList.Count);
    }

    public void AddWeaponsSOToList(Weapons weapons)
    {
        WeaponSOList.Add(weapons.GetWeaponsSO());
    }

    public Vector3 GetDirection()
    {
        return mouseController.GetDirection();
    }

    public void SetCurrentWeaponSO(WeaponsSO weapon)
    {
        CurrentWeaponsSO = weapon;
    }

    public Weapons GetCurrentWeapon()
    {
        return GetWeapons(CurrentWeaponsSO);
    }

    public Weapons GetWeapons(WeaponsSO weaponsSO)
    {
        for (int i = 0; i < WeaponList.Count; i++)
        {
            if (WeaponList[i].GetWeaponsSO() == weaponsSO)
                return WeaponList[i];
        }
        return null;
    }

    /// <summary>
    /// Change which mouseController is it refering to. Refers to the PlayerManager
    /// </summary>
    void ChangeMouseControllerReference()
    {
        mouseController = PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<MouseController>();
        mouseController.onNumsInput += OnWeaponSwap;
        mouseController.Fire += FireCurrentWeapon;
    }
}
