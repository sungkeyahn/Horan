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

    //public Action KeyAction = null;
    public Action <KeyBoardEvent> KeyAction = null;
    bool _keyPressed;
    public Action<MouseEvent> MouseAction = null;
    bool _pressed = false;
    float _pressedTime = 0;

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
    }
    void OnDestroy()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
