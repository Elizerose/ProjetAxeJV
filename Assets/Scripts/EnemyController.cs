using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Gestion et comportements des ennemis
/// 
/// </summary>
public class EnemyController : MonoBehaviour
{
    public Type EnemyType;
    private EnemyData _data;

    [SerializeField] private GameObject _feedbackImage;
    private bool _canDisplayFeedBack = true;

    private Rigidbody2D _rb2D;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    public enum STATE
    {
        NONE,
        INIT,
        IDLE,
        MOVE,
        FOLLOW,
        CHARGE,
        FIRE,
        DEATH
    }

    [SerializeField] private STATE _state = STATE.NONE;

    [Header ("Patroll")]
    public List<Transform> points; // chemins de patrouilles
    private Transform targetPoint; // target
    private int destination = 0; // index de la target
    private float directionX;
    private float cooldown;
    private bool isFacingRight = true;
    private float speed;

    // Charge ennemi 
    Vector2 startChargePosition;
    private float WarningDelay = 1f;
    private bool isInCharge = false;
    private bool CanRun = false;
    private bool CanCharge = true;

    // arbre
    [SerializeField] GameObject projectilePrefab;

    private float FireCoolDownTimer;


    void Awake()
    {
        // On récupère nos composants
        TryGetComponent(out _rb2D);
        TryGetComponent(out _collider);
        TryGetComponent(out _spriteRenderer);
        TryGetComponent(out _animator);
    }

    void Start()
    {
        // On récupère la base de donnée de l'ennemi
        _data = DatabaseManager.Instance.GetData(EnemyType);
        Init();

        // Début de la patrouille Ennemis : On initialise notre premier point (l'arbre ne bouge pas donc pas de patrouille pour lui)
        if (_data.type != Type.Arbre)
            targetPoint = points[0]; 
    }

    private void Init()
    {
        // On met l'etat de l'ennemi à INIT
        _state = STATE.INIT;

        // On récupère et assigne ses _data
        name = _data.type.ToString();
        Debug.Log(name);
        _spriteRenderer.sprite = _data.sprite;
        speed = _data.stats.speed;
        //transform.localScale = new Vector3(_data.scale, _data.scale, _data.scale);
        // on enleve et remet le collider pour le reset a la bonne taille
        _collider.enabled = false;
        _collider.enabled = true;

        _feedbackImage.GetComponent<Image>().sprite = null;
        _feedbackImage.SetActive(false);

        if (_data.type == Type.Arbre)
            FireCoolDownTimer = _data.stats.AttackCooldown;

        // Une fois terminé, on change son état pour idle
        _state = STATE.IDLE;

    }

    void Update()
    {
        // Si lennemi est en pleine initialisation, on ne fait rien
        if (_state < STATE.INIT)
            return;

        // Si le joueur est en vu, on passe son état à Follow que pour les barbares, et l'arbre attaque a vu les autres ont juste une patrouille
        if (IsPlayerInSight())
        {
            if (_canDisplayFeedBack)
                StartCoroutine(FeedBackDisplay(HUDManager.Instance.Exclamation));

            if (_data.type == Type.Barbare)
                _state = STATE.CHARGE;
            else if (_data.type == Type.Arbre)
                _state = STATE.FIRE;
        }
        else
        {
            _canDisplayFeedBack = true;
        }
        
            

        // Notre animator de l'arbre n'as pas de speed donc on évite
        if (_data.type != Type.Arbre)
            _animator.SetFloat("Speed", Mathf.Abs(_rb2D.linearVelocity.x));

        // Changement d'état
        switch (_state)
        {
            case STATE.IDLE:

                if (_data.type != Type.Arbre)
                {
                    if (cooldown > _data.waitTimePatroll)
                    {
                        if (_data.type == Type.Barbare)
                            _animator.SetBool("isCharging", false);

                        // Quand le compteur d'attente est fini, onremet le cooldown à 0, et on remet l'etat à move pour quil continue la patrouille
                        cooldown = 0;
                        _state = STATE.MOVE;
                    }
                    // On arrete l'ennemi et incremente le cooldown
                    _rb2D.linearVelocity = Vector2.zero;
                    cooldown += Time.deltaTime;
                } else
                {
                    // Si c'est notre arbre, on initialise sa direction
                    directionX = 1;
                }
                
                break;

            case STATE.MOVE:
                // on active la patouille
                Patroll();
                break;

            case STATE.FOLLOW:
                // on active la chasse
                Chase();
                break;

            case STATE.CHARGE:
                if (CanCharge)
                    Charge();   
                break;

            case STATE.FIRE:
                Fire();
                break;

            case STATE.DEATH:
                break;
        }

       
        Flip();
    }


