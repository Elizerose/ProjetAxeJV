using UnityEngine;

public class ItemManager : MonoBehaviour
{
    PlateformesData _data;

    public void AddItem(ColorPowerController.ColorAbilities color)
    {
        _data = DatabaseManager.Instance.GetPlateformesData(color);

        _data.number += 1;
    }

    public void RemoveItem(ColorPowerController.ColorAbilities color)
    {
        _data = DatabaseManager.Instance.GetPlateformesData(color);

        _data.number -= 1;
    }
}
