using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBeam : MonoBehaviour
{

    //private bool hitenemy = false;
    public Enemy_1 enemy;
    [SerializeField] float duration = 1;

    private void OnEnable()
    {
        StartCoroutine(beamPersist());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<EnemyEntity>())
            collider.GetComponent<EnemyEntity>().ChangeHealth(-10);
    }

    IEnumerator beamPersist()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