    // --------------------------- COMMUN  ----------------------------

    // Fonction pour Flip l'ennemi
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
    

    // Fonction de Patrouille
    private void Patroll()
    {
        // calcul de la direction (sens)
        directionX = Mathf.Sign(targetPoint.position.x - transform.position.x); // Math.Sign renvoie la direction : 1 si droite / -1 si gauche / 0 si rien (distance entre notre ennemis et notre target)
                                                                                // si position.x enemis = 2 et position.x point = -5, notre joueur doit aller à gauche : - 5 - 2 = -7 Math.Sign renvoie le signe donc - 1 
         // On déplace l'ennemi
        _rb2D.linearVelocity = new Vector2(directionX * speed, _rb2D.linearVelocity.y);

        // Quand l'ennemi atteint (à 0.3f près) le points target de patrouille, on passe au point suivant
        if (Mathf.Abs(transform.position.x - targetPoint.position.x) < 0.3f)
        {
            // On passe son état à IDLE
            _state = STATE.IDLE;
            // On passe au point target suivant
            destination = (destination + 1) % points.Count; // récupère le reste pour ne jamais finir une patrouille
                                                            // ex : 0 + 1 % 3 = 1 % 3 = 1 (point suivant % points total)
                                                            // ex dernier point : 2 + 1 % 3 = 3 % 3 = 0

            targetPoint = points[destination];
        }

    }


    // Chase / Follow
    private void Chase()
    {
        // calcul de la direction (sens)
        directionX = Mathf.Sign(GameManager.Instance.Player.position.x - transform.position.x);
        // Math.Sign renvoie la direction : 1 si droite / -1 si gauche / 0 si rien (distance entre notre ennemis et notre target)
        // si position.x enemis = 2 et position.x point = -5, notre joueur doit aller à gauche : - 5 - 2 = -7 Math.Sign renvoie le signe donc - 1 

        // L'ennemi suit le joueur
        _rb2D.linearVelocity = new Vector2(directionX * speed, _rb2D.linearVelocity.y);

        // Si l'ennemi est à 1 du joueur il se stop (pour pas qu'il lui rentre dedans)
        if (Mathf.Abs(GameManager.Instance.Player.position.x - transform.position.x) < 1f)
        {
            _rb2D.linearVelocity = Vector3.zero;
        }

        // Si le joueur n'est plus en vu, il repasse en idle et reprendra sa patrouille
        if (!IsPlayerInSight())
            _state = STATE.IDLE;
    }




    // --------------------------- CHECKS ----------------------------

    // Check si le player est en vue 
    //private bool IsPlayerInSight()
    //{
    //    // on défini la 'position de centre' d'ou va se creer notre boite invisible pour regarder si le joueur est dedans
    //    float checkposition;
    //    if (_data.type == Type.Arbre) // on check devant et derriere si c'est un arbre donc on part du milieu de son transform
    //        checkposition = transform.position.x;
    //    else
    //        checkposition = transform.position.x + _data.detection.DistanceInSight / 2 * directionX; // Sinon, on prend le milieu entre le personnage et sa distance a verifier pour que la distance se creer bien de l'ennemi à la distance voulu * la direction (l'orientation de l'ennemi = devant lui)

    //    // On cree un vecteur X correspondant
    //    Vector3 sightTransform = new Vector3(checkposition, transform.position.y, transform.position.z);

    //    // On cree notre boite invisible pour detecter le joueur
    //    Collider2D[] targets = Physics2D.OverlapBoxAll(sightTransform, new Vector2(_data.detection.DistanceInSight, transform.localScale.y), 0);

