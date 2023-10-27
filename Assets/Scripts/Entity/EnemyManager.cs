using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    private int EnemyHp;
    private float EnemySpeed;

    public void UpdateHP(int hp)
    {
        EnemyHp = hp;
    }

    public int LoadHP()
    {
        return EnemyHp;
    }

    public void UpdateSpeed(int speed)
    {
        EnemySpeed = speed;
    }

    public float LoadSpeed()
    {
        return EnemySpeed;
    }
}
