using TMPro;
using UnityEngine;
using static ColorPowerController;

public class ItemController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlateformesData _data;
    public ColorAbilities itemColor;

    private void Awake()
    {
        TryGetComponent(out _spriteRenderer);
    }
    void Start()
    {
        _data = DatabaseManager.Instance.GetPlateformesData(itemColor);
        if (_data.ItemSprite != null) 
            GetComponent<SpriteRenderer>().sprite = _data.ItemSprite;
        GetComponent<SpriteRenderer>().color = _data.PowerColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _data.number += 1;
            gameObject.SetActive(false);
        }
    }
}
