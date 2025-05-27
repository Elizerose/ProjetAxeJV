using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;
using static ColorPowerController;

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

    private float rotation = 0;

    private GrowDir _dir;

    private bool _isGrowing = false;
    private bool enableGround = false;


    private Transform _preview;
    private bool _powerActive = false;

    private void Start()
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = GameManager.Instance.Camera.transform;
        source.weight = 1f; // 1 = influence maximale

        RotationConstraint[] constraints = GetComponentsInChildren<RotationConstraint>();

        foreach (RotationConstraint c in constraints)
        {
            c.AddSource(source);
            c.constraintActive = true;
        }

    }

    public override void Init(PlateformesData data)
    {
        base.Init(data);
        _canPlace = false;
        _preview = transform.Find("Preview");
        _dir = GrowDir.None;

        
    }

    public override void Update()
    {
        base.Update();
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
        base.ActivePower();
        _powerActive = true;
    }



    // Pouvoir vert
    private void SetDirection()
    {
        

        if (_dir != GrowDir.None)
        {
            if (!_isGrowing && _powerActive)
            {
                _preview.gameObject.SetActive(false);
                StartCoroutine(Grow());
                return;
            }
            else if (_isGrowing && _powerActive)
                return;
                
        }

        // On vérifie d'abord si c'est contre un mur : dans ce cas, on fait pousser a l'horizontal. Sinon a la vertical.

        // GAUCHE
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, 0.8f);
        Debug.DrawRay(transform.position, Vector2.left * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitLeft)
        {
            if (ray.collider != null && ray.collider.gameObject.name != "GreenPlatform" && ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _dir = GrowDir.Right;
                enableGround = true;
                rotation = -90;
                if (_powerActive)
                    return;
            }
        }

        // DROITE
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, 0.8f);
        Debug.DrawRay(transform.position, Vector2.right * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitRight)
        {

            if (ray.collider != null && ray.collider.gameObject.name != "GreenPlatform" && ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _dir = GrowDir.Left;
                enableGround = true;
                rotation = 90;
                if (_powerActive)
                    return;
            }
        }


        // HAUT
        RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, 0.8f);
        Debug.DrawRay(transform.position, Vector2.up * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitUp)
        {
            if (ray.collider != null && ray.collider.gameObject.name != "GreenPlatform" && ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _dir = GrowDir.Down;
                rotation = 180;
                if (_powerActive)
                    return;
            }
        }


        // BAS
        RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, Vector2.down, 0.8f);
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitDown)
        {
            if (ray.collider != null && ray.collider.gameObject.name != "GreenPlatform" && ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _dir = GrowDir.Up;
                rotation = 0;
                if (_powerActive)
                    return;
            }
        }

        if (_dir != GrowDir.None && !_powerActive)
        {
            if (_preview != null)
            {
                transform.localRotation = Quaternion.Euler(0, 0, rotation);
                //_preview.localRotation = Quaternion.Euler(0, 0, rotation);
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
            yield return new WaitForSeconds(1);
            Vector3 Position = BlockPosition(previousBlock);

            // on instancie un block dans la direction.
            GameObject block = Instantiate(_ivyPrefab, Position, transform.rotation, transform);
            if (enableGround)
            {
                block.GetComponent<BoxCollider2D>().isTrigger = false;
                block.layer = LayerMask.NameToLayer("Ground");
            }
            previousBlock = block.transform;
            //block.transform.localRotation = Quaternion.Euler(0, 0, rotation);

            
        }
    }

    private Vector3 BlockPosition(Transform previousBlock)
    {
        Vector3 blockPos = new Vector3(previousBlock.position.x, previousBlock.position.y, previousBlock.position.z);
        switch (_dir)
        {
            case GrowDir.Up:
                blockPos.y += transform.localScale.y;
                break;
            case GrowDir.Down:
                blockPos.y -= transform.localScale.y;
                break;
            case GrowDir.Left:
                blockPos.x -= transform.localScale.x;
                break;
            case GrowDir.Right:
                blockPos.x += transform.localScale.x;
                break;
            default:
                break;
        }
        return blockPos;
    }
}
