using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float HP = 3f; 
    public float MaxHP = 3f;

    void Update()
    {
        if (HP <= 0)
        {
        Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Peinture"))
        {
            Destroy(collision.gameObject);
            HP-=1;
        }
    }
}
