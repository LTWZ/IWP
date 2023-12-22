using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeGun : Weapons
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
        if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() >= 10)
        {
            Rigidbody2D bullet = Instantiate(bullet_prefab, GetEmitterPivot().position, Quaternion.identity).GetComponent<Rigidbody2D>();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            bullet.velocity = WeaponManager.GetInstance().GetDirection().normalized * BulletSpeed;
            PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().ChangeMana(10);
        }
        else if (PlayerManager.GetInstance().GetCurrentPlayer().GetComponent<PlayerEntity>().GetCurrMana() < 10)
        {

        }
    }
}
