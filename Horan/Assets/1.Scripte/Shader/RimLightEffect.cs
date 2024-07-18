using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RimLightEffect : ChangeMaterial
{
    public void RimLightEffectSwitch(bool isoff)
    {
        Swap(isoff);
    }
    public void RimLightEffectForSecoend(float time)
    {
        StopCoroutine(Cor_RimLightEffectForSecoend(time));
        StartCoroutine(Cor_RimLightEffectForSecoend(time));
    }
    IEnumerator Cor_RimLightEffectForSecoend(float time)
    {
        Swap();
        yield return new WaitForSecondsRealtime(time);
        Swap(true);
    }

}
