using UnityEngine;
using System.Collections;

public class PlatformDrop : MonoBehaviour
{
    private Collider2D _collider;
    private bool _playerOnPlatform;
    private bool _hasJumpedOff;

    private AudioManager audioManager;
    private int playerLayer;

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0 && !_hasJumpedOff)
        {
            _hasJumpedOff = true; // Блокируем повторное воспроизведение
            audioManager?.PlaySFX(audioManager.jumpingOff);

            _collider.enabled = false;
            StartCoroutine(EnableCollider());
        }

        // Добавляем дополнительную проверку: если игрок не касается платформы, сбрасываем флаг
        if (!_collider.enabled)
        {
            _playerOnPlatform = false;
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;
        _hasJumpedOff = false; // Сбрасываем флаг только после завершения таймера
    }

    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            _playerOnPlatform = value;

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            SetPlayerOnPlatform(other, true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            SetPlayerOnPlatform(other, false);

        }
    }
}
