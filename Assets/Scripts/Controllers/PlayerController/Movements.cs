using UnityEngine;

public class Movements : MonoBehaviour
{
    public float MoveSpeed = 4f;
    public float JumpForce = 15f;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        Debug.DrawRay(transform.position + Vector3.down * 0.5f, Vector2.down * 0.6f, Color.red); 
    }

    void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * MoveSpeed, rb.linearVelocity.y); 

        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

    }

    bool CheckIfGround()
    {
        Vector2 rayOrigin = transform.position + Vector3.down * 0.5f; 
        RaycastHit2D ray = Physics2D.Raycast(rayOrigin, Vector2.down, 0.6f, LayerMask.GetMask("Ground"));

        return ray.collider != null;
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CheckIfGround())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }
    }
}
