using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponsSO", menuName = "ScriptableObjects/WeaponsSO")]
public class WeaponsSO : ScriptableObject
{
     public string WeaponName;
     public Sprite WeaponImage;

     public int bulletcount;
     public float firerate;
     public int manacost;

}
