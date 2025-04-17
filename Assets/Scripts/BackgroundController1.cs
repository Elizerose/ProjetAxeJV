using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float _length, _startPosition;

    public float ParallaxEffect;

    [SerializeField] private bool _followInY; 
    [SerializeField] private Camera _camera;

    private void Start()
    {
        _startPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x; // longueur du background
    }

    private void LateUpdate() // LateUpdate pour eviter les tremblements
    {
        // calcul pour le paralax
        float temp = _camera.transform.position.x * (1 - ParallaxEffect);

        // décalage horizontal a appliquer pour que le background suive la camera
        float distance = _camera.transform.position.x * ParallaxEffect;

        // Si on a cocher qu'il devait suivre en Y, on met la meme transform.position.y, on ajoute a la position de départ la distance a ajouter
        if (_followInY)
            transform.position = new Vector3(_startPosition + distance, _camera.transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);
        
        // Si la camera depasse la longueur du background, on reset la starting position a la longueur pour repeter le background
        if (temp > _startPosition + _length) 
        {
            _startPosition += _length;
        }
        else if (temp < _startPosition - _length) // et inversement
        {
            _startPosition -= _length;
        }

    }








}
