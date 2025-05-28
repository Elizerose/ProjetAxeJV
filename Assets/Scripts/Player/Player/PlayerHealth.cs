using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour
{
    public float HP = 3f;
    public float MaxHP = 3f;
    public bool CanTakeDamage = true;
    public Transform Lastcheckpoint;
    public GameObject Spawners_parent;
    public GameObject EnemiesHolder;

    void Start()
    {
       // SpawnEnemies();
    }

    void Update()
    {
        if (CheckForHP())
        {
            ResetPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {  
    {
        if ((other.gameObject.CompareTag("Enemy") && other.GetComponent<Collider2D>() != null )|| other.gameObject.CompareTag("Projectile"))
        {
          Debug.Log(other.gameObject.name);
          if (CanTakeDamage)
          {
              Destroy(other);
              CanTakeDamage = false;
              HP -= 1;
              gameObject.GetComponent<SpriteRenderer>().color = Color.red;
             StartCoroutine(ResetDamageCooldown());
          }    
      }
      else
      {
        //print(other.name);
      }
    }
}

    private IEnumerator ResetDamageCooldown()
    {
     yield return new WaitForSeconds(1.5f);
     CanTakeDamage = true; 
     gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    bool CheckForHP()
    {
        return HP <= 0;
    }

    private void ResetPlayer()
    {
        HP = MaxHP;
        gameObject.transform.position = Lastcheckpoint.transform.position;
        //SpawnEnemies();
    }

    /*
    void SpawnEnemies()
    {
        print("Spawned Enemies");
        foreach (Transform enemies in EnemiesHolder.transform)
        {
            Destroy(enemies.gameObject);
        }
        for (int i = 0; i < Spawners_parent.transform.childCount; i++)
        {
        Transform Child = Spawners_parent.transform.GetChild(i);
        Child.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GameObject new_Enemy = Instantiate(Child.GetComponent<Spawner>().Enemy_To_Spawn);
        new_Enemy.transform.position = Child.transform.position;
        new_Enemy.transform.parent = EnemiesHolder.transform;
        }
    
    }
    */
}
