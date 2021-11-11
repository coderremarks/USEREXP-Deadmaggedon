using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "UnitStatsTemplate",menuName = "Unit/StatsTemplate")]
public class UnitStats : ScriptableObject
{
    [SerializeField] public int team;
    [SerializeField] public bool isAlive;
    [SerializeField] public float maxHealth;
    [SerializeField] public float movementSpeed;
}
