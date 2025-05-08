using UnityEngine;

/// <summary>
///  Check si le player a rebondit sur la plateforme
/// </summary>

public class BounceCheck : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("le joueur a sauté dessus");
            // Si le joueur sort du trigger c'est qu'il a rebondit alors on peut détruire le GameObject
            GameManager.Instance.Player.GetComponent<PlateformPlacement>().HasBounce = true;
            Destroy(gameObject);
        }
    }
}
