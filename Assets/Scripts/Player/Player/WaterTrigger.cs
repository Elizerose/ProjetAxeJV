using UnityEngine;


/// <summary>
/// Check si le player est dans l'eau
/// </summary>
/// 
public class WaterTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWaterCheck"))
            collision.transform.parent.GetComponent<Water>().EnterWater();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWaterCheck"))
            collision.transform.parent.GetComponent<Water>().ExitWater();
    }
}
