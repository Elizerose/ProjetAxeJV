using UnityEngine;
using UnityEngine.Rendering;


public class PlayerControllerElise : MonoBehaviour
    
{
    private Rigidbody2D _rb2D;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;

    [Header("MOVE")]
    [SerializeField]private float moveSpeed;

    [Header("JUMP")]
    [SerializeField] private float jumpForce;
    private int _currentJump;

    [Header("WALL")]
    [SerializeField] private float _distanceDW;
    [SerializeField] private LayerMask _DetectWall;

    [Header("IVY")]
    private bool isOnIvy = false;

    private bool isInWater = false;

    // Initialisation des components
    void Awake()
    {
        TryGetComponent(out _rb2D);
        TryGetComponent(out _playerTransform);
        TryGetComponent(out _spriteRenderer);

    }

    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            MoveX();
        }
        if (Input.GetKeyDown(KeyCode.Space) && _currentJump < 1 && !isOnIvy && !isInWater)
        {
            Jump();
        }

        if (isOnIvy || isInWater)
        {
            MoveY();     
        }
    }

    void MoveX()
    {

        Vector3 direction = Input.GetAxisRaw("Horizontal") * Vector2.right;

        // Detection du mur (on s'arrete)
        var hit = Physics2D.BoxCast(transform.position, Vector2.one, 0 , direction, _distanceDW, _DetectWall);

        if (hit.collider != null)
        {
            return;
        }

        // Flip
        if (!Input.GetKey(KeyCode.LeftControl))
            _spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0;

        // Deplacements
        _playerTransform.position += direction * moveSpeed * Time.deltaTime;
    }

    void Jump()
    {
        _rb2D.linearVelocity = Vector2.up * jumpForce;
        _currentJump++;
    }

    void MoveY()
    {
        Vector2 velocity = new Vector2();

        // Z pour monter et S pour descendre dans l'eau et sur le lierre
        if (Input.GetButton("Vertical"))
        {
            Vector3 direction = Input.GetAxisRaw("Vertical") * Vector2.up;

            velocity = new Vector2(0, direction.y * moveSpeed);
        }
        // Espace aussi pour monter dans l'eau
        else if (Input.GetButton("Jump") && isInWater)
        {
            velocity = new Vector2(0, 1 * moveSpeed);
        }
        // Si on ne se deplace pas
        else
        {
            // Si on est dans l'eau, on coule
            if (isInWater)
                velocity = new Vector2(0, -1);

            // Si on est sur le lierre on ne bouge pas
            else if (isOnIvy)
                velocity = new Vector2(0, 0);
        }

        _rb2D.linearVelocity = velocity;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si on detecte un sol
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            _currentJump = 0;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        // Si on detecte un lierre (echelle)
        if (collision.gameObject.CompareTag("Ivy"))
        {
            isOnIvy = true;
            _rb2D.gravityScale = 0; // on met la gravité à 0 pour pas tomber
        }
        // Si on detecte de l'eau
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            isInWater = true;
        }
    }

    // Si on sort de nos zones lierre et eau on reset nos variables
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ivy") || collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            isOnIvy = false;
            isInWater = false;
            _rb2D.gravityScale = 1;
            _rb2D.linearVelocity = new Vector2(0, 0);
        }
    }






}
