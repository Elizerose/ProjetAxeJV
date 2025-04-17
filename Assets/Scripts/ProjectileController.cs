using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 
/// Gestion des projectiles
/// 
/// </summary>
public class ProjectileController : MonoBehaviour
{
    private float speed = 5f;
    private Vector3 moveDirection;
    private string _target;

    // Set l'orientation dans laquelle va aller le projectile
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    // A qui fait il des degats ?
    public void SetTarget(string target)
    {
        _target = target;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * moveDirection;
    }

    // Gestion des degats selon les target
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_target != null)
        {
            if (collision.CompareTag(_target))
            {
                // degats
                Destroy(gameObject);
                GameManager.Instance.Death(GameManager.DeathCauses.Enemy);
            }
        }
        
        
    }


}
