using UnityEngine;

public class CheckBounces : MonoBehaviour
{
    [SerializeField] private AudioClip _bounceSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            AudioManager.Instance.PlaySFX(_bounceSound);
    }
}
