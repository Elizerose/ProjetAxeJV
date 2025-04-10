using System.Collections;
using System.Collections.Generic;
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
    private float speed;

    [Header ("Patroll")]
    public List<Transform> points; // chemins de patrouilles
    private Transform targetPoint; // target
    private int destination = 0; // index de la target
    private bool isWaiting;
    private float directionX;
    private float WaitTimePatroll;
    private bool isFacingRight = true;



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
        name = data.type;
        _spriteRenderer.sprite = data.sprite;
        speed = data.speed;
        WaitTimePatroll = data.waitTimePatroll;
        transform.localScale = new Vector3(data.scale, data.scale, data.scale);
        // on enleve et remet le collider pour le reset a la bonne taille
        _collider.enabled = false;
        _collider.enabled = true;

    }

    void Update()
    {
        Patroll();
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

    IEnumerator WaitForPatroll(float waitTime)
    {
        isWaiting = true;
        _rb2D.linearVelocity = Vector2.zero; // On arrete l'ennemi
        yield return new WaitForSeconds(waitTime); //On attend X secondes
        isWaiting = false;
    }

    private void Patroll()
    {
        if (isWaiting)
            return;

        // calcul de la direction
        directionX = Mathf.Sign(targetPoint.position.x - transform.position.x); // Math.Sign renvoie la direction : 1 si droite / -1 si gauche / 0 si rien (distance entre notre ennemis et notre target)
                                                                                // si position.x enemis = 2 et position.x point = -5, notre joueur doit aller à gauche : - 5 - 2 = -7 Math.Sign renvoie le signe donc - 1 

        _rb2D.linearVelocity = new Vector2(directionX * speed, _rb2D.linearVelocity.y); // On déplace l'ennemis

        // Quand l'ennemi atteint (à 0.3f près) le points target de patrouille, on passe au point suivant
        if (Mathf.Abs(transform.position.x - targetPoint.position.x) < 0.3f)
        {
            // On arrete l'ennemi au point pendant quelques secondes, avant qu'il reprenne sa route
            StartCoroutine(WaitForPatroll(WaitTimePatroll));
            // On passe au point target suivant
            destination = (destination + 1) % points.Count; // récupère le reste pour ne jamais finir une patrouille
                                                            // ex : 0 + 1 % 3 = 1 % 3 = 1 (point suivant % points total)
                                                            // ex dernier point : 2 + 1 % 3 = 3 % 3 = 0

            targetPoint = points[destination];
        }
    }
}
