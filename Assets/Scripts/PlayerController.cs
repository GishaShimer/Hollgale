using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour
{
    /////////////////////////////////////////////////////////
    // Компоненты
    [HideInInspector]public Rigidbody2D _rb;
    private Animator _anim;
    private SoundManager audioManager;
    private LadderMovement ladder;

    [SerializeField] private AudioSource _audioSource;

    // Основные настройки
    [Header("-----------База-----------")]
    [SerializeField] private GameObject[] _bodyParts;
    [SerializeField] private Light2D[] _lights;

    // Коллизии и физика
    [Header("-----------Физика и коллизии-----------")]
    [HideInInspector] public BoxCollider2D _groundCheck;
    [SerializeField] private BoxCollider2D _CeilingCheck;
    [SerializeField] private Collider2D _idleCollider;
    [SerializeField] private Collider2D _dashCollider;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _platformMask;

    // Эффекты
    [Header("-----------Эффекты-----------")]
    [SerializeField] private ParticleSystem _runParticles;
    [SerializeField] private ParticleSystem _jumpParticles;

    // Звуки шагов
    [Header("-----------Звуки шагов-----------")]
    private Dictionary<int, AudioClip> _stepSoundsByLayer;
    [SerializeField] private LayerMask[] _stepSoundLayers;
    [SerializeField] private AudioClip[] _stepSounds;

    // Перемещение
    [Header("-----------Перемещение-----------")]
    public float MoveSpeed;
    private bool _isMoving = false;
    private float _horizontalInput;

    // Прыжок
    [Header("-----------Прыжок-----------")]
    public float JumpForce;
    public float JumpTime;
    private float _jumpTimeCounter;
    private bool _isJumping;
    private bool _wasGrounded = true;
    private bool _jumpedPlatformUp = false;

    // Дэш
    [Header("-----------Дэш-----------")]
    public float DashingPower = 12f;
    public float DashingTime = 0.1f;
    private bool _canDash = true;
    [HideInInspector]public bool _isDashing;
    private float _dashingCooldown = 1f;

    // Падение
    [Header("-----------Падение-----------")]
    private bool isFallingSound = false;
    private float fallTime = 0f;
    private float fallSoundDelay = 0.6f;

    // Общее состояние
    public bool isEnabled;

    /////////////////////////////////////////////////////////



    private void Awake()
    {

            _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

            _audioSource = GetComponent<AudioSource>();
 
            ladder = GetComponent<LadderMovement>();
 
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();

        InitializeStepSounds();
        _dashCollider.enabled = false;
    }


    private void Update()
    {
        
        if (_isDashing || !isEnabled) return;
        Movement();
        Jump();

        StepSounds();
        IsFalling();
        SpeedrunMode();
        Land();

        _wasGrounded = IsGrounded();
        Dashing();
        _anim.SetBool("isGrounded", IsGrounded());

    }



    private void FixedUpdate()
    {
        if (_isDashing || !isEnabled) return;
        _rb.velocity = new Vector2(_horizontalInput * MoveSpeed, _rb.velocity.y);
   
            _anim.SetFloat("xVelocity", Mathf.Abs(_rb.velocity.x));
            _anim.SetFloat("yVelocity", _rb.velocity.y);


        if (_CeilingCheck.IsTouchingLayers(_groundMask))
        {
   
            _isJumping = false;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
        }
    }
    private void Movement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        FlipCharacter(_horizontalInput);

        if (_horizontalInput != 0 && IsGrounded())
        {
            if (!_runParticles.isPlaying) _runParticles.Play();
            _isMoving = true;
        }
        else
        {
            if (_runParticles.isPlaying) _runParticles.Stop();
            _isMoving = false;
        }
    }
    private void Dashing()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && IsGrounded())
        {
            _anim.SetTrigger("Dash");
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        audioManager.PlaySFX(audioManager.dash, 1f);
        _canDash = false;
        _isDashing = true;

        _idleCollider.isTrigger = true;
        _dashCollider.enabled = true;

        DisableLight();
        DisableBodyParts();

        _rb.velocity = Vector2.zero;
        _rb.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * DashingPower, 0f), ForceMode2D.Impulse);

        yield return new WaitForSeconds(DashingTime);

        _isDashing = false;

        yield return new WaitWhile(() =>
     Physics2D.OverlapBox(
         _CeilingCheck.bounds.center,
         _CeilingCheck.bounds.size,
         0f,
         _groundMask
     ) != null
 );
        EnableLight();
        EnableBodyParts();

        _idleCollider.isTrigger = false;
        _dashCollider.enabled = false;

        yield return new WaitForSeconds(_dashingCooldown);
        audioManager.PlaySFX(audioManager.dashRecovery, 1f);
        _canDash = true;
    }

    private void Jump()
    {
        bool canJumpFromLadder = ladder != null && ladder.CanJumpFromLadder();

        if ((IsGrounded() || canJumpFromLadder) && Input.GetKeyDown(KeyCode.Space))
        {
            if (canJumpFromLadder)
            {
                ladder.ExitLadder();
            }

            audioManager.PlaySFX(audioManager.jump, 1f);
            _jumpParticles.Play();
            _isJumping = true;
            _jumpTimeCounter = JumpTime;
            _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);

            // Проверяем, прыгает ли игрок сквозь платформу
            if (_groundCheck.IsTouchingLayers(_platformMask))
            {
                _jumpedPlatformUp = true;
            }
        }

        if (Input.GetKey(KeyCode.Space) && _isJumping)
        {
            if (_jumpTimeCounter > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isJumping = false;
        }

        // Сброс частицы прыжка, если приземлились
        if (IsGrounded() && !_isJumping)
        {
            _jumpParticles.Stop();
            _jumpedPlatformUp = false; // Сбрасываем флаг прыжка через платформу при приземлении
        }
    }


    private void IsFalling()
    {
        if (!IsGrounded() && _rb.velocity.y < 0)
        {
            fallTime += Time.deltaTime;

            if (fallTime >= fallSoundDelay && !isFallingSound)
            {
                audioManager.PlayAmbient(audioManager.falling, 1f);
                isFallingSound = true;
            }
        }
        else
        {
            audioManager.AmbientSource.Stop();
            fallTime = 0f;
            
        }
    }

    private void Land()
    {
        if (!_wasGrounded && IsGrounded())
        {
            if (_jumpedPlatformUp)
            {
                _jumpedPlatformUp = false;
                return;
            }

            if (isFallingSound)
            {
                isFallingSound = false;
                _anim.SetTrigger("Land");
                audioManager.PlaySFX(audioManager.hardLand, 1f);
                DisableControls();
                Invoke(nameof(EnableControls), 0.3f);
      
            }
            else
            {
                _anim.SetTrigger("Land");
                _jumpParticles.Play();
                audioManager.PlaySFX(audioManager.softLand, 1f);
            }
         

        }
    }

    private void StepSounds()
    {
        if (_isMoving && IsGrounded())
        {
            foreach (var entry in _stepSoundsByLayer)
            {
                if (_groundCheck.IsTouchingLayers(entry.Key))
                {
                    if (_audioSource.clip != entry.Value)
                    {
                        _audioSource.clip = entry.Value;
                        _audioSource.Play();
                    }
                    else if (!_audioSource.isPlaying)
                    {
                        _audioSource.Play();
                    }
                    return;
                }
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }

    private void DisableLight()
    {
        foreach (var light in _lights)
        {
            light.enabled = false;
        }
    }

    private void EnableLight()
    {
        foreach (var light in _lights)
        {
            light.enabled = true;
        }
    }

    public void DisableControls()
    {

        _rb.velocity = Vector2.zero;
        isEnabled = false;
    }
    public void EnableControls()
    {
        isEnabled = true;
    }
  


    public void DisableBodyParts()
    {
        foreach (var part in _bodyParts)
        {
            if (part.TryGetComponent<Collider2D>(out var col))
                col.enabled = false;

            if (part.TryGetComponent<SpriteRenderer>(out var sprite))
                sprite.enabled = false;
        }
    }



    public void EnableBodyParts()
    {
        foreach (var part in _bodyParts)
        {
            if (part.TryGetComponent<Collider2D>(out var col))
                col.enabled = true;

            if (part.TryGetComponent<SpriteRenderer>(out var sprite))
                sprite.enabled = true;
        }
    }
    private void InitializeStepSounds()
    {

        _stepSoundsByLayer = new Dictionary<int, AudioClip>();
        for (int i = 0; i < _stepSoundLayers.Length && i < _stepSounds.Length; i++)
        {
            _stepSoundsByLayer[_stepSoundLayers[i].value] = _stepSounds[i];
        }
    }

    private void FlipCharacter(float input)
    {
        if (input > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (input < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void SpeedrunMode()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            MoveSpeed = 15f;
            JumpForce = 14f;
            DashingPower = 20f;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            MoveSpeed = 6f;
            JumpForce = 9f;
            DashingPower = 10f;
        }
    }
    public bool IsGrounded()
    {
        return _groundCheck.IsTouchingLayers(_groundMask) || _groundCheck.IsTouchingLayers(_platformMask);

    }
    public bool IsPlatform()
    {
        return !_groundCheck.IsTouchingLayers(_groundMask) && _groundCheck.IsTouchingLayers(_platformMask);

    }


}