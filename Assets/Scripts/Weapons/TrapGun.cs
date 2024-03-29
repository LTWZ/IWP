using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGun : Weapons
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
        if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() >= 25)
        {
            Rigidbody2D bullet = Instantiate(bullet_prefab, GetEmitterPivot().position, Quaternion.identity).GetComponent<Rigidbody2D>();
            AudioManager.instance.PlaySFX("Trap");
            bullet.velocity = WeaponManager.GetInstance().GetDirection().normalized * BulletSpeed;
            PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().ChangeMana(-25);
        }
        else if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() < 25)
        {

        }
    }
}
