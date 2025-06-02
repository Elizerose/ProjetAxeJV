using System.Collections;
using UnityEngine;

public class ProjectilePeintureBehavior : MonoBehaviour
{
    private Vector3 StartingPos;
    private float MaxDistance = 8;

    [SerializeField] private ParticleSystem _explodeParticule;

    private void Start()
    {
        StartingPos = transform.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(StartingPos, transform.position);
        if (distance > MaxDistance)
            Destroy(gameObject);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _explodeParticule.Play();
            StartCoroutine(WaitForParticleToEnd());
        }
    }

    private IEnumerator WaitForParticleToEnd()
    {
        // Attend que le système de particules soit terminé
        while (_explodeParticule.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

}
