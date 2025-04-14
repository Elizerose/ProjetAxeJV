using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool CanSwim = true;
    public bool InWater = false;
    public float ImpulseSpeed = 3f;
    public float WaterGrav = 0.1f;
    private float NormalGrav;
    private bool CanImpulse = true;
    public float ImpulseCDTime = 0.35f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        NormalGrav = rb.gravityScale; 
    }

    void Update()
    {
        if (InWater && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Swim());
        }
    }

    private IEnumerator Swim()
    {
        if (CanImpulse)
        {
            Debug.Log("CanImpulse");
            CanImpulse = false;
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                Debug.Log("swim");
                rb.linearVelocity = new Vector2(horizontal * ImpulseSpeed, vertical * ImpulseSpeed);
            }
            yield return new WaitForSeconds(ImpulseCDTime);
            CanImpulse = true;
        }

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
