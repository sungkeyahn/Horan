using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }
    
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null) root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    int order = 0;
    //Stack<PopupUI> popupStack = new Stack<PopupUI>(); 버전이 올라서 그런가? UI 부분에 버그가 많다
    Stack<GameObject> popupStack = new Stack<GameObject>();
    public T ShowPopupUI<T>(string prefabName = null) where T : PopupUI
    {
        if (string.IsNullOrEmpty(prefabName)) prefabName = typeof(T).Name;
       
        GameObject prefab= Resources.Load<GameObject>($"UI/{prefabName}");
        GameObject go = Object.Instantiate(prefab,Root.transform);
       
        T popup = go.GetComponent<T>();
        popupStack.Push(go);

        go.transform.SetParent(Root.transform);

        return popup;
    }
    public void ClosePopupUI()
    {
        if (popupStack.Count == 0) return;
        GameObject popup;
        if (popupStack.TryPop(out popup))
        {
            Object.Destroy(popup.gameObject);
            order--;
        }
        popup = null;
  
    }
    public void ClosePopupUI(PopupUI popup)
    {
        if (popupStack.Count == 0) return;

        GameObject peek;
        popupStack.TryPeek(out peek);
        if (popup.gameObject != peek)
            return;

        ClosePopupUI();
    }
    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
            ClosePopupUI();
    }

    SceneUI sceneUI = null;
    public T ShowSceneUI<T>(string name = null) where T : SceneUI
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject prefab = Resources.Load<GameObject>($"UI/{name}");
        GameObject go = Object.Instantiate(prefab, Root.transform);
        T scene = go.GetComponent<T>();
        sceneUI = scene;

        go.transform.SetParent(Root.transform);

        return scene;
    }
    public SceneUI GetSceneUI() 
    {
        if (sceneUI == null) 
            return null;
        else
            return sceneUI;
    }


    public T MakeSubUI<T>(Transform parent, string name = null) where T : BaseUI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject prefab = Resources.Load<GameObject>($"UI/{name}");
        GameObject go = Object.Instantiate(prefab, Root.transform);

        if (parent != null)
            go.transform.SetParent(parent);

        return go.GetComponent<T>();
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : BaseUI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject prefab = Resources.Load<GameObject>($"UI/{name}");
        GameObject go = Object.Instantiate(prefab, Root.transform);

        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return go.GetComponent<T>();
    }

    public void Clear()
    {
        //DestoryAllUIs
        //CloseAllPopupUI();

    }
}
