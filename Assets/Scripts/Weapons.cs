using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [SerializeField] WeaponsSO weapons_data;
    [SerializeField] Transform EmitterPivot;
    private float FireElapsed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        FireElapsed = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        FireElapsed += Time.deltaTime;
    }

    public void FireWeapon()
    {
        if (FireElapsed > weapons_data.firerate)
        {
            Shoot();
            FireElapsed = 0;
        }
    }

    protected virtual void Shoot()
    {
        return;
    }

    public Transform GetEmitterPivot()
    {
        return EmitterPivot;
    }
}
