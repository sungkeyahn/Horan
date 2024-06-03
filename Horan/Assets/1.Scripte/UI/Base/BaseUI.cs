using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UIEvent
{
    Click,
    Drag,
};

public abstract class BaseUI : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> ob = new Dictionary<Type, UnityEngine.Object[]>();

    void Start()
    {
        Init();
    }

    public abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object  //���÷��� ����� Ȱ���� ���ε� �Լ� 
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        ob.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = FindChild(gameObject, names[i], true);
            else
                objects[i] = FindChild<T>(gameObject, names[i], true);
        }
    }
    public void BindEvent(GameObject go, Action<PointerEventData> action, UIEvent type = UIEvent.Click)
    {
        UIEventHandler evt = go.GetComponent<UIEventHandler>();
        switch (type)
        {
            case UIEvent.Click:
                evt.OnPointerClickHandler -= action;
                evt.OnPointerClickHandler += action;
                break;
            case UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            default:
                break;
        }
    }
    public T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform tr = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || tr.name == name)
                {
                    T component = tr.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }
    public GameObject FindChild(GameObject go, string name = null, bool recursive = false)    //GameObject�� ��ȯ���ִ� ����
    {
        Transform tr = FindChild<Transform>(go, name, recursive);
        if (tr == null) return null;
        return tr.gameObject;
    }
    
    //���� ����� TŸ���� ���ʸ� �Լ��� ����ϴ� �Լ��� ���� ����
    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] obs = null;
        if (ob.TryGetValue(typeof(T), out obs) == false) return null;

        return obs[index] as T;
    }
    protected GameObject GetObject(int index) { return Get<GameObject>(index); }
    protected Text GetText(int index) { return Get<Text>(index); }
    protected Button GetButton(int index) { return Get<Button>(index); }
    protected Image GetImage(int index) { return Get<Image>(index); }
}
