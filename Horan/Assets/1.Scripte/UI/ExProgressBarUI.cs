using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExProgressBarUI : BaseUI
{
    enum Components { Slider_EXBar, }

    public float ratio = 1;

    public override void Init()
    {
        Bind<GameObject>(typeof(Components));
        //SetHpRatio(ratio);
    }
    public void SetHpRatio(float ratio)
    {
        GetObject((int)Components.Slider_EXBar).GetComponent<Slider>().value = ratio;
    }
}
