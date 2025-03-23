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
        originalGravityScale = rb.gravityScale; // Сохраняем исходную гравитацию
    }

    private void FixedUpdate()
    {
        CheckLadder();
        Climbing();
    }

    private void CheckLadder()
    {
        // Проверяем, касается ли игрок лестницы
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 0.5f, ladderMask);
        onLadder = hitInfo.collider != null;

        // Если игрок на лестнице и пытается карабкаться
        if (onLadder && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            isClimbing = true;
            // Останавливаем движение по иксам, если игрок на лестнице
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
            // Получаем вертикальный ввод
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Если игрок на земле, не замедляем скорость по X
            float horizontalSpeed = player.IsGrounded() ? rb.velocity.x : rb.velocity.x * climbSpeedX;

            // Позволяем двигаться вверх/вниз
            rb.velocity = new Vector2(horizontalSpeed, verticalInput * climbSpeedY);

            // Запускаем звук лазания, если он не проигрывается
            if (verticalInput != 0 && !isClimbingSoundPlaying)
            {
                audioManager.PlaySFX(audioManager.ladderClimbing);
                isClimbingSoundPlaying = true; // Помечаем, что звук проигрывается
            }

            rb.gravityScale = 0f;
        }
        else
        {
            // Если игрок не лазает, возвращаем исходное значение гравитации
            rb.gravityScale = originalGravityScale;

            // Останавливаем звук лазания, когда игрок не лазит
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
