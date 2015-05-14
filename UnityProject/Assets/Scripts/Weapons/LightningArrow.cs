using System.Collections;
using UnityEngine;

public class LightningArrow : Arrow
{
    public GameObject lightningEffectObj;

    public void CreateLightningHitAnimation()
    {
        GameObject temp = (GameObject)Instantiate(lightningEffectObj);
        temp.transform.localScale = new Vector3(2.0f, 2.0f);
        temp.transform.position = transform.FindChild("PreserveScale").FindChild("Tip").position;
    }
}