using UnityEngine;

public class Player_Shoot : MonoBehaviour
{
    public GameObject PeinturePrefab;
    public float Peinture_Vitesse;
    public GameObject ShootPoint;
    private bool Shoot_debounce = false;
    public float Shoot_cooldown = 0.5f;
    private float Last_used_time;

    void Update()
    {
        if (Time.time > Last_used_time + Shoot_cooldown)
        {
            Shoot_debounce = false;
        }

        if (Input.GetMouseButtonDown(0) && !Shoot_debounce && ColorPowerController.Instance._state != ColorPowerController.STATE_POWER.INPLACEMENT)
        {
            Shoot_debounce = true;
            Last_used_time = Time.time;
            Lance_Peinture();
        }
    }

    void Lance_Peinture()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDirection = (mousePos - (Vector2)ShootPoint.transform.position).normalized;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        GameObject Nv_Peinture = Instantiate(PeinturePrefab, ShootPoint.transform.position, Quaternion.Euler(0,0,angle - 90));
        Nv_Peinture.GetComponent<Rigidbody2D>().AddForce(shootDirection * Peinture_Vitesse, ForceMode2D.Impulse);
    }
}
