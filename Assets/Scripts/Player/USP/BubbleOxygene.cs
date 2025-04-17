using UnityEngine;

/// <summary>
/// Check si le player est dans la bulle d'oxygene
/// </summary>
public class BubbleOxygene : MonoBehaviour
{
    // S'il est dans la bulle : il peut respirer on oublie le timer
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ColorPowerController.Instance.CanInvokePaletteUnderWater = true;
            GameManager.Instance.Player.GetComponent<Water>().CanBreatheUnderWater = true;
        }
            
    }

    // sil sort de la bulle, il ne peut plus respirer
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ColorPowerController.Instance.CanInvokePaletteUnderWater = false;
            GameManager.Instance.Player.GetComponent<Water>().CanBreatheUnderWater = false;
        }
            
    }
}
