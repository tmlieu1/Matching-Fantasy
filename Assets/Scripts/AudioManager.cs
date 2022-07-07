using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public static Boolean muted = false;
    public static Boolean adminMode = false;

    // init each sound in the manager
    void Awake() {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //plays song on menu
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            Play("menu");
        }
    }

    // play the sound, to call this in other scripts use:
    // FindObjectOfType<AudioManager>().Play("soundname");
    // Make sure the sound exists in the AudioManager first.
    public void Play (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            return;
        }
        s.source.Play();
    }

    // stop a sound
    public void Stop (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            return;
        }
        s.source.Stop();
    }

    // toggles the sound
    public void SoundToggle() {
        if (PlayerPrefs.GetInt("Muted", 0) == 0) {
            PlayerPrefs.SetInt("Muted", 1);
        }
        else {
            PlayerPrefs.SetInt("Muted", 0);
        }
    }
    
    // toggles the admin permissions
    // 0 means sound on, 1 means sound off
    public void AdminToggle(){
        if (PlayerPrefs.GetInt("Admin", 0) == 0){
            PlayerPrefs.SetInt("Admin", 1);
        }
        else {
            PlayerPrefs.SetInt("Admin", 0);
        }
    }
}
