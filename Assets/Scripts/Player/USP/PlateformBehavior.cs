using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static ColorPowerController;

public class PlateformBehavior : MonoBehaviour
{
    private PlateformesData plateformesData;
    private float _autoDestroy;
    [HideInInspector] public Text timer;
    public void Init(PlateformesData data)
    {
        // récupérer les infos du pouvoir
        plateformesData = data;
    }

    public void TimeToDestroy()
    {

        for (float i = plateformesData.AutoDestroyTimer; i > 0 ; i -= Time.deltaTime)
        {
            Debug.Log(i);
            if (timer != null)
                timer.text = i.ToString();
        }
        HUDManager.Instance.ColorsListActive.Remove(DatabaseManager.Instance.GetPlateformesData(plateformesData.color));
        HUDManager.Instance.ShowCurrentPower();
        Destroy(gameObject);
    }


    //public IEnumerator TimeToDestroy()
    //{
    //    yield return new WaitForSeconds(plateformesData.AutoDestroyTimer);


        

    //    //ResetPower();
    //}
}
