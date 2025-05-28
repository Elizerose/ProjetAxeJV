using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float HP = 3f; 
    public float MaxHP = 3f;

    void Update()
    {
        if (HP <= 0)
        {
            Destroy(transform.parent.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Peinture"))
        {
            Destroy(collision.gameObject);
            HP-=1;
            StartCoroutine(EnemyDmg());
        }
        
    }

    IEnumerator EnemyDmg()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
