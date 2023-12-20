using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBeamEnemy : MonoBehaviour
{

    //private bool hitenemy = false;
    public Enemy_1 enemy;
    public int damage = 10; // was private
    [SerializeField] float duration = 1;

    private void OnEnable()
    {
        StartCoroutine(beamPersist());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerEntity>())
            collider.GetComponent<PlayerEntity>().ChangeHealth(-damage);
    }

    IEnumerator beamPersist()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
