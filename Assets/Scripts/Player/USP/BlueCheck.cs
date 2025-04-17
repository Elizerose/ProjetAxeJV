using UnityEngine;



/// <summary>
/// Script a attacher au prefab de notre plateforme bleu.
/// 
/// check si la plateforme peut etre placée
/// 
/// 
/// </summary>



public class BlueChecks : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
            GameManager.Instance.Player.GetComponent<PlayerAbilities>()._canPlace = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.Instance.Player.GetComponent<PlayerAbilities>()._canPlace = false;
    }
}
