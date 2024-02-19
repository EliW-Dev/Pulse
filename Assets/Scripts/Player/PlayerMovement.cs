using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Rigidbody2D _rBody;
    [SerializeField] private Transform _groundCheckAnchor;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask _groundLayerMask;

    private int _facingDirection = 1; //1 = facing right
    private float _originalScale = 0.0f;
    private float _jumpVelocity = 10.0f;
    

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rBody = GetComponent<Rigidbody2D>();

        _originalScale = transform.localScale.x;

        //bind Actions
        PlayerInput.OnPlayerJump += PlayerJump;

        if(_groundCheckAnchor == null)
        {
            foreach(Transform childTransform in transform.GetComponentsInChildren<Transform>())
            {
                if(childTransform.CompareTag("PlayerGroundCheck"))
                {
                    _groundCheckAnchor = childTransform;
                    break;
                }
            }

            if(_groundCheckAnchor == null) { Debug.Log("Warning! Player groundCheckAnchor not found!"); }
        }
    }

    private void OnDestroy()
    {
        //un-bind Actions
        PlayerInput.OnPlayerJump -= PlayerJump;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_playerInput == null || _rBody == null) { return; }

        ProcessPlayerMovement();
    }

    private void ProcessPlayerMovement()
    {
        float xVelocity = _playerInput.HorizontalInputVaue * 10.0f;
        //TODO - clamp speed

        //is the player character facing int the direction of input
        if (xVelocity * _facingDirection < 0.0f)
        {
            TogglePlayerDirection();
        }

        _rBody.velocity = new Vector2(xVelocity, _rBody.velocity.y);
    }

    private void TogglePlayerDirection()
    {

        _facingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = _originalScale * _facingDirection;

        transform.localScale = scale;
    }

    private void PlayerJump()
    {
        //Debug.DrawLine(groundCheckAnchor.position, groundCheckAnchor.position + (Vector3.down * groundCheckDistance), Color.red, 2.0f);
        if (!Physics2D.Raycast(_groundCheckAnchor.position, Vector2.down, _groundCheckDistance, _groundLayerMask)) return;

        _rBody.velocity = new Vector2(_rBody.velocity.x, _jumpVelocity);

    }
}
