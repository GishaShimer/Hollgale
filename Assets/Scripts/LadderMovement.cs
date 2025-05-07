using System.Collections;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    [SerializeField] private float climbSpeedY = 8f;
    public bool isClimbing;
    public bool onLadder;

    public  Rigidbody2D rb;
    public float originalGravityScale;
    private SoundManager audioManager;
    private Player player;

    [SerializeField] private LayerMask ladderMask;
    [SerializeField] private BoxCollider2D _groundCheck;
    [SerializeField] private BoxCollider2D _ceilingCheck;

    private bool delay;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        CheckLadder();
        Climbing();
    }

    public void CheckLadder()
    {
        bool touchingLadderBottom = _groundCheck.IsTouchingLayers(ladderMask);
        bool touchingLadderTop = _ceilingCheck.IsTouchingLayers(ladderMask);

        onLadder = touchingLadderBottom || touchingLadderTop;

        if (onLadder && Input.GetAxisRaw("Vertical") != 0)
        {
            isClimbing = true;
        }
        else if (!onLadder)
        {
            isClimbing = false;
        }
      
    }

    private void Climbing()
    {
        if (isClimbing)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            float horizontalInput = player.IsGrounded() ? Input.GetAxisRaw("Horizontal") : 0f; // Проверка через Player

            rb.velocity = new Vector2(horizontalInput * climbSpeedY, verticalInput * climbSpeedY);

            if (verticalInput != 0 && !delay)
            {
                StartCoroutine(ClimbSound());
            }

            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = originalGravityScale;
        }
    }

    private IEnumerator ClimbSound()
    {
        audioManager.PlaySFX(audioManager.ladderClimbing, 1f);
        delay = true;
        yield return new WaitForSeconds(0.5f);
        delay = false;
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
            rb.gravityScale = originalGravityScale;
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
