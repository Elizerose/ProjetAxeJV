using Unity.VisualScripting;
using UnityEngine;

public class Paint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // on r�cup�re l'objet
            // on le d�truit
        }
    }
}
