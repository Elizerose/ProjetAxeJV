using UnityEngine;

public class Vines : MonoBehaviour
{
    public bool IsClimbing = false;
    public float ClimbSpeed = 3f;
    private Rigidbody2D rb;
    private bool isTouchingVine = false; 
    private Transform currentVine; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isTouchingVine && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space)))
        {
            IsClimbing = !IsClimbing; 
            rb.gravityScale = IsClimbing ? 0 : 1;
            rb.linearVelocity = Vector2.zero; 
        }

        if (IsClimbing)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector2(0, vertical * ClimbSpeed);

            if (currentVine != null)
            {
                transform.position = new Vector2(currentVine.position.x, transform.position.y);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            isTouchingVine = true;
            currentVine = other.transform; 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            isTouchingVine = false;
            if (IsClimbing)
            {
                IsClimbing = false;
                rb.gravityScale = 1; 
            }
        }
    }
}