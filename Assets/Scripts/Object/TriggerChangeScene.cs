using UnityEngine;

public class TriggerChangeScene : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SceneManager.Instance.ChangeScene(_sceneIndex);
    }
}
