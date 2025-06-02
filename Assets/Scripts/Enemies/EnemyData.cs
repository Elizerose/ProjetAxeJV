using System;
using Unity.VisualScripting;
using UnityEngine;

public enum Type
{
    Garde,
    Arbre,
    Barbare,
    Poisson
}

[Serializable]
public struct EnemyStats
{
    public int pv;
    public float speed;
    public float AttackCooldown;

    public EnemyStats(int pv, float speed, float cooldown)
    {
        this.pv = pv;
        this.speed = speed;
        AttackCooldown = cooldown;
    }
}

[Serializable]
public struct EnemyDetection
{
    public float AttackDistance;
    public float DistanceInSight;
    

    public EnemyDetection(float attack, float sight)
    {
        AttackDistance = attack;
        DistanceInSight = sight;
        
    }
}

[Serializable]
public class EnemyData : BaseData
{
    public Type type;

    [Header("STATS")]
    public EnemyStats stats;

    [Header("SETUP")]

    public float waitTimePatroll;
    public float scale;

    [Header("CHECKS")]
    public EnemyDetection detection;
}
