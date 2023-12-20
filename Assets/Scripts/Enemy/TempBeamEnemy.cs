using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBeamEnemy : MonoBehaviour
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

    }

    IEnumerator beamPersist()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
