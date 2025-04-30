using TMPro;
using UnityEngine;
using static ColorPowerController;

public class ItemController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private ItemData _data;
    public ColorAbilities itemColor;

    private void Awake()
    {
        TryGetComponent(out _spriteRenderer);
    }
    void Start()
    {
        _data = DatabaseManager.Instance.GetItemData(itemColor);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
