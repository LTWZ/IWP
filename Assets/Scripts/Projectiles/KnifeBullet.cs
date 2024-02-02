using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBullet : MonoBehaviour
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
        if (collider.GetComponent<EnemyEntity>())
        {
            collider.GetComponent<EnemyEntity>().ChangeHealth(-1);

            Debug.Log("Hit");
            if (collider.gameObject.tag != "Player" && collider.gameObject.tag != "Room" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "Iceball" && collider.gameObject.tag != "Bladestorm")
            {
                Destroy(gameObject);
            }

            // Start the bleeding effect
            StartBleedingEffect(collider.GetComponent<EnemyEntity>());
        }


        void StartBleedingEffect(EnemyEntity enemy)
        {
            // Check if the enemy already has a bleeding effect
            BleedingEffect existingBleedingEffect = enemy.GetComponent<BleedingEffect>();

            if (existingBleedingEffect == null)
            {
                // Create a new BleedingEffect component on the enemy
                BleedingEffect bleedingEffect = enemy.gameObject.AddComponent<BleedingEffect>();

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
