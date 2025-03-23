using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour{
    /////////////////////////////////////////////////////////
    AudioManager audioManager;

    private Rigidbody2D _rb;
    private Animator _anim;

    [Header("-----------База так звана-----------")]
    [SerializeField] private GameObject[] _bodyParts;
    [SerializeField] private BoxCollider2D _groundCheck;
    [SerializeField] private BoxCollider2D _CeilingCheck;
    [SerializeField] private AudioSource _audioSource;

    [Header("-----------Еффекти-----------")]
    [SerializeField] private ParticleSystem _runParticles;
    [SerializeField] private ParticleSystem _jumpParticles;
    [SerializeField] private Light2D[] _lights;

    [Header("-----------Фізика і колізії-----------")]
    [SerializeField] private Collider2D _idleCollider;
    [SerializeField] private Collider2D _dashCollider;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _platformMask;



    [Header("-----------Звуки кроків-----------")]
    private Dictionary<int, AudioClip> _stepSoundsByLayer;
    [SerializeField] private LayerMask[] _stepSoundLayers;
    [SerializeField] private AudioClip[] _stepSounds;

   
/////////////////////////////////////////////////////////
   

    [Header("-----------Переміщення-----------")]
    private bool _isMoving = false;
    public float MoveSpeed;
    private float _horizontalInput;

    [Header("-----------Стрибок-----------")]
    public float JumpForce;
    private bool _grounded;
    private float _jumpTimeCounter;
    public float JumpTime;
    private bool _isJumping;
    private bool _wasGrounded = true;

    [Header("-----------Деш-----------")]
    private bool _canDash = true;
    private bool _isDashing;
    public float DashingPower = 12f;
    public float DashingTime = 0.1f;
    private float _dashingCooldown = 1f;

    //потім налаштую
    private bool isFallingSound= false;
    private float fallTime = 0f; // Время, сколько игрок уже падает
    private float fallSoundDelay = 0.4f;

    private bool _jumpedPlatformUp = false;



    /////////////////////////////////////////////////////////    

    private void Awake()
    {
       
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

    

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
 

        _stepSoundsByLayer = new Dictionary<int, AudioClip>();
        for (int i = 0; i < _stepSoundLayers.Length && i < _stepSounds.Length; i++)
        {
            _stepSoundsByLayer[_stepSoundLayers[i].value] = _stepSounds[i];
        }
    }

    private void Update()
    {
        if (_isDashing) return;
        _horizontalInput = Input.GetAxis("Horizontal");

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

        FlipCharacter(_horizontalInput);

        Jump();
        _grounded = IsGrounded();
        if (!_wasGrounded && _grounded)
        {
            Land();
        }
        _wasGrounded = _grounded;
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && IsGrounded())
        {
            _anim.SetTrigger("Dash");
            StartCoroutine(Dash());
        }
        WalkingSound();
        isFalling();

    }
       

   

    private void WalkingSound()
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
                        _audioSource.Play(); // Принудительно запускаем новый звук
                    }
                    else if (!_audioSource.isPlaying)
                    {
                        _audioSource.Play(); // Запускаем звук, если он по какой-то причине остановился
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

    private void FixedUpdate()
    {
        if (_isDashing) return;
        _rb.velocity = new Vector2(_horizontalInput * MoveSpeed, _rb.velocity.y);
        _anim.SetFloat("xVelocity", Mathf.Abs(_rb.velocity.x));
        _anim.SetFloat("yVelocity", _rb.velocity.y);


        if (_CeilingCheck.IsTouchingLayers(_groundMask))
        {
   
            _isJumping = false;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
        }
    }

    private void Jump()
    {
        LadderMovement ladder = GetComponent<LadderMovement>();
        bool canJumpFromLadder = ladder != null && ladder.CanJumpFromLadder();

        if ((IsGrounded() || (ladder != null && ladder.CanJumpFromLadder())) && Input.GetKeyDown(KeyCode.Space))
        {
            if (canJumpFromLadder)
            {
                ladder.ExitLadder();
            }
            audioManager.PlaySFX(audioManager.jump);
            _jumpParticles.Play();
            _isJumping = true;
            _jumpTimeCounter = JumpTime;
            _anim.SetBool("isJumping", true);
            _anim.SetTrigger("Jump");
            _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);

       
            if (!_grounded && _rb.velocity.y > 0 && _groundCheck.IsTouchingLayers(_platformMask))
            {
                _jumpedPlatformUp = true; // Игрок прыгнул через платформу вверх
            }

        }

        if (Input.GetKey(KeyCode.Space) && _isJumping)
        {
            if (!_grounded && _rb.velocity.y > 0 && _groundCheck.IsTouchingLayers(_platformMask))
            {
                _jumpedPlatformUp = true; // Игрок прыгнул через платформу вверх
            }
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
        if (IsGrounded() && !_isJumping)
        {
            _jumpParticles.Stop();
            _anim.SetBool("isJumping", false);
        
        }

    }

   

    private void Land()
    {

        if (_jumpedPlatformUp)
        {

            _jumpedPlatformUp = false;
            return; // Не проигрываем звук приземления
        }

        _anim.SetTrigger("Land");
     
        _jumpParticles.Play(); 
        audioManager.PlaySFX(audioManager.land);


    
    }

    public bool IsGrounded()
    {
        return _groundCheck.IsTouchingLayers(_groundMask)|| _groundCheck.IsTouchingLayers(_platformMask);

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

    private IEnumerator Dash()
    {
        audioManager.PlaySFX(audioManager.dash);
        _canDash = false;
        _isDashing = true;

        _idleCollider.isTrigger = true;
        _dashCollider.enabled = true;

        DisableLight();
        DisableBodyParts();

        //   _rb.velocity = new Vector2(transform.localScale.x * DashingPower, 0f);
        _rb.velocity = Vector2.zero;
        _rb.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * DashingPower, 0f), ForceMode2D.Impulse);

        yield return new WaitForSeconds(DashingTime);

        _isDashing = false;
        EnableLight();
        EnableBodyParts();

        yield return new WaitForSeconds(_dashingCooldown);
        _canDash = true;
        _idleCollider.isTrigger = false;
        _dashCollider.enabled = false;
    }

    private void isFalling()
    {
        if (!IsGrounded() && _rb.velocity.y < 0) // Если персонаж падает
        {
            _anim.SetBool("isFalling", true);
            

            fallTime += Time.deltaTime; // Считаем, сколько он уже падает

            if (fallTime >= fallSoundDelay && !isFallingSound)
            {

                audioManager.PlayFall(audioManager.falling);
                isFallingSound = true;


            }
        }
        else
        {
            
            audioManager.StopFallSound();
            fallTime = 0f;
            isFallingSound = false;
            _anim.SetBool("isFalling", false);
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

    private void DisableBodyParts()
    {
        foreach (var part in _bodyParts)
        {
            if (part.TryGetComponent<Collider2D>(out var col))
                col.enabled = false;

            if (part.TryGetComponent<SpriteRenderer>(out var sprite))
                sprite.enabled = false;
        }
    }

    private void EnableBodyParts()
    {
        foreach (var part in _bodyParts)
        {
            if (part.TryGetComponent<Collider2D>(out var col))
                col.enabled = true;

            if (part.TryGetComponent<SpriteRenderer>(out var sprite))
                sprite.enabled = true;
        }
    }
}