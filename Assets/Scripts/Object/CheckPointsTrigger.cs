using UnityEngine;
using UnityEngine.UI;

public class CheckPointsTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip _checpointSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInChildren<Animator>(true).enabled = true; // Animation de la peinture
            StartCoroutine(HUDManager.Instance.DisplaySave());
            AudioManager.Instance.PlaySFX(_checpointSound);
        }
    }
}
