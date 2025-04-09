using System;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string name;

    [Header ("STATS")]
    public int pv;
    public float speed;
    public int damage;

    [Header ("SETUP")]
    public Sprite sprite;
}