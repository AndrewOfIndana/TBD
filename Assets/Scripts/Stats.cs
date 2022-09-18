using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user create stats
public class Stats : ScriptableObject
{
    [Header("Information")]
    public int unitID;
    public string unitName;
    public string unitDescription;

    [Header("Stats")]
    public float unitHealth;
    public float unitAttack;
    public float unitSpeed;
    public float unitAttackRate;
    public float unitAttackRange;

    [Header("Attributes")]
    public Sprite unitSprite;
    public Sprite unitEnemySprite;
    public Vector3 unitSize;
    public int unitBehavior;
}
