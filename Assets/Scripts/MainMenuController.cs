using UnityEngine;
using UnityEngine.SceneManagement;  // Đảm bảo sử dụng namespace này để quản lý các scene


public class MainMenuController : MonoBehaviour
{
    // Hàm này sẽ được gọi khi nhấn nút Play
    public void PlayGame()
    {
        // Chuyển đến Scene GamePlay
        SceneManager.LoadScene("Scene0");  // "GamePlay" là tên của scene bạn muốn chuyển tới
    }

    // Hàm này có thể gọi khi nhấn nút Upgrade Power
    public void UpgradePower()
    {
        // Logic nâng cấp sức mạnh (chưa có, bạn có thể tạo menu nâng cấp)
        SceneManager.LoadScene("UpgradeMenu");
    }

    // Hàm này có thể gọi khi nhấn nút Upgrade Skin
    public void Quit()
    {
        SceneManager.LoadScene("MainMenu2D");
    }
}
