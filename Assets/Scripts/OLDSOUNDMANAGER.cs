using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDSOUNDMANAGER : MonoBehaviour
{/*
    public static SoundManager S;

    public AudioSource _MainMenuMusic;
    public AudioSource _FooFooMusic;

    public AudioSource _FooFooDeathSound;
    public AudioSource _MouseDeathSound;
    public AudioSource _HammerHitSound;
    public AudioSource _HammerSwingSound;
    public AudioSource _MouseThrowFailSound;
    public AudioSource _MouseThrowSuccessSound;

    public AudioClip[] _HammerHitSounds;
    public AudioClip[] _HammerSwingSounds;

    private AudioSource[] Sources;

    /*
    [SerializeField]
    private AudioClip[] _UISounds;
    [SerializeField]
    private AudioClip[] _DropItemSounds;
    
    private AudioClip UISound;
    private AudioClip BouncySound;
    private AudioClip DropItemSound;
    private AudioClip DeathSound;
    private AudioClip WallHitSound;
    private AudioClip BouncerSound;
    private AudioClip LevelMusic1;
    private AudioClip LevelMusic2;
    private AudioClip LevelMusic3;
    private AudioClip BlackHoleSound;
    private AudioSource[] Sources;
    
    "*"/


    [SerializeField]
    private float lowRandom = 0.95f;
    [SerializeField]
    private float highRandom = 1.05f;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        // assign the singleton
        S = this;

        Sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
    }

    public void FadeIn(AudioSource audioSource)
    {
        float volume = audioSource.GetComponent<AudioSource>().volume;
        volume = 0.0f;
        audioSource.Play();

        if (volume < 1.0f)
        {
            volume += 0.1f * Time.deltaTime;
            audioSource.GetComponent<AudioSource>().volume = volume;
        }
        audioSource.volume = 1.0f;
    }

    public void FadeOut()
    {
        foreach (AudioSource playingSource in Sources)
        {
            if (playingSource.isPlaying)
            {
                float volume = playingSource.GetComponent<AudioSource>().volume;
                if (volume > 0.1)
                {
                    volume -= 0.1f * Time.deltaTime;
                    playingSource.GetComponent<AudioSource>().volume = volume;
                }
                playingSource.volume = 0.0f;
                playingSource.Stop();
            }
        }
    }
    
    // Music
    public void MakeFooFooMusicSound()
    {
        _FooFooMusic.Play();
    }

    // Sounds
    public void MakeFooFooDeathSound()
    {
        _FooFooDeathSound.Play();
    }

    public void MakeMouseDeathSound()
    {
        _MouseDeathSound.Play();
    }

    public void MakeHammerHitSound()
    {
        int randNum = Random.Range(0, _HammerHitSounds.Length);
        _HammerHitSound.clip = _HammerHitSounds[randNum];
        float newPitch = Random.Range(lowRandom, highRandom);
        _HammerHitSound.pitch = newPitch;
        _HammerHitSound.Play();
    }

    public void MakeHammerSwingSound()
    {
        int randNum = Random.Range(0, _HammerSwingSounds.Length);
        _HammerSwingSound.clip = _HammerSwingSounds[randNum];
        float newPitch = Random.Range(lowRandom, highRandom);
        _HammerSwingSound.pitch = newPitch;
        _HammerSwingSound.Play();
    }

    public void MakeMouseThrowFailSound()
    {
        _MouseThrowFailSound.Play();
    }

    public void MakeMouseThrowSuccessSound()
    {
        _MouseThrowSuccessSound.Play();
    }

    /*
    public void MakeDropItemSound()
    {
        int randNum = Random.Range(0, _DropItemSounds.Length);
        DropItemSound = _DropItemSounds[randNum];
        _DropItemSound.clip = DropItemSound;
        float newPitch = Random.Range(lowRandom, highRandom);
        _DropItemSound.pitch = newPitch;
        _DropItemSound.Play();
    }
    */
}