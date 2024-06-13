using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDelete : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DeleteTime());
    }
    float lifetime = 1f;
    public void SetLifeTime(float time)
    {
        if (0<time)
        {
            lifetime = time;
        }
    }
    IEnumerator DeleteTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
        yield break;
    }

}