    //    // Si le player est détécté dans cette box, le joueur est en vue, on return true, sinon false
    //    foreach (Collider2D target in targets)
    //    {
    //        if (target.CompareTag("Player"))
    //            return true;
    //    }

    //    return false;
    //}

    private bool IsPlayerInSight()
    {
        Vector2 origin = transform.position;

        if (_data.type == Type.Arbre)
        {
            Vector2 rightTarget = origin + Vector2.right * _data.detection.DistanceInSight;
            Vector2 leftTarget = origin + Vector2.left * _data.detection.DistanceInSight;

            Debug.DrawLine(origin, rightTarget, Color.red);
            Debug.DrawLine(origin, leftTarget, Color.cyan);

            RaycastHit2D hitFront = Physics2D.Raycast(origin, Vector2.right, _data.detection.DistanceInSight);
            RaycastHit2D hitBack = Physics2D.Raycast(origin, Vector2.left, _data.detection.DistanceInSight);

            return (hitFront.collider != null && hitFront.collider.CompareTag("Player")) ||
                   (hitBack.collider != null && hitBack.collider.CompareTag("Player"));
        }
        else
        {
            Vector2 direction = Vector2.right * directionX;
            Vector2 target = origin + direction * _data.detection.DistanceInSight;

            Debug.DrawLine(origin, target, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, _data.detection.DistanceInSight);

            return hit.collider != null && hit.collider.CompareTag("Player");
        }
    }


    // Check si l'ennemi peut attaquer
    //private bool CanAttack()
    //{
    //    // pareil qu'au dessus mais cette fois ci avec la distance d'attaque

    //    float checkposition;
    //    if (_data.type == Type.Arbre)
    //        checkposition = transform.position.x;
    //    else
    //        checkposition = transform.position.x + _data.detection.AttackDistance / 2 * directionX;

    //    Vector3 sightTransform = new Vector3(checkposition, transform.position.y, transform.position.z);

    //    Collider2D[] targets = Physics2D.OverlapBoxAll(sightTransform, new Vector2(_data.detection.AttackDistance, transform.localScale.y + 2f), 0);

    //    foreach (Collider2D target in targets)
    //    {
    //        if (target.CompareTag("Player"))
    //            return true;
    //    }

    //    return false;
    //}

