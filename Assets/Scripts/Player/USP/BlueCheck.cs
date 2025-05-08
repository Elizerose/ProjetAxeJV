using UnityEngine;

/// <summary>
/// Script a attacher au prefab de notre plateforme bleu.
/// check si la plateforme peut etre plac�e
/// </summary>



public class BlueChecks : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
            GameManager.Instance.Player.GetComponent<PlateformPlacement>()._canPlace = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
            GameManager.Instance.Player.GetComponent<PlateformPlacement>()._canPlace = false;
    }
}
