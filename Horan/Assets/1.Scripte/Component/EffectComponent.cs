using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EffectInfo
{
    public EffectInfo(string EffectName, Vector3 StartlocPos, float StartDelay = 0f)
    {
        effectName = EffectName;
        startPos = StartlocPos;
        startDelay = StartDelay;
    }
    public string effectName;
    public Vector3 startPos;
    public float startDelay;
}
public class EffectComponent : MonoBehaviour
{
    public string EffectName;
    public Vector3 Position;
    public Vector3 Rotation;

    public string AnimName;
    public float SpawnTime;
    public float DeleteTime;

    public Animator Anim;

    public bool isProjectiles;

    bool isSpawned =false;
    private void Update()
    {
        if (Anim != null)
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
            {
                var normalizedTime = Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (isSpawned &&  normalizedTime  <= SpawnTime)
                    isSpawned = false;

                if (normalizedTime != 0 && normalizedTime < 1f && SpawnTime < normalizedTime) 
                {
                    if (!isSpawned)
                        SpawnEffect(EffectName); //StartCoroutine(EffectSpawn());
                    isSpawned = true;
                }
            }
        }
    }
    GameObject SpawnEffect(string key)
    {
        GameObject prefab = null;
        if (Managers.DataLoder.DataCache_Effect.TryGetValue(key, out prefab))
        {
            GameObject gameObject;
            if (isProjectiles)
            {
                gameObject = GameObject.Instantiate(prefab);
                gameObject.transform.localPosition = transform.position+ Position;
                gameObject.GetComponent<ProjectilesEffectMover>().SetTarget(transform.forward);
            }
            else
            {
                gameObject = GameObject.Instantiate(prefab, transform);
                gameObject.transform.localPosition = Position;
                gameObject.transform.localRotation = Quaternion.Euler(Rotation);
            }
            Destroy(gameObject, DeleteTime);
            return gameObject;
        }
        return null;
    }
}
