using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMapEnterUI : PopupUI
{
    bool isInit = false;
    enum Components { Image , Text }
    RectTransform tr;
    TMPro.TMP_Text text;

    public override void Init()
    {
        if (isInit) return;

        #region Bind
        Bind<GameObject>(typeof(Components));

        tr = GetObject((int)Components.Image).GetComponent<RectTransform>();
        text = GetObject((int)Components.Text).GetComponent<TMPro.TMP_Text>();
        #endregion Bind
        isInit = true;
    }
    Vector2 delta = new Vector2(1f,0);
    private void Start()
    {
        Init();
        StartCoroutine(FadeTextToFullAlpha());
    }

     IEnumerator FadeTextToFullAlpha() // 알파값 0에서 1로 전환
     {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (tr.sizeDelta.x < 600)
        {
            tr.sizeDelta = tr.sizeDelta + delta;
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime /0.5f));
            yield return null;
        }
        gameObject.SetActive(false);
     }

}
