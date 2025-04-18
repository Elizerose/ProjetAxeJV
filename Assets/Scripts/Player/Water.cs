using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static ColorPowerController;
using static GameManager;

public class Water : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool CanSwim = true;
    public bool InWater = false;
    private float ImpulseSpeed = 3f;
    private float WaterGrav = 0.1f;
    private float NormalGrav;
    private bool CanImpulse = true;
    private float ImpulseCDTime = 0.35f;


    private float OxygeneTimer;
    public bool CanBreatheUnderWater = false;




    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        NormalGrav = rb.gravityScale; 
    }

    void Update()
    {
        if (InWater && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Swim());
        }
        if (InWater)
        {
            if (!CanBreatheUnderWater)
            {
                HUDManager.Instance.OxygeneTimerGO.SetActive(true);
                HUDManager.Instance.OxygeneTimerGO.GetComponent<Text>().text = ((int)OxygeneTimer).ToString();
                OxygeneTimer -= Time.deltaTime;
                if (OxygeneTimer <= 0)
                {
                    GameManager.Instance.Death(DeathCauses.Water);
                    OxygeneTimer = 25f;
                }
                
            }
            else
            {
                HUDManager.Instance.OxygeneTimerGO.SetActive(false);
            }
            
                
        } 
        else
        {
            HUDManager.Instance.OxygeneTimerGO.SetActive(false);
            OxygeneTimer = 25f;
        }
    }

    private IEnumerator Swim()
    {
        if (CanImpulse)
        {
            Debug.Log("CanImpulse");
            CanImpulse = false;
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");


            if (horizontal != 0 || vertical != 0)
            {
                Debug.Log("swim");
                rb.linearVelocity = new Vector2(horizontal * ImpulseSpeed, vertical * ImpulseSpeed);
            }
            

            yield return new WaitForSeconds(ImpulseCDTime);
            CanImpulse = true;

            
        }

        GetComponent<Movements>().Flip();

    }

    public void EnterWater()
    {
        if (CanSwim)
        {
            InWater = true;
            rb.gravityScale = WaterGrav;
            rb.linearVelocity = Vector2.zero;

        }
    }

    public void ExitWater()
    {
        if (CanSwim)
        {
            InWater = false;
            rb.gravityScale = NormalGrav;
        }
    }
}
