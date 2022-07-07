using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image image;

    void Update() {
        slider.value = GameManager.currPlayerHp/GameManager.maxPlayerHp;
        if (GameManager.playerDefend == true) {
            image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else image.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    }
}
