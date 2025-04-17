using UnityEngine;

public class VineBehavior : MonoBehaviour
{

    [SerializeField] private float StartRotation;
    [SerializeField] private float speed;
    private Transform _vineTransform;
    private float currentRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.parent.TryGetComponent(out _vineTransform);

        if (StartRotation < -45)
            StartRotation = -45;
        else if (StartRotation > 45)
            StartRotation = 45;

        _vineTransform.rotation = Quaternion.Euler(0, 0, StartRotation);
    }

    // Update is called once per frame
    void Update()
    {
        float phaseOffset = Mathf.Asin(StartRotation / 45f); // Décalage de phase pour démarrer à StartRotation
        float angle = Mathf.Sin(Time.time * speed + phaseOffset) * 45f;

        _vineTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //PlayerController.Instance.isOnVine = true;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform.parent, true);
            collision.GetComponent<Rigidbody2D>().gravityScale = 0;
            collision.transform.rotation = Quaternion.identity;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            collision.GetComponent<Rigidbody2D>().gravityScale = 1;
            collision.transform.rotation = Quaternion.Euler(0, 0, 0);

            //PlayerController.Instance.isOnVine = false;
        }
    }

}
