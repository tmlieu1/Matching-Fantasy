using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI scoreCount;

    private void Awake() {
        scoreCount = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        scoreCount.text = "Score: " + GameManager.score.ToString();
    }
}
