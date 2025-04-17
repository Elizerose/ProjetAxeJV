using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Database des plateformes de couleurs
/// </summary>
/// 
[CreateAssetMenu(fileName = "PlateformesData", menuName = "Scriptable Objects/PlateformesData")]
public class PlateformesDataBase : ScriptableObject
{
    [SerializeField] private List<PlateformesData> datas = new();

    public PlateformesData GetData(ColorPowerController.ColorAbilities color)
    {
        // On checher l'ennemi avec son type plutot que l'id (plus pratique)
        foreach (PlateformesData data in datas)
        {
            if (data.color == color)
            {
                return data;
            }
        }
        return null;
    }

}

[Serializable]
public class PlateformesData
{
    public ColorPowerController.ColorAbilities color;

    public GameObject Prefab;
    public float AutoDestroyTimer;
    public bool Istrigger;
}