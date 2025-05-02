using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static ColorPowerController;

public class PlateformBehavior : MonoBehaviour
{
    public bool StartDelai;
    private float _autoDestroyTimer;
    [HideInInspector] public Text timer;
    public void Init(PlateformesData data)
    {
        // récupérer les infos du pouvoir
        _autoDestroyTimer = data.AutoDestroyTimer;
        timer = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (StartDelai)
        {
            TimeToDestroy();
        }
    }

    public void TimeToDestroy()
    {
        _autoDestroyTimer -= Time.deltaTime;

        if (_autoDestroyTimer < 0 )
        {
            //HUDManager.Instance.ColorsListActive.Remove(DatabaseManager.Instance.GetPlateformesData(color));
            HUDManager.Instance.ShowCurrentPower();
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

}
