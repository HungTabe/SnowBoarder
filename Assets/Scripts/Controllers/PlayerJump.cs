using UnityEngine;
using UnityEngine.Events;

public class PlayerJump : MonoBehaviour
{
    public FloatVariable JumpForce;

    public IntVariable RemainingJumps;

    public BoolVariable IsAlive;

    [Tooltip("Event invoked when player jumps.")]
    public UnityEvent PlayerJumpEvent;

    private Rigidbody2D rigidBody;

    private bool inputJump;

    public ScoreManager scoreManager;

    public AchievementsManager achievementsManager;

    private float jumpStartTime;  // Thời gian bắt đầu cú nhảy
    private bool isGrounded;  // Kiểm tra khi người chơi chạm đất

    private bool isJumping;  // Kiểm tra xem người chơi có đang nhảy không
    private float jumpDuration;  // Thời gian nhảy



    private void Awake()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Kiểm tra nhấn phím nhảy
        if (Input.GetKeyDown(KeyCode.Space) && this.IsAlive.Value && RemainingJumps.Value > 0)
        {
            this.inputJump = true;
            this.isJumping = true;  // Đánh dấu nhân vật đang nhảy
            this.jumpStartTime = Time.time;  // Ghi lại thời gian khi cú nhảy bắt đầu
        }


        // Debug Log các giá trị của inputJump và isJumping
        Debug.Log("inputJump: " + inputJump + ", isJumping: " + isJumping);
    }

    private void FixedUpdate()
    {
        // Thực hiện cú nhảy nếu người chơi nhấn phím nhảy và còn lần nhảy
        if (this.inputJump && this.RemainingJumps.Value > 0)
        {
            this.Jump(this.JumpForce.Value);
            this.RemainingJumps.ApplyChange(-1);
            this.PlayerJumpEvent.Invoke();

            // Cộng điểm mỗi khi người chơi nhảy
            scoreManager.AddScore(100);

            // Cleanup sau khi thực hiện cú nhảy
            this.inputJump = false;
        }

        // Debug Log giá trị isGrounded sau mỗi lần kiểm tra
        Debug.Log("isGrounded in FixedUpdate: " + isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra khi nhân vật chạm đất
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;  // Đánh dấu đã chạm đất
            this.isJumping = false;

            // Tính tổng thời gian nhảy từ lúc nhảy đến khi chạm đất
            jumpDuration = Time.time - jumpStartTime;

            // Cộng điểm nếu thời gian nhảy dưới 3 giây
            if (jumpDuration <= 3f)
            {
                scoreManager.AddScore(50);  // Cộng thêm điểm cho cú nhảy nhanh
            }

            // Cộng điểm nếu nhân vật thực hiện flip trong không trung
            if (Mathf.Abs(rigidBody.angularVelocity) > 0)
            {
                scoreManager.AddScore(200);  // Cộng điểm cho flip
            }

            // Cập nhật thành tích nếu cần
            achievementsManager.OnFlipPerformed();
            achievementsManager.OnScoreMilestoneReached();
        }

        // Debug Log các giá trị của isGrounded và jumpDuration khi chạm đất
        Debug.Log("OnCollisionEnter2D -> isGrounded: " + isGrounded + ", jumpDuration: " + jumpDuration);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Kiểm tra khi nhân vật rời khỏi mặt đất (bắt đầu bay lên)
        if (collision.gameObject.CompareTag("Ground") && this.inputJump)
        {
            isGrounded = false;  // Đánh dấu đã rời đất

        }

        // Debug Log giá trị của isGrounded khi rời khỏi mặt đất
        Debug.Log("OnCollisionExit2D -> isGrounded: " + isGrounded);
    }

    /// <summary>
    /// Increase vertical velocity of the player.
    /// </summary>
    /// <param name="force"></param>
    private void Jump(float force)
    {
        this.rigidBody.linearVelocity = new Vector2(this.rigidBody.linearVelocity.x, (Vector2.up.y * force));
        this.inputJump = false;
    }
}
