using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private Vector2 _sizeBoxCast = new Vector2(5, 3);

    private Vector3 _velocity;

    void Awake()
    {
        if (_target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _target = player.transform;
            }
            else
            {
                Debug.LogWarning("CameraController: No target found.");
            }
        }
    }

    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 cameraPos = transform.position;
        Vector2 halfBox = _sizeBoxCast / 2f;

        float left = cameraPos.x - halfBox.x;
        float right = cameraPos.x + halfBox.x;
        float bottom = cameraPos.y - halfBox.y;
        float top = cameraPos.y + halfBox.y;

        Vector3 newTargetPos = cameraPos;

        if (_target.position.x < left || _target.position.x > right)
        {
            newTargetPos.x = _target.position.x + _offset.x;
        }

        if (_target.position.y < bottom || _target.position.y > top)
        {
            newTargetPos.y = _target.position.y + _offset.y;
        }

        Vector3 smoothPos = Vector3.SmoothDamp(cameraPos, newTargetPos, ref _velocity, _smoothTime);
        transform.position = new Vector3(smoothPos.x, smoothPos.y, cameraPos.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _sizeBoxCast);
    }
}

