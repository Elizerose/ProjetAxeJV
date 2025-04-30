using System.Collections.Generic;
using UnityEngine;
using static ColorPowerController;


[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Objects/Item")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemData> datas = new();

    public ItemData GetData(ColorAbilities color)
    {
        foreach (ItemData data in datas)
        {
            if (data.ItemColor == color)
            {
                return data;
            }
        }
        return null;
    }
}
