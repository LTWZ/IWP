using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapons
{
    [SerializeField] GameObject bullet_prefab;
    [SerializeField] float BulletSpeed;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void Shoot()
    {
        //FINISH UP LOGIC TMR
        if (PlayerMovement.GetInstance().Player.currMana >= 2)
        {
            Rigidbody2D bullet = Instantiate(bullet_prefab, GetEmitterPivot().position, Quaternion.identity).GetComponent<Rigidbody2D>();
            bullet.velocity = WeaponManager.GetInstance().GetDirection().normalized * BulletSpeed;
            PlayerMovement.GetInstance().Player.currMana -= 2;
        }
        else if (PlayerMovement.GetInstance().Player.currMana < 2)
        {

        }
    }
}
