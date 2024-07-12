using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTipUI : SceneUI
{
    enum Components
    { Image }
    public override void Init()
    {
        Bind<GameObject>(typeof(Components));
        int num = UnityEngine.Random.Range(1, 6);
        Debug.Log(num);
        Sprite sp = null;
        Managers.DataLoder.DataCache_Sprite.TryGetValue($"Loading_Tip{num}", out sp);
        GetObject((int)Components.Image).GetComponent<Image>().sprite=sp;
    }
}
