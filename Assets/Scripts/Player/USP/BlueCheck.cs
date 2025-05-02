using UnityEngine;

/// <summary>
/// Script a attacher au prefab de notre plateforme bleu.
/// check si la plateforme peut etre placée
/// </summary>



public class BlueChecks : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
            GameManager.Instance.Player.GetComponent<PlateformPlacement>()._canPlace = true;
        else
            GameManager.Instance.Player.GetComponent<PlateformPlacement>()._canPlace = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
            GameManager.Instance.Player.GetComponent<PlateformPlacement>()._canPlace = false;
    }
}
