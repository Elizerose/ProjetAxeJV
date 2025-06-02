using UnityEngine;
using UnityEngine.UI;

public class CheckPointsTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip _checpointSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.ItemList.Clear();// On clear la liste car on passe le checkpoint

            GetComponentInChildren<Animator>(true).enabled = true; // Animation de la peinture
            StartCoroutine(HUDManager.Instance.DisplaySave());
            AudioManager.Instance.PlaySFX(_checpointSound);

            GameManager.Instance.Player.GetComponent<PlayerHealth>().Lastcheckpoint = transform;
        }
    }
}
