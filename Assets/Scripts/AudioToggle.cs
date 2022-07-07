using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    private AudioManager manager;
    public Button soundToggleButton;
    public Button adminToggleButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    void Start () {
        manager = GameObject.FindObjectOfType<AudioManager>();
        if (PlayerPrefs.GetInt("Muted", 0) == 0) {
            soundToggleButton.GetComponent<Image>().sprite = soundOnSprite;
            adminToggleButton.GetComponent<Image>().sprite = soundOnSprite;
            AudioListener.volume = 1;
        }
        else {
            soundToggleButton.GetComponent<Image>().sprite = soundOffSprite;
            adminToggleButton.GetComponent<Image>().sprite = soundOffSprite;
            AudioListener.volume = 0;
        }
    }

    public void StopSound() {
        if (PlayerPrefs.GetInt("Admin", 0) == 0) {
            manager.SoundToggle();
            UpdateSound();
        }
    }

    public void AdminSound() {
        manager.SoundToggle();
        manager.AdminToggle();
        UpdateSound();
    }

    void UpdateSound() {
        if (PlayerPrefs.GetInt("Muted", 0) == 0) {
            AudioListener.volume = 1;
            soundToggleButton.GetComponent<Image>().sprite = soundOnSprite;
            adminToggleButton.GetComponent<Image>().sprite = soundOnSprite;
        }
        else {
            AudioListener.volume = 0;
            soundToggleButton.GetComponent<Image>().sprite = soundOffSprite;
            adminToggleButton.GetComponent<Image>().sprite = soundOffSprite;
        }
    }
}
