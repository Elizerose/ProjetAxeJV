using UnityEngine;



/// <summary>
/// Script a attacher au prefab de notre plateforme du pouvoir jaune.
/// 
/// check si le placement de la plateforme est correct ou non
/// 
/// 
/// </summary>



public class Checks : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.Player.GetComponent<PlateformPlacement>()._canPlace = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            GameManager.Instance.Player.GetComponent<PlateformPlacement>()._canPlace = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            GameManager.Instance.Player.GetComponent<PlateformPlacement>()._canPlace = true;
    }
}
