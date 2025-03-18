using UnityEngine;


public class PlayerController : MonoBehaviour
    
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
            Move();
        }
        if (Input.GetKeyDown(KeyCode.Space) && _currentJump < 1)
        {
            Jump();
        }
    }

    void Move()
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

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // GetMask("Ground","Wall")

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            _currentJump = 0;
    }




}
