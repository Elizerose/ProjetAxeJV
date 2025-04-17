using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Scriptable Objects/Enemy")]

public class EnemyDatabase : ScriptableObject
{
    [SerializeField] private List<EnemyData> datas = new();

    public EnemyData GetData(Type type)
    {
        // On checher l'ennemi avec son type plutot que l'id (plus pratique)
        foreach (EnemyData data in datas)
        {
            if (data.type == type)
            {
                return data;
            }
        }
        return null;
        
        
        //id = Math.Clamp(id, 0, datas.Count - 1);
        //return datas[id];
    }
}