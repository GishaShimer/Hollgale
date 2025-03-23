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
            _hasJumpedOff = true; // ��������� ��������� ���������������
            audioManager?.PlaySFX(audioManager.jumpingOff);

            _collider.enabled = false;
            StartCoroutine(EnableCollider());
        }

        // ��������� �������������� ��������: ���� ����� �� �������� ���������, ���������� ����
        if (!_collider.enabled)
        {
            _playerOnPlatform = false;
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;
        _hasJumpedOff = false; // ���������� ���� ������ ����� ���������� �������
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
