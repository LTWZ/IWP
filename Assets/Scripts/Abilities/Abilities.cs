using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Abilites")]
public class Abilities : ScriptableObject
{
    public Sprite abilitySprite;
    public float cooldown;
    public int manaCost;
}