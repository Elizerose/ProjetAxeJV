using UnityEngine;
/// <summary>
/// Base de nos controllers 
/// </summary>
/// 
public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rb;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    protected virtual void Awake()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _spriteRenderer);
        TryGetComponent(out _animator);
    }

    protected void Start()
    {
        Init();
    }

    protected virtual void Init()
    {

    }
    
}
