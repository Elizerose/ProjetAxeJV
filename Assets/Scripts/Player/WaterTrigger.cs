using UnityEngine;


/// <summary>
/// Check si le player est dans l'eau
/// </summary>
/// 
public class WaterTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<Water>().EnterWater();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<Water>().ExitWater();
    }
}
