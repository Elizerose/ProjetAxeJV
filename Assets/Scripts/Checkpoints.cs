using UnityEngine;

public class ChackpointCheck : MonoBehaviour
{
    public GameObject player;
    private float Detectradius = 2f;
    public GameObject CPText;
    public ParticleSystem CPEffect;

    void Update()
    {
        Transform nearestCP = ReturnNearestCP();

        if (nearestCP != null)
        {
            CPText.transform.position = nearestCP.position + new Vector3(0, 1f, 0);
            CPText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                player.GetComponent<PlayerHealth>().Lastcheckpoint = nearestCP;

                ParticleSystem newCPEffect = Instantiate(CPEffect, nearestCP.position, Quaternion.identity);

                newCPEffect.Emit(15);
                Destroy(newCPEffect, 1);
            }
        }
        else
        {
            CPText.SetActive(false);
        }
    }

    Transform ReturnNearestCP()
    {
        Transform nearestCP = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform CP in gameObject.transform)
        {
            float dist = Vector2.Distance(CP.position, player.transform.position);
            if (dist < Detectradius && dist < closestDistance)
            {
                closestDistance = dist;
                nearestCP = CP;
            }
        }
        return nearestCP;
    }
}
