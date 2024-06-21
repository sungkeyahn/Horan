using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    GameObject Prefab ;
    public string SoundName;

    public Animator Anim;
    public string AnimName;

    bool IsLoop;
    private void Start()
    {
        Prefab = InstanceSoundObejct(SoundName);
        IsLoop = Prefab.GetComponent<AudioSource>().loop;
        if (IsLoop)
            PlaySound(0.4f);
    }
    private void Update()
    {
        if (Anim != null && !IsLoop)
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
            {
                var normalizedTime = Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (normalizedTime != 0 && normalizedTime < 1f)
                        PlaySound();
            }
        }
    }
    public GameObject InstanceSoundObejct(string key)
    {
        GameObject prefab = null;
        if (Managers.DataLoder.DataCache_Sound.TryGetValue(key, out prefab))
        {
            GameObject gameObject = GameObject.Instantiate(prefab, transform);   
            return gameObject;
        }
        return null;
    }
    public void PlaySound(float volume = 1.0f)
    {
        if (Prefab)
        {
            AudioSource audio = Prefab.GetComponent<AudioSource>();
            if (audio && !audio.isPlaying)
            {
                audio.volume = volume;
                audio.Play();
            }
        }
    }

}
