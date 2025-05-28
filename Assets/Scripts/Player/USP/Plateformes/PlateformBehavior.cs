using TMPro;
using UnityEngine;

public class PlateformBehavior : MonoBehaviour
{
    public bool StartDelai;
    private float _autoDestroyTimer;
    [HideInInspector] public TextMeshProUGUI timer;

    private ParticleSystem _feedbackPlacement;

    protected bool _canPlace = false;

    private float _duration = 1f;
    private float _time = 0f;

    // Init les datas correspondante a la plateforme
    public virtual void Init(PlateformesData data)
    {
        _canPlace = false;
        _autoDestroyTimer = data.AutoDestroyTimer;
        _feedbackPlacement = GetComponentInChildren<ParticleSystem>();
        timer = GetComponentInChildren<TextMeshProUGUI>();
    }

    public virtual void Update()
    {
        if (StartDelai)
        {
            TimeToDestroy();
            TextAnim();           
        }
        UpdatePlacementStatus();
    }

    // Gestion du timer et de l'auto destruction
    public void TimeToDestroy()
    {
        _autoDestroyTimer -= Time.deltaTime;

        if (_autoDestroyTimer < 0 )
        {
            //HUDManager.Instance.ShowCurrentPower();
            Destroy(gameObject);
        }
        else
        {
            if (timer != null)
            {
                timer.text = ((int)_autoDestroyTimer).ToString();
            }     
        }   
    }

    // Animation du timer pendant sa décrémentation
    private void TextAnim()
    {
        float ratio = _time / _duration;

        float t = Mathf.Sin(ratio * Mathf.PI);

        timer.fontSize = Mathf.Lerp(55,70,t);

        _time += Time.unscaledDeltaTime;

        if (_time > _duration)
        {
            _time = 0f;
        }
    }


    // Check si la plateforme est plaçable
    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            _canPlace = false;
        else 
            _canPlace = true;
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            _canPlace = true;
    }



    public void UpdatePlacementStatus()
    {     
        GameManager.Instance.Player.GetComponent<PlateformPlacement>().CanPlace = _canPlace;
    }

    public virtual void ActivePower()
    {
        _feedbackPlacement.Play();
    }
}
