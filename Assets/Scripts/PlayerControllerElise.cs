using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayerControllerElise : MonoBehaviour
    
{
    public static PlayerControllerElise Instance { get; private set; }


    private Rigidbody2D _rb2D;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;

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
        TryGetComponent(out _playerTransform);
        TryGetComponent(out _spriteRenderer);

    }

    void Update()
    {
        if (Input.GetButton("Horizontal") && !(isInWater && swimMode==1))
        {
            MoveX();
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
        if ((_isFacingRight && Input.GetAxis("Horizontal") < 0f || !_isFacingRight && Input.GetAxis("Horizontal") > 0f))
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }

        //_spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0;

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
        // On appuie sur A pour relacher
        else if (Input.GetKeyDown(KeyCode.A) && isOnIvy)
        {
            if (currentIvy != null)
            {
                currentIvy.GetComponent<Collider2D>().enabled = false;
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

        _rb2D.linearVelocity = velocity;

    }

    private void SwimByImpulses()
    {
        // Debug pour vérifier que les touches sont détectées dans Update
        if (Input.GetKeyDown(KeyCode.Z))
        {
            lastDirectionInput = Vector2.up * swimForce;
            //_rb2D.linearVelocity = lastDirectionInput;
            lastDirectionTimer = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lastDirectionInput = Vector2.right * swimForce;
            //_rb2D.linearVelocity = lastDirectionInput;
            lastDirectionTimer = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            lastDirectionInput = Vector2.left * swimForce;
            //_rb2D.linearVelocity = lastDirectionInput;
            lastDirectionTimer = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastDirectionInput = Vector2.down * swimForce;
            //_rb2D.linearVelocity = lastDirectionInput;
            lastDirectionTimer = Time.time;
        }

        else
        {
            _rb2D.linearVelocity = Vector2.Lerp(_rb2D.linearVelocity, Vector2.down * fallSpeed, Time.deltaTime * 2f);
        }

        // Si une direction a été choisie et que l'espace est pressé
        if (lastDirectionInput != Vector2.zero && Input.GetKeyDown(KeyCode.Space) && (Time.time - lastDirectionTimer < 3f))
        {
            Debug.Log("Impulsion appliquée : " + lastDirectionInput);
            _rb2D.linearVelocity = lastDirectionInput;

            lastDirectionInput = Vector2.zero; // Réinitialiser la direction après application
            lastDirectionTimer = 0f; // Réinitialiser le timer
        }
        else if (Time.time - lastDirectionTimer >= 3f)
        {
            lastDirectionTimer = 0f; // Réinitialiser le timer
            lastDirectionInput = Vector2.zero; // Réinitialiser la direction après application
        }
    }

    private IEnumerator WaitForSwim()
    {
        yield return new WaitForSeconds(0.5f);
        _rb2D.linearVelocity = new Vector2(0, -1) * fallSpeed;
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
            currentIvy = collision.gameObject;
            _currentJump = 0;
            isOnIvy = true;
            _rb2D.gravityScale = 0; // on met la gravité à 0 pour pas tomber
        }
        // Si on detecte de l'eau
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            PanelControllerElise.Instance.ActiveFilter("water");
            isInWater = true;
            _rb2D.gravityScale = 0;
        }

        //if (isInWater)
        //{
        //    _rb2D.gravityScale = 0.1f;
        //}
    }

    // Si on sort de nos zones lierre et eau on reset nos variables
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ivy") || collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (currentIvy != null && currentIvy != collision.gameObject)
                currentIvy.GetComponent<Collider2D>().enabled = true;
            currentIvy = null;
            isOnIvy = false;
            isInWater = false;
            _rb2D.gravityScale = 1;
            _rb2D.linearVelocity = new Vector2(0, 0);
            PanelControllerElise.Instance.DeactiveFilter();
        }
    }
}
