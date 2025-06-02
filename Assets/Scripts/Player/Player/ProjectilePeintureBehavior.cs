using System.Collections;
using UnityEngine;

public class ProjectilePeintureBehavior : MonoBehaviour
{
    private Vector3 StartingPos;
    private float MaxDistance = 8;

    [SerializeField] private AudioClip _launchSound;
    [SerializeField] private AudioClip _destroySound;

    [SerializeField] private ParticleSystem _explodeParticule;

    private void Start()
    {
        StartingPos = transform.position;
        AudioManager.Instance.PlaySFX(_launchSound);
    }

    private void Update()
    {
        float distance = Vector3.Distance(StartingPos, transform.position);
        if (distance > MaxDistance)
            StartCoroutine(WaitForParticleToEnd());
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            AudioManager.Instance.PlaySFX(_destroySound);
            StartCoroutine(WaitForParticleToEnd());
        }
    }

    private IEnumerator WaitForParticleToEnd()
    {
        
        _explodeParticule.Play();

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }

}
