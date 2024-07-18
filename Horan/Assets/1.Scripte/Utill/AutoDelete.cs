using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDelete : MonoBehaviour
{
    [SerializeField]
    float lifetime = 1f;
    private void Start()
    {
        StartCoroutine(DeleteTime());
    }
    public void SetLifeTime(float time)
    {
        if (0<time)
        {
            lifetime = time;
        }
    }
    IEnumerator DeleteTime()
    {
        yield return new WaitForSecondsRealtime(lifetime);
        Destroy(gameObject);
        yield break;
    }

}
