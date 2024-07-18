using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : ChangeMaterial
{
    float time = 1;
    bool isActive;
    float Speed;
    public void DissolveEffectForUpdate(float speed)
    {
        Swap();
        Speed = speed;
        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        { 
            time = Mathf.Clamp(time - Time.deltaTime* Speed, 0, 1);
            Renderer.material.SetFloat("_Progress", time);
            if (time == 0) 
            {
                isActive = false;
                time = 1;
            }
        }
    }

}
