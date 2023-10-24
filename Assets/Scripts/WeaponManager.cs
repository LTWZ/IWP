using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        WeaponList = new List<Weapons>();
        mouseController.Fire += FireCurrentWeapon;
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
