using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(int soundNum)
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            _audio.clip = sounds[soundNum];
            _audio.Play();
        }
    }
    public void PlaySound(SoundType soundType)
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            _audio.clip = sounds[(int)soundType];
            _audio.Play();
        }
    }
}

[SerializeField]
public enum SoundType
{
    CardClick,
    WinSound,
    IncorrectClick
}
