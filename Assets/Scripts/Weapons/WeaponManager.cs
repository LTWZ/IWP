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
    private Weapons CurrentWeapons;

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

        WeaponList.Remove(GetCurrentWeapon());
        Destroy(GetCurrentWeapon().gameObject);

        OnWeaponSwap(WeaponList.Count);
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
        WeaponList = new();
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

                SetCurrentWeapon(WeaponList[actualVal]);
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

    public Vector3 GetDirection()
    {
        return mouseController.GetDirection();
    }

    public void SetCurrentWeapon(Weapons weapon)
    {
        CurrentWeapons = weapon;
    }

    public Weapons GetCurrentWeapon()
    {
        return CurrentWeapons;
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
