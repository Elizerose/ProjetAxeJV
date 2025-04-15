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

        // Mise à jour de la position du fond
        transform.position = new Vector3(startingPosition + distanceX, transform.position.y, transform.position.z);

        // Si le fond dépasse le côté droit ou gauche, le remettre au début
        if (Camera.transform.position.x > startingPosition + length)
        {
            startingPosition += length; // Réinitialise la position du fond à droite
        }
        else if (Camera.transform.position.x < startingPosition - length)
        {
            startingPosition -= length; // Réinitialise la position du fond à gauche
        }
    }
}
