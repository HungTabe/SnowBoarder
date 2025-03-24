using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI achievementText;

    public void UpdateUI(int score, string achievement)
    {
        scoreText.text = "Score: " + score.ToString();
        achievementText.text = "Achievement: " + achievement;
    }
}
