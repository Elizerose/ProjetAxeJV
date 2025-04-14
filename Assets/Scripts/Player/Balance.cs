using UnityEngine;

public class Balance : MonoBehaviour
{
    private HingeJoint2D hinge;
    private Rigidbody2D rb;
    private bool IsSwinging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (IsSwinging && Input.GetKeyDown(KeyCode.Space))
        {
            Release();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BVine") && hinge == null)
        {
            Attach(collision.gameObject);
        }
    }

    void Attach(GameObject segment)
    {
        Rigidbody2D segmentRB = segment.GetComponent<Rigidbody2D>();
        if (segmentRB == null) return; 

        hinge = gameObject.AddComponent<HingeJoint2D>();
        hinge.connectedBody = segmentRB;
        hinge.autoConfigureConnectedAnchor = false;
        hinge.anchor = Vector2.zero;
        hinge.connectedAnchor = segmentRB.transform.InverseTransformPoint(transform.position);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;
        IsSwinging = true;
    }

    void Release()
    {
        if (hinge != null) 
        {
            Destroy(hinge);
            hinge = null; 
            IsSwinging = false;
        }
    }
}
