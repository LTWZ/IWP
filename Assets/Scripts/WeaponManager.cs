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
    [SerializeField] MouseController mouseController;
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
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        WeaponList = new();
        mouseController.onNumsInput += OnWeaponSwap;
        mouseController.Fire += FireCurrentWeapon;
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

    // Update is called once per frame
    void Update()
    {
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
}
