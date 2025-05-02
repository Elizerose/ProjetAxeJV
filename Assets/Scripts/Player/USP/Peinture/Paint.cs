using Unity.VisualScripting;
using UnityEngine;

public class Paint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // on récupère l'objet
            // on le détruit
        }
    }
}
