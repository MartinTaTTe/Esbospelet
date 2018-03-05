using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    
    public AudioSource audioManager;
    public TimeManager timeManager;

    public AudioClip[] soundtrack;
    public AudioClip[][] music;

    public int lastMusicAge = 1950;

    public void SetMusic(int age)
    {
        if(age == 1950)
        {
            soundtrack = music[0];
            lastMusicAge = 1950;
        }else if (age == 1960)
        {
            soundtrack = music[1];
            lastMusicAge = 1960;
        }
        else if (age == 1970)
        {
            soundtrack = music[2];
            lastMusicAge = 1970;
        }
        else if (age == 1980)
        {
            soundtrack = music[3];
            lastMusicAge = 1980;
        }
        else if (age == 1990)
        {
            soundtrack = music[4];
            lastMusicAge = 1990;
        }
        else if (age == 2000)
        {
            soundtrack = music[5];
            lastMusicAge = 2000;
        }
        else if (age > 1950)
        {
            Debug.Log("Error: Music age is too big");
        }
    }

    // Use this for initialization
    void Start()
    {
        InitMusic();
        SetMusic(1950);

        audioManager = GameObject.FindGameObjectWithTag("_Manager").GetComponent<AudioSource>();
        timeManager = GameObject.FindGameObjectWithTag("_Manager").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bool decadeChange = false;

        if (lastMusicAge + 10 <= timeManager.GetCurrentYear())
        {
            SetMusic(lastMusicAge + 10);
            decadeChange = true;
        }

        if (!audioManager.isPlaying || decadeChange)
        {
            audioManager.Stop();
            audioManager.clip = soundtrack[Random.Range(0, soundtrack.Length)];
            audioManager.Play();
        }
    }

    void InitMusic()
    {
        music = new AudioClip[][] {
    new AudioClip[] {
        Resources.Load<AudioClip>("Music/1950/blow_me") ,
        Resources.Load<AudioClip>("Music/1950/Elektronik_Supersonic_Copyrighted") ,
        Resources.Load<AudioClip>("Music/1950/jazz_winds") ,
        Resources.Load<AudioClip>("Music/1950/piano_sway") ,
        Resources.Load<AudioClip>("Music/1950/raw_rivalry") ,
        Resources.Load<AudioClip>("Music/1950/slowing_down_time") ,
        Resources.Load<AudioClip>("Music/1950/string_swing") ,
        Resources.Load<AudioClip>("Music/1950/string_wing") ,
        Resources.Load<AudioClip>("Music/1950/struttin") ,
        Resources.Load<AudioClip>("Music/1950/swing_thing") },
    new AudioClip[] {
        Resources.Load<AudioClip>("Music/1960/all_blues_time") ,
        Resources.Load<AudioClip>("Music/1960/funk_machine") ,
        Resources.Load<AudioClip>("Music/1960/funk_the_scratch") ,
        Resources.Load<AudioClip>("Music/1960/octo_blues") ,
        Resources.Load<AudioClip>("Music/1960/soul_of_blues") ,
        Resources.Load<AudioClip>("Music/1960/southern_bluesmen") ,
        Resources.Load<AudioClip>("Music/1960/steady_joe") ,
        Resources.Load<AudioClip>("Music/1960/way_cool") },
    new AudioClip[] {
        Resources.Load<AudioClip>("Music/1970/70s_rocker") ,
        Resources.Load<AudioClip>("Music/1970/scene_around_town") ,
        Resources.Load<AudioClip>("Music/1970/shuffle_my_soul") ,
        Resources.Load<AudioClip>("Music/1970/slow_climb") ,
        Resources.Load<AudioClip>("Music/1970/sun_never_sets") ,
        Resources.Load<AudioClip>("Music/1970/unrelenting") },
    new AudioClip[] {
        Resources.Load<AudioClip>("Music/1980/piano_power") ,
        Resources.Load<AudioClip>("Music/1980/pumped_up") ,
        Resources.Load<AudioClip>("Music/1980/spin") ,
        Resources.Load<AudioClip>("Music/1980/turn_up_the_heat") },
    new AudioClip[] {
        Resources.Load<AudioClip>("Music/1990/future_groove") ,
        Resources.Load<AudioClip>("Music/1990/rave_nation") ,
        Resources.Load<AudioClip>("Music/1990/rave_wave") ,
        Resources.Load<AudioClip>("Music/1990/speak_easy") },
    new AudioClip[] {
        Resources.Load<AudioClip>("Music/2000/let_go") }
    };
    }
}
