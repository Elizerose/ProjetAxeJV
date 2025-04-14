using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayerController : MonoBehaviour
    
{
    public static PlayerController Instance { get; private set; }


    private Rigidbody2D _rb2D;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    [Header("MOVE")]
    [SerializeField]private float moveSpeed;
    private bool _isFacingRight = true;

    [Header("JUMP")]
    [SerializeField] private float jumpForce;
    private int _currentJump;

    [Header("WALL")]
    [SerializeField] private float _distanceDW;
    [SerializeField] private LayerMask _DetectWall;

    [Header("IVY")]
    private bool isOnIvy = false;
    [HideInInspector] public bool isOnVine = false;
    private GameObject currentIvy;
    private GameObject lastIvy;

    [Header("SWIM")]
    private bool isInWater = false;
    public int swimMode = 0;
    [SerializeField] private float swimForce;
    private Vector2 lastDirectionInput;
    private float lastDirectionTimer;
    [SerializeField] private float fallSpeed;




    // Initialisation des components
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        TryGetComponent(out _rb2D);
        TryGetComponent(out _animator);
        TryGetComponent(out _playerTransform);
        TryGetComponent(out _spriteRenderer);

    }

    void Update()
    {
        if (Input.GetButton("Horizontal") && !(isInWater && swimMode==1) )
        {
            MoveX();
        }
        
        if (!Input.GetButton("Horizontal") && !(isInWater && swimMode == 1))
        {
            _rb2D.linearVelocity = new Vector2(0, _rb2D.linearVelocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _currentJump < 1 && !isOnIvy && !isInWater)
        {
            Jump();
        }

        if (isOnIvy || (isInWater && swimMode == 0))
        {
            MoveY();     
        }
        else if (isInWater || swimMode == 1)
        {
            SwimByImpulses();
        }

        _animator.SetFloat("Speed", Mathf.Abs(_rb2D.linearVelocity.x));
        
        Flip();
    }

    void MoveX()
    {

        float direction = Input.GetAxisRaw("Horizontal");// * Vector2.right;

        // Detection du mur (on s'arrete)

        // Deplacements
        //_playerTransform.position += direction * moveSpeed * Time.deltaTime;
        _rb2D.linearVelocity = new Vector2(direction * moveSpeed, _rb2D.linearVelocity.y);


        // Prb avec cette methode : on s'arrete devant les pentes ..
        var hit = Physics2D.BoxCast(transform.position, Vector2.one, 0, direction * Vector2.right, _distanceDW, _DetectWall);

        if (hit.collider != null)
        {
            if (!hit.collider.CompareTag("Slope"))
                return;
        }

        
    }

    void Flip()
    {
        // Flip
        if ((_isFacingRight && Input.GetAxis("Horizontal") < 0f || !_isFacingRight && Input.GetAxis("Horizontal") > 0f))
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }

    void Jump()
    {
        _animator.SetTrigger("Jump");
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

            velocity = new Vector2(_rb2D.linearVelocity.x, direction.y * moveSpeed);
        }
        // Espace aussi pour monter dans l'eau
        else if (Input.GetButton("Jump") && isInWater)
        {
            velocity = new Vector2(_rb2D.linearVelocity.x, 1 * moveSpeed);
        }
        // On appuie sur A pour relacher
        else if (Input.GetKeyDown(KeyCode.A) && isOnIvy)
        {
            if (currentIvy != null)
            {
                currentIvy.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(WaitForSecond());
            }
        }
        // Si on ne se deplace pas
        else
        {
            // Si on est dans l'eau, on coule
            if (isInWater)
                velocity = new Vector2(0, -1) * fallSpeed;

            // Si on est sur le lierre on ne bouge pas
            else if (isOnIvy)
                velocity = new Vector2(0, 0);
        }

        _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, velocity.y);

    }

    private void SwimByImpulses()
    {
        
        if (Input.GetButtonDown("Vertical"))
        {
            lastDirectionInput = Input.GetAxisRaw("Vertical") * Vector2.up;
            lastDirectionTimer = Time.time;
        }
        else if (Input.GetButtonDown("Horizontal"))
        {
            lastDirectionInput = Input.GetAxisRaw("Horizontal") * Vector2.right;
            lastDirectionTimer = Time.time;
        }
        // On coule
        else
        {
            _rb2D.linearVelocity = Vector2.Lerp(_rb2D.linearVelocity, Vector2.down * fallSpeed, Time.deltaTime * 2f);
           
        }

        // Si une direction a été choisie et que l'espace est pressé
        if (lastDirectionInput != Vector2.zero && Input.GetKeyDown(KeyCode.Space) && (Time.time - lastDirectionTimer < 3f))
        {
            _rb2D.linearVelocity = lastDirectionInput * swimForce;

            lastDirectionInput = Vector2.zero; // Réinitialisation la direction 
            lastDirectionTimer = 0f; // Réinitialisation du timer
        }
        // si le temps est ecoulé, il a pas appuyé sur espace assez vite, on reset
        else if (Time.time - lastDirectionTimer >= 3f)
        {
            lastDirectionTimer = 0f; 
            lastDirectionInput = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si on detecte un sol
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _currentJump = 0;
        }
            
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        // Si on detecte un lierre (echelle)
        if (collision.gameObject.CompareTag("Ivy"))
        {
            currentIvy = collision.gameObject;
            lastIvy = collision.gameObject;
            _currentJump = 0;
            isOnIvy = true;
            _rb2D.gravityScale = 0; // on met la gravité à 0 pour pas tomber
        }
        // Si on detecte de l'eau
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            PanelController.Instance.ActiveFilter("water");
            isInWater = true;
            _rb2D.gravityScale = 0;
        }
    }

    // Si on sort de nos zones lierre et eau on reset nos variables
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ivy") || collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            currentIvy = null;
            isOnIvy = false;
            isInWater = false;
            _rb2D.gravityScale = 1;
            _rb2D.linearVelocity = new Vector2(0, 0);
            PanelController.Instance.DeactiveFilter();
        }
    }

    private IEnumerator WaitForSecond()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log(lastIvy);
        lastIvy.GetComponent<Collider2D>().enabled = true;
    }
}
