using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance {
        get
        {
            return _instance;
        }
    }




    private float minpitch = 0.9f;
    private float maxpitch = 1.1f;

    public AudioSource efxSource;
    public AudioSource bgSource;

    public void RandomPlay(params AudioClip[] clips)
    {
        float pitch = Random.Range(minpitch,maxpitch);
        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        efxSource.clip = clip;
        efxSource.pitch = pitch;
        efxSource.Play();

    }
    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }

    public void StopBg()
    {
        bgSource.Stop();
    }
}
