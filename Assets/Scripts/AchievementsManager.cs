using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    public int flipsInRow = 0;  // Đếm số cú flip liên tiếp
    public int scoreMilestone = 500;  // Mốc điểm để đạt thành tích
    private bool achieved500Points = false;

    public ScoreManager scoreManager; // Lấy đối tượng ScoreManager

    public void OnFlipPerformed()
    {
        flipsInRow++;
        if (flipsInRow >= 10)  // Nếu thực hiện 10 cú flip liên tiếp
        {
            UnlockAchievement("10 Flips in a Row");
        }
    }

    public void OnScoreMilestoneReached()
    {
        if (!achieved500Points && scoreManager.score >= scoreMilestone)
        {
            achieved500Points = true;
            UnlockAchievement("500 Points in One Run");
        }
    }

    private void UnlockAchievement(string achievementName)
    {
        Debug.Log("Unlocked Achievement: " + achievementName);
        // Bạn có thể hiển thị thông báo UI hoặc lưu thành tích ở đây
    }
}
