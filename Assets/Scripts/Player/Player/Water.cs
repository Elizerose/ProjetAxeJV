using System.Collections;
using TMPro;
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

    private float _time = 0f;
    private float _duration = 1f;

    private float OxygeneTimer;
    private TextMeshProUGUI _WaterTimerText;
    private float _startFontSize;
    public bool CanBreatheUnderWater = false;



    public float ImpulseCDTime = 0.35f;
    public ParticleSystem BubbleEffect;
    public ParticleSystem WaterEnter;
    private Vector3 PlayerScale;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        PlayerScale = GetComponent<Transform>().localScale; 
        NormalGrav = rb.gravityScale;

        _WaterTimerText = HUDManager.Instance.OxygeneTimerGO.GetComponentInChildren<TextMeshProUGUI>();
        _startFontSize = _WaterTimerText.fontSize;

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
                HUDManager.Instance.OxygeneTimerGO.GetComponent<Image>().enabled = false;

                HUDManager.Instance.OxygeneTimerGO.SetActive(true);
                HUDManager.Instance.OxygeneTimerGO.GetComponentInChildren<TextMeshProUGUI>().text = ((int)OxygeneTimer).ToString();

                OxygeneTimer -= Time.deltaTime;

                if (OxygeneTimer <= 10f)
                {
                    float ratio = _time / _duration;
                    float t = Mathf.Sin(ratio * Mathf.PI);
                    HUDManager.Instance.OxygeneTimerGO.GetComponentInChildren<TextMeshProUGUI>().fontSize = Mathf.Lerp(_startFontSize + 10, _startFontSize + 20, t) ;
                    HUDManager.Instance.OxygeneTimerGO.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;

                    _time += Time.unscaledDeltaTime;
                    if (_time >= _duration)
                        _time = 0 ;
                }
                if (OxygeneTimer <= 0)
                {
                    GameManager.Instance.Death(DeathCauses.Water);
                    HUDManager.Instance.OxygeneTimerGO.SetActive(false);
                    _WaterTimerText.fontSize = _startFontSize;
                    _WaterTimerText.color = Color.black;
                    OxygeneTimer = 25f;
                }
                
            }
            // Il peut respirer sous leau
            else
            {
                OxygeneTimer = 25f;
                //HUDManager.Instance.OxygeneTimerGO.SetActive(false);
                HUDManager.Instance.OxygeneTimerGO.GetComponent<Image>().enabled = true;

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
            CanImpulse = false;
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");


            if (horizontal != 0 || vertical != 0)
            {
                if (horizontal > 0)
                    transform.localScale = new Vector3(PlayerScale.x, PlayerScale.y, PlayerScale.z);
                else if (horizontal < 0)
                    transform.localScale = new Vector3(-PlayerScale.x, PlayerScale.y, PlayerScale.z);
                rb.linearVelocity = new Vector2(horizontal * ImpulseSpeed, vertical * ImpulseSpeed);
            }
            
            yield return new WaitForSeconds(ImpulseCDTime);
            CanImpulse = true;
            
        }

        GetComponent<Movements>().Flip();

    }

    public void EnterWater()
    {
        WaterEnter.Play();
        if (CanSwim)
        {
            InWater = true;
            BubbleEffect.gameObject.SetActive(true);
            rb.gravityScale = WaterGrav;
            rb.linearVelocity = Vector2.zero;

        }
    }

    public void ExitWater()
    {
        if (CanSwim)
        {
            BubbleEffect.gameObject.SetActive(false);
            InWater = false;
            rb.gravityScale = NormalGrav;
        }
    }
}
