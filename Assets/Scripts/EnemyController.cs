using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    public string EnemyType;
    private EnemyData data;


    private Rigidbody2D _rb2D;
    private Collider2D _collider;
    private Transform _enemyTransform;
    private SpriteRenderer _spriteRenderer;
    
    public enum STATE
    {
        NONE,
        INIT,
        IDLE,
        MOVE,
        FOLLOW,
        FIRE,
        DEATH
    }

    [SerializeField] private STATE _state = STATE.NONE;

    [Header ("Patroll")]
    public List<Transform> points; // chemins de patrouilles
    private Transform targetPoint; // target
    private int destination = 0; // index de la target
    private bool isWaiting;
    private float directionX;
    
    private float cooldown;
    private float WaitTimePatroll;
    private bool isFacingRight = true;
    private float speed;


    void Awake()
    {
        TryGetComponent(out _rb2D);
        TryGetComponent(out _collider);
        TryGetComponent(out _enemyTransform);
        TryGetComponent(out _spriteRenderer);
    }

    void Start()
    {
        data = DatabaseManager.Instance.GetData(EnemyType);
        Init();

        // Début de la patrouille Ennemis :
        targetPoint = points[0]; 
    }

    private void Init()
    {
        _state = STATE.INIT;

        name = data.type;
        _spriteRenderer.sprite = data.sprite;
        speed = data.speed;
        transform.localScale = new Vector3(data.scale, data.scale, data.scale);
        // on enleve et remet le collider pour le reset a la bonne taille
        _collider.enabled = false;
        _collider.enabled = true;

        _state = STATE.IDLE;

    }

    void Update()
    {
        if (_state < STATE.INIT)
            return;

        if (IsPlayerInSight())
            _state = STATE.FOLLOW;

        switch (_state)
        {
            case STATE.IDLE:
                if (cooldown > data.waitTimePatroll)
                {
                    cooldown = 0;
                    _state = STATE.MOVE;
                    
                }
                _rb2D.linearVelocity = Vector2.zero;
                cooldown += Time.deltaTime;
                break;

            case STATE.MOVE:
                Patroll();
                break;

            case STATE.FOLLOW:
                Debug.Log("joueur en vue");
                Chase();
                break;

            case STATE.FIRE:
                break;

            case STATE.DEATH:
                break;
        }

       
        Flip();
    }

    private void Flip()
    {
        if ((isFacingRight && directionX < 0f || !isFacingRight && directionX > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }

    private void Patroll()
    {
        // calcul de la direction
        directionX = Mathf.Sign(targetPoint.position.x - transform.position.x); // Math.Sign renvoie la direction : 1 si droite / -1 si gauche / 0 si rien (distance entre notre ennemis et notre target)
                                                                                // si position.x enemis = 2 et position.x point = -5, notre joueur doit aller à gauche : - 5 - 2 = -7 Math.Sign renvoie le signe donc - 1 

        _rb2D.linearVelocity = new Vector2(directionX * speed, _rb2D.linearVelocity.y); // On déplace l'ennemis

        // Quand l'ennemi atteint (à 0.3f près) le points target de patrouille, on passe au point suivant
        if (Mathf.Abs(transform.position.x - targetPoint.position.x) < 0.3f)
        {
            // On arrete l'ennemi au point pendant quelques secondes, avant qu'il reprenne sa route
            _state = STATE.IDLE;
            // On passe au point target suivant
            destination = (destination + 1) % points.Count; // récupère le reste pour ne jamais finir une patrouille
                                                            // ex : 0 + 1 % 3 = 1 % 3 = 1 (point suivant % points total)
                                                            // ex dernier point : 2 + 1 % 3 = 3 % 3 = 0

            targetPoint = points[destination];
        }

    }

    // Check si le player est en vue 
    private bool IsPlayerInSight()
    {
        Vector3 sightTransform = new Vector3(transform.position.x + data.DistanceInSight / 2 * directionX, transform.position.y, transform.position.z);
        Collider2D[] targets = Physics2D.OverlapBoxAll(sightTransform, new Vector2(data.DistanceInSight * directionX, transform.localScale.y + 2f), 0);

        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }


    private void Chase()
    {
        // Check si le joueur est assez proche pour l'attaquer
        Vector3 attackTransform = new Vector3(transform.position.x + data.AttackDistance / 2 * directionX, transform.position.y, transform.position.z);
        Collider2D[] targets = Physics2D.OverlapBoxAll(attackTransform, new Vector2(data.AttackDistance * directionX, transform.localScale.y + 2f), 0);

        // Chasse du joueur
        directionX = Mathf.Sign(GameManager.Instance.Player.position.x - transform.position.x);
        // Math.Sign renvoie la direction : 1 si droite / -1 si gauche / 0 si rien (distance entre notre ennemis et notre target)
        // si position.x enemis = 2 et position.x point = -5, notre joueur doit aller à gauche : - 5 - 2 = -7 Math.Sign renvoie le signe donc - 1 

        _rb2D.linearVelocity = new Vector2(directionX * speed, _rb2D.linearVelocity.y);

        if (Mathf.Abs(GameManager.Instance.Player.position.x - transform.position.x) < 1f)
        {
            _rb2D.linearVelocity = Vector3.zero;
        }

        if (!IsPlayerInSight())
            _state = STATE.IDLE;

    }


    //private void OnDrawGizmos()
    //{
    //    Vector3 sightTransform = new Vector3(transform.position.x + data.DistanceInSight / 2 * directionX, transform.position.y, transform.position.z);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(sightTransform, new Vector3(data.DistanceInSight * directionX, transform.localScale.y + 2f, 1f));

    //    Vector3 attackTransform = new Vector3(transform.position.x + data.AttackDistance / 2 * directionX, transform.position.y, transform.position.z);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(attackTransform, new Vector3(data.AttackDistance *directionX, transform.localScale.y + 2f, 1f));
    //}
}
