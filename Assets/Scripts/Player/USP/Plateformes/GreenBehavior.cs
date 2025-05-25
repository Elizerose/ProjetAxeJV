using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GreenBehavior : PlateformBehavior
{
    [SerializeField] private GameObject _ivyPrefab;

    private enum GrowDir
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    private GrowDir _dir;

    private bool _isGrowing = false;
    private bool enableGround = false;

    private bool _powerActive = false;


    public override void Init(PlateformesData data)
    {
        base.Init(data);
        _canPlace = false;
    }

    public override void Update()
    {
        base.Update();
        if (_powerActive)
            SetDirection();
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            _canPlace = false;
        else
            _canPlace = true;
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            _canPlace = false;
    }

    public override void ActivePower()
    {
        _powerActive = true;
    }


    // Pouvoir vert
    private void SetDirection()
    {

        if (_dir != GrowDir.None)
        {
            if (!_isGrowing)
                StartCoroutine(Grow());
            return;
        }

        // On vérifie d'abord si c'est contre un mur : dans ce cas, on fait pousser a l'horizontal. Sinon a la vertical.

        // GAUCHE
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, 0.8f);
        Debug.DrawRay(transform.position, Vector2.left * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitLeft)
        {
            if (ray.collider != null && ray.collider != GetComponent<Collider2D>() && ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _dir = GrowDir.Right;
                enableGround = true;
                return;
            }
        }

        // DROITE
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, 0.8f);
        Debug.DrawRay(transform.position, Vector2.right * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitRight)
        {

            if (ray.collider != null && ray.collider != GetComponent<Collider2D>() && ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _dir = GrowDir.Left;
                enableGround = true;
                return;
            }
        }


        // HAUT
        RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, 0.8f);
        Debug.DrawRay(transform.position, Vector2.up * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitUp)
        {
            if (ray.collider != null && ray.collider != GetComponent<Collider2D>() && ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _dir = GrowDir.Down;
                return;
            }
        }


        // BAS
        RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, Vector2.down, 0.8f);
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitDown)
        {
            if (ray.collider != null && ray.collider != GetComponent<Collider2D>() && ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _dir = GrowDir.Up;
                return;
            }
        }
    }

    private IEnumerator Grow()
    {
        _isGrowing = true;
        Transform previousBlock = transform;

        if (enableGround)
        {
            previousBlock.GetComponent<BoxCollider2D>().isTrigger = false;
            previousBlock.gameObject.layer = LayerMask.NameToLayer("Ground");
        }

        for (int i = 0; i < 4; i++)
        {
            Vector3 Position = BlockPosition(previousBlock);

            // on instancie un block dans la direction.
            GameObject block = Instantiate(_ivyPrefab, Position, Quaternion.identity, transform);
            if (enableGround)
            {
                block.GetComponent<BoxCollider2D>().isTrigger = false;
                block.layer = LayerMask.NameToLayer("Ground");
            }
            previousBlock = block.transform;

            yield return new WaitForSeconds(1);
        }
    }

    private Vector3 BlockPosition(Transform previousBlock)
    {
        Vector3 blockPos = new Vector3(previousBlock.position.x, previousBlock.position.y, previousBlock.position.z);

        switch (_dir)
        {
            case GrowDir.Up:
                blockPos.y += previousBlock.localScale.y;
                break;
            case GrowDir.Down:
                blockPos.y -= previousBlock.localScale.y;
                break;
            case GrowDir.Left:
                blockPos.x -= previousBlock.localScale.x;
                break;
            case GrowDir.Right:
                blockPos.x += previousBlock.localScale.x;
                break;
            default:
                break;
        }

        return blockPos;
    }
}
