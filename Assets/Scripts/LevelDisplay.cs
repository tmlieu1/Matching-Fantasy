using UnityEngine;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    private TextMeshProUGUI levelCount;

    private void Awake() {
        levelCount = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        levelCount.text = "LVL " + GameManager.level.ToString();
    }
}
