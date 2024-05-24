using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    public GameObject LoadEffect(string key)
;
    public void SpawnEffect(GameObject EffectPrefab, Vector3 StartRotation, Transform Parent = null)
;
}
public interface ISound
{
    public GameObject LoadSound(string key, Transform Parent = null)
;
    public void PlaySound(GameObject SoundPrefab)
;
}



public class UnitController : MonoBehaviour, IEffect, ISound
{
    public string MyName;
    public GameObject LoadSound(string key, Transform Parent = null)
    {
        GameObject prefab = null;
        if (Managers.DataLoder.DataCache_Sound.TryGetValue(key, out prefab))
            return GameObject.Instantiate(prefab, Parent);
        return null;
    }
    public void PlaySound(GameObject SoundPrefab)
    {
        AudioSource audio = SoundPrefab.GetComponent<AudioSource>();
        if (audio && !audio.isPlaying)
        {
            audio.Play();
        }
    }
    public GameObject LoadEffect(string key)
    {
        GameObject prefab = null;
        if (Managers.DataLoder.DataCache_Effect.TryGetValue(key, out prefab))
            return prefab;
        return null;
    }
    public void SpawnEffect(GameObject EffectPrefab, Vector3 StartRotation, Transform Parent = null)
    {
        GameObject ob = GameObject.Instantiate(EffectPrefab, Parent);
        if (ob)
        {
            ParticleSystem particle = ob.GetComponent<ParticleSystem>();
            var main = particle.main;
            main.startRotation3D = true;
            main.startRotationX = StartRotation.x;
            main.startRotationY = StartRotation.y;
            main.startRotationZ = StartRotation.z;
        }
    }
}
