using System;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string type;

    [Header ("STATS")]
    public int pv;
    public float speed;

    [Header ("SETUP")]
    public Sprite sprite;
    public float waitTimePatroll;
    public float scale;

    [Header ("CHECKS")]
    public float AttackDistance;
    public float DistanceInSight;
}