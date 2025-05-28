using UnityEngine;
using UnityEngine.UI;

public class BlueBehavior : PlateformBehavior
{
    public override void Init(PlateformesData data)
    {
        base.Init(data);
        
    }


    public override void Update()
    {
        base.Update();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la plateforme va dans l'eau : plaçable
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
            _canPlace = true;
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        // Si le joueur va dans la bulle, peut respirer, enleve le timer oxygene
        if (collision.CompareTag("Player"))
        {
            ColorPowerController.Instance.CanInvokePaletteUnderWater = true;
            GameManager.Instance.Player.GetComponent<Water>().CanBreatheUnderWater = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        // Si la plateforme sort de l'eau : inplaçable
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
            _canPlace = false;

        // Si le joueur sort de la bulle, reset le timer d'oxygene
        if (collision.CompareTag("Player"))
        {
            ColorPowerController.Instance.CanInvokePaletteUnderWater = false;
            GameManager.Instance.Player.GetComponent<Water>().CanBreatheUnderWater = false;
        }
    }

    public override void ActivePower()
    {
        base.ActivePower();
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255 / 2);
    }


}
