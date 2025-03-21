using UnityEngine;

public class Water : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool CanSwim = true;
    public bool InWater = false;
    public float SwimSpeed = 2f;
    public float WaterGrav = 1f;
    private float NormalGrav;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        NormalGrav = rb.gravityScale; 
    }

    void Update()
    {
        if (InWater)
        {
            Swim();
        }
    }

    private void Swim()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(horizontal * SwimSpeed, vertical * SwimSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water") && CanSwim)
        {
            InWater = true;
            rb.gravityScale = WaterGrav;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water") && CanSwim)
        {
            InWater = false;
            rb.gravityScale = NormalGrav;
        }
    }
}
