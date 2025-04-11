using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothTime = 0.1f;

    private Vector3 _velocity;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _target = GameManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        var limiteLeft = transform.TransformPoint(-SizeBoxCast / 2);
        var limiteRight = transform.TransformPoint(SizeBoxCast / 2);

        var isOutLeft = _target.position.x < limiteLeft.x;
        var isOutRight = _target.position.x > limiteRight.x;

        Vector2 targetOut = isOutLeft ? limiteLeft : isOutRight ? limiteRight : transform.position;
        


        var smoothPos = Vector3.SmoothDamp(transform.position, targetOut, ref _velocity, _smoothTime);
        Vector3 targetPos = new Vector3(smoothPos.x, smoothPos.y, transform.position.z);


        transform.position = targetPos;
        
    }

    [SerializeField] private Vector2 SizeBoxCast;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, SizeBoxCast);
    }
}
