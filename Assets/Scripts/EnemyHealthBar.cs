using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    void Update() {
        slider.value = Enemy.hp;
    }
}
