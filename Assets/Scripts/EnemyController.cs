using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    public int EnemyID;
    private EnemyData data;


    private Rigidbody2D _rb2D;
    private Transform _enemyTransform;
    private SpriteRenderer _spriteRenderer;



    void Awake()
    {
        TryGetComponent(out _rb2D);
        TryGetComponent(out _enemyTransform);
        TryGetComponent(out _spriteRenderer);
    }

    void Start()
    {
        data = DatabaseManager.Instance.GetData(0);
        Init();
    }

    private void Init()
    {
        name = data.name;
        _spriteRenderer.sprite = data.sprite;
    }


    

    void Update()
    {
        
    }
}
