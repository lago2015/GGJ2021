using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Specific for inverted Robot")]
    public bool MovementInverted;
    public float FireRate;
    public float MoveSpeed;
    public float JumpForce;
    public Vector2 ColliderSlidingOffset;
    public Vector2 ColliderSlidingSize;
    public LayerMask FloorMask;
    public Transform GunLeftTransform;
    public Transform GunRightTransform;
    public GameObject BulletPrefab;

    private float _cooldownTime;
    private float _inputSpeed = 1;
    private float _isJumpingAxis;
    private float _floorCheckDistance = 0.25f;

    private bool _isJumping;
    private bool _isGrounded;
    private bool _isShooting;
    private bool _isSliding;
    private bool _isActive;
    private bool _ableToShoot;
    private bool _ableToSlide;

    private Vector2 _defaultSlidingOffset;
    private Vector2 _defaultSlidingSizeY;
    private PlayerAnimation _actor;
    private RaycastHit2D _ray;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;
    private int _specialDirection;    //meant to give either 1 or -1 to give the opposite robot an inverted behavior

    private const string _jump1ButtonName = "Jump";
    private const string _shoot1ButtonName = "Fire1";
    private const string _shoot2ButtonName = "Fire2";
    private const string _slide1ButtonName = "Slide1";
    private const string _slide2ButtonName = "Slide2";
    public bool IsActive
    {
        get => _isActive;
        set
        {

            if (_actor) _actor.SetMoveSpeed(value);
            if (!value) StopAllCoroutines();

            _isActive = value;
        }
    }


    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _defaultSlidingOffset = _collider.offset;
        _defaultSlidingSizeY = _collider.size;
        _rb = GetComponent<Rigidbody2D>();
        _actor = GetComponent<PlayerAnimation>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _specialDirection = MovementInverted ? -1 : 1;

        var key = MovementInverted ? DataKeys.PLAYERB : DataKeys.PLAYERA;
        DataManager.ToTheCloud(key, this);
    }

    private void OnEnable()
    {
        _ableToShoot = true;
        _ableToSlide = true;
        _isGrounded = true;
        _isJumping = false;
        _isShooting = false;
        _isSliding = false;

        if (!MovementInverted)
        {
            DataManager.ToTheCloud(DataKeys.PLAYERA_STARTLINE, transform.position);
        }
    }

    private void Update()
    {
        if (!IsActive)
        {
            _actor.SetMoveSpeed(false);
            return;
        }
        Movement();
        Slide();
        Jump();
        //Shoot();
        FloorAndFallingCheck();

    }

    private void Slide()
    {
        if (!_isGrounded || _isSliding) return;
        _isSliding = Input.GetButtonDown(MovementInverted ? _slide2ButtonName : _slide1ButtonName);
        if (_isSliding && _ableToSlide)
        {
            _ableToSlide = false;
            _actor.SetAnimation(PlayerStates.Slide);
            StartCoroutine(SlideDuration());
        }
    }

    IEnumerator SlideDuration()
    {
        yield return new WaitForSeconds(0.25f);
        _collider.offset = ColliderSlidingOffset;
        _collider.size = ColliderSlidingSize;
        yield return new WaitForSeconds(_actor.AnimationLength);
        _actor.SetAnimation(PlayerStates.Grounded);
        yield return new WaitForSeconds(0.25f);
        _isSliding = false;
        _collider.offset = _defaultSlidingOffset;
        _collider.size = _defaultSlidingSizeY;
        _ableToSlide = true;
    }

    private void Shoot()
    {
        _isShooting = Input.GetButtonDown(MovementInverted ? _shoot2ButtonName : _shoot1ButtonName);
        if (_isShooting && _ableToShoot)
        {
            _ableToShoot = false;
            _cooldownTime = Time.time + FireRate;
            var spawnPoint = _spriteRenderer.flipX ? GunLeftTransform : GunRightTransform;
            Instantiate(BulletPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else if (Time.time > _cooldownTime)
        {
            _ableToShoot = true;
        }
    }

    private void Jump()
    {
        if (_isSliding) return;
        //Jumping Check
        _isJumpingAxis = Input.GetAxis(_jump1ButtonName);
        if (_isJumpingAxis > 0 && _isGrounded && !_isJumping)
        {
            _rb.AddForce(transform.up * JumpForce);
            _actor.SetAnimation(PlayerStates.Jumping);
            StartCoroutine(DelayToCheckFloor());
            _isJumping = true;
        }
    }

    private void FloorAndFallingCheck()
    {
        //Checking if player is falling
        if (_rb.velocity.y < 0)
        {
            _actor.SetAnimation(PlayerStates.Falling);
            _isJumping = true;
            _isGrounded = false;
        }
        //Floor Checking
        if (!_isGrounded && _isJumping)
        {
            _ray = Physics2D.Raycast(transform.position, -transform.up, _floorCheckDistance, FloorMask);
            //Debug.DrawRay(transform.position,-transform.up * _floorCheckDistance,Color.red,2);
            if (_ray.collider != null)
            {
                _isJumping = false;
                _actor.SetAnimation(PlayerStates.Grounded);
                StartCoroutine(DelayToEnableJump());
            }
        }
    }

    IEnumerator DelayToEnableJump()
    {
        yield return new WaitForFixedUpdate();
        _isGrounded = true;
    }

    IEnumerator DelayToCheckFloor()
    {
        yield return new WaitForSeconds(0.25f);
        _isGrounded = false;
    }

    private void Movement()
    {
        //Movement
        transform.position += new Vector3(_inputSpeed, 0, 0) * Time.deltaTime * MoveSpeed * _specialDirection;
    }
}
