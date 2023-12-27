using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    private float MaxTravelTime = 5f;
    public float bleedingDamagePerSecond = 5f;
    public float bleedingDuration = 5f;
    // Start is called before the first frame update
    void Start()
    {
        ////only collision detection
        //mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //rb = GetComponent<Rigidbody2D>();
        //mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 direction = mousePos - transform.position;
        //Vector3 rotation = transform.position - mousePos;
        //rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        //float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0,0,rot +90);    

        Destroy(gameObject, MaxTravelTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // this is hardcoded btw, thr value. U find a way to get it. Idk how u gonna set atatck value
        // my suggestion is to refer to the player dmg value, or the gun value. But u prob need a way to reference to those
        if (collider.GetComponent<PlayerEntity>())
        {
            collider.GetComponent<PlayerEntity>().ChangeHealth(-1);

            Debug.Log("Hit");
            if (collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "Room" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "Iceball" && collider.gameObject.tag != "Bladestorm")
            {
                Destroy(gameObject);
            }

            // Start the bleeding effect
            StartBleedingEffect(collider.GetComponent<PlayerEntity>());
        }


        void StartBleedingEffect(PlayerEntity player)
        {
            // Check if the enemy already has a bleeding effect
            BleedingEffectEnemy existingBleedingEffect = player.GetComponent<BleedingEffectEnemy>();

            if (existingBleedingEffect == null)
            {
                // Create a new BleedingEffect component on the enemy
                BleedingEffectEnemy bleedingEffect = player.gameObject.AddComponent<BleedingEffectEnemy>();

                // Set bleeding parameters
                bleedingEffect.StartBleeding(bleedingDamagePerSecond, bleedingDuration);
            }
            else
            {
                // If the enemy already has a bleeding effect, extend its duration or modify as needed
                existingBleedingEffect.ExtendBleedingDuration(bleedingDuration);
            }
        }


    }
}
