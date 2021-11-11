using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StructureStatsTemplate", menuName = "Structure/StatsTemplate")]
public class StructureStats : ScriptableObject
{
    [SerializeField] public int team;
    [SerializeField] public bool isAlive;
    [SerializeField] public float maxHealth;

}
