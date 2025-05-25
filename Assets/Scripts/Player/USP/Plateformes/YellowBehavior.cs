using UnityEngine;
using UnityEngine.Rendering.Universal;

public class YellowBehavior : PlateformBehavior
{
    public override void Init(PlateformesData data)
    {
        base.Init(data);
        _canPlace = true;
        GetComponent<Light2D>().enabled = false;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    public override void ActivePower()
    {
        GetComponent<Light2D>().enabled = true;
    }
}
