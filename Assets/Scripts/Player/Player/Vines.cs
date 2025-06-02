using UnityEngine;

public class Vines : MonoBehaviour
{
    public bool IsClimbing = false;
    public float ClimbSpeed = 3f;
    private Rigidbody2D rb;
    private bool isTouchingVine = false; 
    private Transform currentVine;

    [SerializeField] private AudioClip _climbSound;
    private bool _isSoundPlay = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isTouchingVine)
        {
            if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
                MoveY();
            else
            {
                rb.linearVelocity = Vector2.zero;

                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().Stop();
                _isSoundPlay = false;
            }
        }
        

        //    if (isTouchingVine) //&& Input.GetKeyDown(KeyCode.Space))
        //    {
        //        //IsClimbing = !IsClimbing; 
        //        //rb.gravityScale = IsClimbing ? 0 : 1;
        //        IsClimbing = true;
        //        rb.gravityScale = 0;
        //        rb.linearVelocity = Vector2.zero;
        //    }

        //    if (IsClimbing)
        //    {
        //        float vertical = Input.GetAxisRaw("Vertical");
        //        float horizontal = Input.GetAxisRaw("Horizontal");

        //        rb.linearVelocity = new Vector2(horizontal * ClimbSpeed, vertical * ClimbSpeed);
        //    }
    }

    private void MoveY()
    {
        if (!_isSoundPlay)
        {
            _isSoundPlay = true;
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().PlayOneShot(_climbSound);
        }
        

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.gravityScale = 0;
        

        rb.linearVelocity = new Vector2(horizontal * ClimbSpeed, vertical * ClimbSpeed);


        if (!GetComponent<Movements>().CheckIfGround())
            HUDManager.Instance.DisplayClimbCmd(false);

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            HUDManager.Instance.DisplayClimbCmd(true);
            isTouchingVine = true;
            currentVine = other.transform; 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Vine"))
        {
            HUDManager.Instance.DisplayClimbCmd(false);
            isTouchingVine = false;
            IsClimbing = false;
            rb.gravityScale = 1;
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Stop();
            _isSoundPlay = false;
        }
    }
}