    private bool CanAttack()
    {
        Vector2 origin = transform.position;

        if (_data.type == Type.Arbre)
        {
            Vector2 rightTarget = origin + Vector2.right * _data.detection.AttackDistance;
            Vector2 leftTarget = origin + Vector2.left * _data.detection.AttackDistance;

            Debug.DrawLine(origin, rightTarget, Color.yellow);
            Debug.DrawLine(origin, leftTarget, Color.green);

            RaycastHit2D hitFront = Physics2D.Raycast(origin, Vector2.right, _data.detection.AttackDistance);
            RaycastHit2D hitBack = Physics2D.Raycast(origin, Vector2.left, _data.detection.AttackDistance);

            return (hitFront.collider != null && hitFront.collider.CompareTag("Player")) ||
                   (hitBack.collider != null && hitBack.collider.CompareTag("Player"));
        }
        else
        {
            Vector2 direction = Vector2.right * directionX;
            Vector2 target = origin + direction * _data.detection.AttackDistance;

            Debug.DrawLine(origin, target, Color.yellow);

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, _data.detection.AttackDistance);

            return hit.collider != null && hit.collider.CompareTag("Player");
        }
    }


    IEnumerator FeedBackDisplay(Sprite feedback)
    {
        _canDisplayFeedBack = false;
        _feedbackImage.GetComponent<Image>().sprite = feedback;
        _feedbackImage.SetActive(true);

        yield return new WaitForSeconds(2f);
        _feedbackImage.SetActive(false);
    }


    // --------------------------- BARBARE ----------------------------

    // Charge de l'ennemi 
    private void Charge()
    {
        
        _animator.SetBool("isCharging", true);

        // on bouge plus mais change son orientation selon le player
        _rb2D.linearVelocity = Vector2.zero;
        directionX = Mathf.Sign(startChargePosition.x + 10f - transform.position.x); // calcul de l'orientation selon le joueur

        // Si notre barbare n'est pas deja en etat de charge, peut charger et ne peut pas encore nous courir dessus, il prend sa pose de charge et clignote
        if (!isInCharge && CanCharge && !CanRun)
        {
            StartCoroutine(TimeToCharge());
        }

        // booléen pour indiquer si la charge (attaque) est fini
        bool finishCharge = false;

        // Si notre ennemi peut nous foncer dessus
        if (CanRun)
        {
            // Charge ennemi
            if (Mathf.Abs(startChargePosition.x + 10f - transform.position.x) > 1f)
            {
                _rb2D.linearVelocity = new Vector2(directionX * 12, _rb2D.linearVelocity.y);
            }
            else
            {
                finishCharge = true;
            }
        }

        // Si pendant la charge le joueur n'est plus en vu, ou alors que la charge est fini, on arrete la charge et réeinitialise toutes les valeurs
        if (!IsPlayerInSight() || finishCharge)
        {
            _rb2D.linearVelocity = Vector2.zero;
            _animator.SetBool("isCharging", false);
            
            StartCoroutine(WaitBeforeCharge());
            
        }

    }

    // Coroutine de précharge
    private IEnumerator TimeToCharge()
    {
        
        isInCharge = true;
        float delay = WarningDelay;
        startChargePosition = transform.position;

        for (int i = 0;  i < 7; i++)
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(delay);
            delay -= 0.6f;
        }
        isInCharge = false;
        CanRun = true;

    }

    // Coroutine de cooldown pour la charge
    private IEnumerator WaitBeforeCharge()
    {
        Debug.Log("Wait !");
        _rb2D.linearVelocity = Vector2.zero;
        // animation regard droite gauche

        CanCharge = false;

        StartCoroutine(FeedBackDisplay(HUDManager.Instance.Interrogation));

        yield return new WaitForSeconds(4f);

        _state = STATE.IDLE;

        StopCoroutine(TimeToCharge());
        _canDisplayFeedBack = true;

        isInCharge = false;
        CanCharge = true;
        CanRun = false;


    }




    // --------------------------- ARBRE ----------------------------
    private void Fire()
    {
        // petit feedback pour dire que l'arbre nous a vu et nous regarde


        // Calcul de l'orientation de l'ennemi
        directionX = Mathf.Sign(GameManager.Instance.Player.position.x - transform.position.x); // calcul de l'orientation selon le joueur

        // feedback temps de chargement du projectile
        if (CanAttack() && FireCoolDownTimer<=0f)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            ProjectileController controller = projectile.GetComponent<ProjectileController>();
            if (controller != null)
            {
                controller.SetDirection(directionX * Vector2.right);
                controller.SetTarget("Player");
                FireCoolDownTimer = _data.stats.AttackCooldown;
            }
            else
            {
                Debug.LogError("pas de scirpt trouvé.");
            }
            
        }

        FireCoolDownTimer -= Time.deltaTime;


    }




    // --------------------------- DEBUG ----------------------------
    //private void OnDrawGizmos()
    //{
    //    if (_data != null)
    //    {
    //        float checkposition;
    //        if (_data.type == Type.Arbre) // on check devant et derriere !
    //            checkposition = transform.position.x;
    //        else
    //            checkposition = transform.position.x + _data.detection.DistanceInSight / 2 * directionX;

    //        Vector3 sightTransform = new Vector3(checkposition, transform.position.y, transform.position.z);
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireCube(sightTransform, new Vector3(_data.detection.DistanceInSight * directionX, 2f, 1f));

    //        //Vector3 attackTransform = new Vector3(transform.position.x + _data.detection.AttackDistance / 2 * directionX, transform.position.y, transform.position.z);
    //        //Gizmos.color = Color.blue;
    //        //Gizmos.DrawWireCube(attackTransform, new Vector3(_data.detection.AttackDistance * directionX, transform.localScale.y / 2f, 1f));
    //    }
    //}
}
