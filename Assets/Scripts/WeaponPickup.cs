using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponPickup : MonoBehaviour, IInteract
{
    [SerializeField] WeaponsSO weaponsSO;
    [SerializeField] SpriteRenderer sp;

    private void Start()
    {
        sp.sprite = weaponsSO.WeaponImage;
    }

    private void AddToWeaponList()
    {
        WeaponStorage weaponStorage = WeaponManager.GetInstance().GetWeaponStorage(weaponsSO);
        if (weaponStorage != null)
        {
            Debug.Log(weaponStorage.WeaponsPrefab);
            GameObject go = Instantiate(weaponStorage.WeaponsPrefab, PlayerMovement.GetInstance().GetHandPivot());
            Weapons weapons = go.GetComponent<Weapons>();
            Debug.Log(weapons);
            WeaponManager.GetInstance().AddWeaponsToList(weapons);
            WeaponManager.GetInstance().SetCurrentWeapon(weapons);
        }
    }

    void IInteract.Interact()
    {
        AddToWeaponList();
        Destroy(gameObject);
    }

    public void SetWeaponsSO(WeaponsSO weaponsSO)
    {
        this.weaponsSO = weaponsSO;
    }
}
