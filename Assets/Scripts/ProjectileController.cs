using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileController : MonoBehaviour
{
    private float speed = 5f;
    private Vector3 moveDirection;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * moveDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // degats
        }
        Destroy(gameObject);
    }


}
