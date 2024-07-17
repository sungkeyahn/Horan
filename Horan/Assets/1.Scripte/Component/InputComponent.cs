using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    public enum MouseEvent
    {
        None,
        Press,
        PointerDown,
        PointerUp,
        Click,
    }
    public enum KeyBoardEvent
    {
        None,
        Press,
        ButtonDown,
        ButtonUp,
    }

    public Action <KeyBoardEvent> KeyAction = null;
    bool _keyPressed;

    public Action<MouseEvent> MouseAction = null;
    bool _pressed = false;
    float _pressedTime = 0;

    public Action <Touch> TouchAction =null;

    void Update()
    {
        if (KeyAction != null)
        {
            if (Input.anyKeyDown)
            {
                KeyAction.Invoke(KeyBoardEvent.ButtonDown);
                _keyPressed = true;
            }
            else if (Input.anyKey)
            {
                KeyAction.Invoke(KeyBoardEvent.Press);
            }
            else
            {
                if (_keyPressed)
                {
                    KeyAction.Invoke(KeyBoardEvent.ButtonUp);
                    _keyPressed = false;
                }
                KeyAction.Invoke(KeyBoardEvent.None);
            }
        }
        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (!_pressed)
                {
                    MouseAction.Invoke(MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(MouseEvent.Press);
                _pressed = true;
            }
            else if (_pressed)
            {
                if (Time.time < _pressedTime + 0.2f)
                    MouseAction.Invoke(MouseEvent.Click);
                MouseAction.Invoke(MouseEvent.PointerUp);

                _pressed = false;
                _pressedTime = 0;
            }
            else
                MouseAction.Invoke(MouseEvent.None);
        }
        if (TouchAction!=null)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    TouchAction.Invoke(touch);
                }
            }
        }
    }
    void OnDestroy()
    {
        KeyAction = null;
        MouseAction = null;
        TouchAction = null;
    }
}

/*  void OnPlayerMouseEvent(InputComponent.MouseEvent evt)
    {
        switch (evt)
        {
            case InputComponent.MouseEvent.None:
                {
                }
                break;
            case InputComponent.MouseEvent.Press:
                {

                    // Debug.Log("Press");
                }
                break;
            case InputComponent.MouseEvent.PointerDown:
                {
                    // Debug.Log("PointerDown");
                }
                break;
            case InputComponent.MouseEvent.PointerUp:
                {
                    // Debug.Log("PointerUp");
                }
                break;
            case InputComponent.MouseEvent.Click:
                {
                    if (atkAble && atkCount < weapon.AnimInfo_FATK.Count)
                    {
                        Act.Execution((int)ECharacterAct.FAttack);
                    }//Debug.Log("Click");
                }
                break;
            default:
                break;
        }
    }
    void OnPlayerKeyBoardEvent(InputComponent.KeyBoardEvent evt)
    {
        switch (evt)
        {
            case InputComponent.KeyBoardEvent.None:
                {
                    move.SetMove(0);
                    //anim.SetInteger("AnimState", (int)EPlayerAnimState.IDLE);
                    for (int i = 0; i < anims.Length; i++)
                    {
                        anims[i].SetInteger("AnimState", (int)EPlayerAnimState.IDLE);
                    }
                    Act.Finish((int)ECharacterAct.Move);
                }
                break;
            case InputComponent.KeyBoardEvent.Press:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    Act.Execution((int)ECharacterAct.Move);
                }
                if (Input.GetKey(KeyCode.E) && !isGuard)
                {
                    Act.Execution((int)ECharacterAct.Guard);
                }
                break;
            case InputComponent.KeyBoardEvent.ButtonDown:
                if (Input.GetKey(KeyCode.Space) && Stat.UseSP(20))
                    Act.Execution((int)ECharacterAct.Dash);
                break;
            case InputComponent.KeyBoardEvent.ButtonUp:
                //if (isGuard && !Input.GetKeyDown(KeyCode.E))
               // {
                    //isGuard = false;
                   // Act.Finish((int)ECharacterAct.Guard);
                //}
                break;
            default:
                break;
        }
    }*/