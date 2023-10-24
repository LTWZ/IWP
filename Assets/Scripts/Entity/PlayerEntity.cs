using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [SerializeField] protected int Hp;
    [SerializeField] protected int speed;
    [SerializeField] protected int mana;

    private void Awake()
    {
        if (PlayerManager.GetInstance() != null)
        {
            Hp = PlayerManager.GetInstance().LoadHP();
        }
    }

    public virtual void Skill1()
    {
        
    }

    public virtual void Skill2()
    {

    }

    public virtual void Skill3()
    {

    }

    public virtual void Skill4()
    {

    }
}
