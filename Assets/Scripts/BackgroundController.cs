using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    private float startingPosition;
    private float length;
    public GameObject Camera;
    public float ParallaxSpeed;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float distanceX = (Camera.transform.position.x - startingPosition) * ParallaxSpeed;

        float movement = Camera.transform.position.x * (1 - ParallaxSpeed);

        // Met à jour la position du background en fonction de la position initiale et des distances
        transform.position = new Vector3(startingPosition + distanceX, transform.position.y, transform.position.z);

        if (movement > startingPosition + length) // si ca depasse du coté droit
        {
            startingPosition += length;
        }
        else if(movement < startingPosition - length) // si ca depasse du coté gauche
        {
            startingPosition -= length;
                
        }

    }
}
