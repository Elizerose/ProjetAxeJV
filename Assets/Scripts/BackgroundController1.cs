using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startingPosition;
    private float length;
    public GameObject Camera;
    public float ParallaxSpeed;
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Calcul de la distance parallax
        float distanceX = (Camera.transform.position.x - startingPosition) * ParallaxSpeed;

        // Mise � jour de la position du fond
        transform.position = new Vector3(startingPosition + distanceX, transform.position.y, transform.position.z);

        // Si le fond d�passe le c�t� droit ou gauche, le remettre au d�but
        if (Camera.transform.position.x > startingPosition + length)
        {
            startingPosition += length; // R�initialise la position du fond � droite
        }
        else if (Camera.transform.position.x < startingPosition - length)
        {
            startingPosition -= length; // R�initialise la position du fond � gauche
        }
    }
}
