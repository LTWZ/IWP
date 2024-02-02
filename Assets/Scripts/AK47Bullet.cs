using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47Bullet : MonoBehaviour
{
    private float MaxTravelTime = 5f;
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
            collider.GetComponent<EnemyEntity>().ChangeHealth(-5);


        Debug.Log("Hit");
        if (collider.gameObject.tag != "Player" && collider.gameObject.tag != "Room" && collider.gameObject.tag != "Skill" && collider.gameObject.tag != "Iceball" && collider.gameObject.tag != "Bladestorm" && collider.gameObject.tag != "Fireball" && collider.gameObject.tag != "EnemyAOE" && collider.gameObject.tag != "Punch" && collider.gameObject.tag != "EnemyAOE")
        {
            Destroy(gameObject);
        }
    }
}

