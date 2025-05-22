using UnityEngine;
using static UnityEngine.UI.Image;

public class GreenPlateformBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // HAUT
        RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, 0.8f);
        Debug.DrawRay(transform.position, Vector2.up * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitUp)
        {
            if (ray.collider != null && ray.collider != GetComponent<Collider2D>())
            {
                Debug.Log("Obstacle en haut : " + ray.collider.name);
            }
        }
        

        // BAS
        RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, Vector2.down, 0.8f);
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitDown)
        {
            if (ray.collider != null && ray.collider != GetComponent<Collider2D>())
            {
                Debug.Log("Obstacle en bas : " + ray.collider.name);
            }
        }
            

        // GAUCHE
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, 0.8f);
        Debug.DrawRay(transform.position, Vector2.left * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitLeft)
        {
            if (ray.collider != null && ray.collider != GetComponent<Collider2D>())
            {
                Debug.Log("Obstacle en gauche : " + ray.collider.name);
            }
        }

        // DROITE
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, 0.8f);
        Debug.DrawRay(transform.position, Vector2.right * 0.8f, Color.red);
        foreach (RaycastHit2D ray in hitRight)
        {
            if (ray.collider != null && ray.collider != GetComponent<Collider2D>())
            {
                Debug.Log("Obstacle en droite : " + ray.collider.name);
            }
        }

    }
}
