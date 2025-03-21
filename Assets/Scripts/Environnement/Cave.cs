using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Cave : MonoBehaviour
{
    public Light2D globalLight;
    private bool isInBox;
    private Coroutine lightCoroutine;
    public GameObject player;

    void Update()
    {
        if (isInBox)
        {
            if (lightCoroutine == null) 
            {
                lightCoroutine = StartCoroutine(ChangeLight());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<Light2D>().enabled = true;
            isInBox = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<Light2D>().enabled = false;
            isInBox = false;
            if (lightCoroutine != null)
            {
                StopCoroutine(lightCoroutine);
                lightCoroutine = null;  
            }

            if (globalLight != null)
                StartCoroutine(ChangeLightBack());
        }
    }

    private IEnumerator ChangeLight()
    {
        if (globalLight == null) yield break;

        while (globalLight.intensity > 0.01f)
        {
            globalLight.intensity -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        lightCoroutine = null;
    }
    private IEnumerator ChangeLightBack()
    {
        if (globalLight == null) yield break;

        while (globalLight.intensity < 1)
        {
            globalLight.intensity += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        lightCoroutine = null;
    }
}
