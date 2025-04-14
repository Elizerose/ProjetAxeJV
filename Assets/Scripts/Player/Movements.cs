using UnityEngine;

public class Movements : MonoBehaviour
{
    public float MoveSpeed = 4f;
    public float JumpForce = 15f;
    private Rigidbody2D rb;
    private Vector3 PlayerScale;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerScale = GetComponent<Transform>().localScale; 
    }

    void Update()
    {
        if (gameObject.GetComponent<Water>().InWater == false)
        {
            Move();
            Jump();
            Debug.DrawRay(transform.position + Vector3.down * 0.5f, Vector2.down * 1f, Color.red); 
        }
    }

    void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * MoveSpeed, rb.linearVelocity.y); 

        if (horizontalInput > 0)
            transform.localScale = new Vector3(PlayerScale.x, PlayerScale.y, PlayerScale.z);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-PlayerScale.x, PlayerScale.y, PlayerScale.z);

    }

    bool CheckIfGround()
    {
        Vector2 rayOrigin = transform.position + Vector3.down * 0.5f; 
        RaycastHit2D ray = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, LayerMask.GetMask("Ground"));

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
