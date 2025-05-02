using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    [Header("placement de la plateforme")]
    public GameObject Prefab;
    public float startingPositionOffsetX;
    public float AutoDestroyTimer;
    public bool Istrigger;
    public Color PowerColor;

    [Header("Item pot de peinture")]
    public float number;
    public Sprite ItemSprite;
}