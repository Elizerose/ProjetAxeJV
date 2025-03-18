using UnityEngine;

public class Movements : MonoBehaviour
{
    public float MoveSpeed = 4f;
    public float JumpForce = 15f;
    private bool TouchFloor = true;
    private Rigidbody2D rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Move();
        Jump();
    }
    void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * MoveSpeed, rb.linearVelocityY);
    }

    void Jump()
{
    if (Input.GetKeyDown(KeyCode.Space) && TouchFloor)
    {    
        rb.linearVelocity = new Vector2(rb.linearVelocityX, JumpForce);            
    }
}
}
