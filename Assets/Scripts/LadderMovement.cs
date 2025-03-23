using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    [SerializeField] private float climbSpeedX = 0.1f;
    [SerializeField] private float climbSpeedY = 8f;
    private bool isClimbing;
    private bool onLadder;
    private Rigidbody2D rb;
    private float originalGravityScale;
    AudioManager audioManager;

    [SerializeField] private LayerMask ladderMask;
    private Player player;
    private bool isClimbingSoundPlaying = false;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        originalGravityScale = rb.gravityScale; // ��������� �������� ����������
    }

    private void FixedUpdate()
    {
        CheckLadder();
        Climbing();
    }

    private void CheckLadder()
    {
        // ���������, �������� �� ����� ��������
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 0.5f, ladderMask);
        onLadder = hitInfo.collider != null;

        // ���� ����� �� �������� � �������� �����������
        if (onLadder && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            isClimbing = true;
            // ������������� �������� �� �����, ���� ����� �� ��������
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else if (!onLadder)
        {
            isClimbing = false;
        }
    }

    private void Climbing()
    {
        if (isClimbing && onLadder)
        {
            // �������� ������������ ����
            float verticalInput = Input.GetAxisRaw("Vertical");

            // ���� ����� �� �����, �� ��������� �������� �� X
            float horizontalSpeed = player.IsGrounded() ? rb.velocity.x : rb.velocity.x * climbSpeedX;

            // ��������� ��������� �����/����
            rb.velocity = new Vector2(horizontalSpeed, verticalInput * climbSpeedY);

            // ��������� ���� �������, ���� �� �� �������������
            if (verticalInput != 0 && !isClimbingSoundPlaying)
            {
                audioManager.PlaySFX(audioManager.ladderClimbing);
                isClimbingSoundPlaying = true; // ��������, ��� ���� �������������
            }

            rb.gravityScale = 0f;
        }
        else
        {
            // ���� ����� �� ������, ���������� �������� �������� ����������
            rb.gravityScale = originalGravityScale;

            // ������������� ���� �������, ����� ����� �� �����
            isClimbingSoundPlaying = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            onLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            onLadder = false;
            isClimbing = false;
        }
    }

    public bool CanJumpFromLadder()
    {
        return onLadder;
    }

    public void ExitLadder()
    {
        isClimbing = false;
        rb.gravityScale = originalGravityScale;
    }
}
