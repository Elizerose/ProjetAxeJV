using UnityEngine;
using UnityEngine.UI;

public class CheckPointsTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInChildren<Animator>(true).enabled = true;
        }
    }
}
