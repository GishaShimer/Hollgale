using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PlatformDrop : MonoBehaviour
{
    private Collider2D _collider;
    private bool _playerOnPlatform;
    private bool _hasJumpedOff;

    private SoundManager audioManager;
    private int playerLayer;
    Player player;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<SoundManager>();
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        _collider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (_playerOnPlatform && Input.GetKeyDown(KeyCode.S) &&!_hasJumpedOff)
        {
            _collider.enabled = false;
            _hasJumpedOff = true;

            audioManager?.PlaySFX(audioManager.jumpingOff, 1f);
            
            StartCoroutine(EnableCollider());
        }
        if (!_collider.enabled)
        {
            _playerOnPlatform = false;
        }
    }
 
    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;
        _hasJumpedOff = false; // —брасываем флаг только после завершени€ таймера
    }

    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            _playerOnPlatform = value;

        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && player.IsPlatform())
        {
            SetPlayerOnPlatform(other, true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {   
        if (other.gameObject.CompareTag("Player"))
        {
            SetPlayerOnPlatform(other, false);

        }
    }

}
