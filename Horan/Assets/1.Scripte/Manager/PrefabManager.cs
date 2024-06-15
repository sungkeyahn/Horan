using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager 
{
    public GameObject PrefabInstance(string key, Transform Parent = null)
    {
        GameObject prefab = null;
        if (Managers.DataLoder.DataCache_Effect.TryGetValue(key, out prefab))
            return GameObject.Instantiate(prefab, Parent);
        if (Managers.DataLoder.DataCache_Sound.TryGetValue(key, out prefab))
            return GameObject.Instantiate(prefab, Parent);
        return null;
    }

    public void PlaySound(GameObject Sound, float volume = 1.0f)
    {
        if (Sound)
        {
            AudioSource audio = Sound.GetComponent<AudioSource>();
            if (audio && !audio.isPlaying)
            {
                audio.volume = volume;
                audio.Play();
            }
        }
    }

    public GameObject SpawnEffect(string key, Vector3 pos)
    {
        GameObject prefab = null;
        if (Managers.DataLoder.DataCache_Effect.TryGetValue(key, out prefab))
        {
            GameObject gameObject = GameObject.Instantiate(prefab,pos, Quaternion.identity);
            return gameObject;
        }
        return null;
    }
    public GameObject SpawnEffect(string key, Vector3 pos, Quaternion quaternion)
    {
        GameObject prefab = null;
        if (Managers.DataLoder.DataCache_Effect.TryGetValue(key, out prefab))
        {
            GameObject gameObject = GameObject.Instantiate(prefab,pos,quaternion);
            return gameObject;
        }
        return null;

    }
    public GameObject SpawnEffect(string key, Transform parent, Vector3 locpos)
    {
        GameObject prefab = null;
        if (Managers.DataLoder.DataCache_Effect.TryGetValue(key, out prefab))
        {
            GameObject gameObject = GameObject.Instantiate(prefab, parent);
            gameObject.transform.localPosition = locpos;
            return gameObject;
        }
        return null;
    }
}
