using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Scriptable Objects/Enemy")]

public class EnemyDatabase : ScriptableObject
{
    [SerializeField] private List<EnemyData> datas = new();

    public EnemyData GetData(int id)
    {
        id = Math.Clamp(id, 0, datas.Count - 1);
        return datas[id];
    }
}