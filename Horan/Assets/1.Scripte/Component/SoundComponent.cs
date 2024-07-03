using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    GameObject Prefab ;
    public string SoundName;

    public Animator Anim;
    public string AnimName;

    AudioSource Audio;

    bool isLoop;
    bool playOnce;
    private void Start()
    {
        Prefab = InstanceSoundObejct(SoundName);
        Audio = Prefab.GetComponent<AudioSource>();
        isLoop = Prefab.GetComponent<AudioSource>().loop;
        if (isLoop)
            PlaySound(0.4f);
    }
    private void Update()
    {
        if (Anim != null && !isLoop)
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
            {
                var normalizedTime = Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (normalizedTime != 0 && normalizedTime < 1f && !playOnce)
                {
                    PlaySound();
                    playOnce = true;
                }
            }
            else
                playOnce = false;
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
            if (Audio && !Audio.isPlaying)
            {
                Audio.volume = volume;
                Audio.Play();
            }
        }
    }

}
