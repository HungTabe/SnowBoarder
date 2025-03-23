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
    private bool hasJumped;  // Kiểm tra xem người chơi đã nhảy chưa
    private bool isGrounded;  // Kiểm tra khi người chơi chạm đất



    private void Awake()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.IsAlive.Value)
        {
            this.inputJump = true;
            this.hasJumped = true;
            this.jumpStartTime = Time.time;  // Ghi lại thời gian khi cú nhảy bắt đầu
        }
    }

    private void FixedUpdate()
    {
        // Player jump
        if (this.inputJump && this.RemainingJumps.Value > 0)
        {
            this.Jump(this.JumpForce.Value);
            this.RemainingJumps.ApplyChange(-1);
            this.PlayerJumpEvent.Invoke();

      

#if UNITY_EDITOR
            Debug.Log(string.Format("PlayerJump.Jump [JumpForce: {0}] [RemainingJumps: {1}]", this.JumpForce.Value, this.RemainingJumps.Value));
            #endif
        }

        // Cleanup
        this.inputJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra khi nhân vật chạm đất
        if (collision.gameObject.CompareTag("Ground") && isGrounded == false)
        {
            isGrounded = true;  // Đánh dấu đã chạm đất

            // Tính thời gian nhảy
            float jumpDuration = Time.time - jumpStartTime;

            // Kiểm tra thời gian nhảy và cộng điểm
            if (jumpDuration <= 3f)  // Nếu nhảy dưới 3 giây
            {
                scoreManager.AddScore(50);  // Cộng thêm điểm cho nhảy nhanh
            }

            // Kiểm tra flip (thực hiện logic flip)
            if (Mathf.Abs(rigidBody.angularVelocity) > 0)  // Điều kiện giả sử có flip (lúc nhân vật xoay)
            {
                scoreManager.AddScore(200);  // Cộng điểm cho flip
            }

            // Cập nhật thành tích nếu cần
            achievementsManager.OnFlipPerformed();
            achievementsManager.OnScoreMilestoneReached();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Kiểm tra khi nhân vật rời khỏi mặt đất
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;  // Đánh dấu đã rời đất
        }
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
