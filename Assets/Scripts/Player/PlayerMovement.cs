using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody2D rBody;
    [SerializeField] private Transform groundCheckAnchor;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayerMask;

    private int facingDirection = 1; //1 = facing right
    private float originalScale = 0.0f;
    private float jumpVelocity = 10.0f;
    

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rBody = GetComponent<Rigidbody2D>();

        originalScale = transform.localScale.x;

        //bind Actions
        PlayerInput.OnPlayerJump += PlayerJump;

        if(groundCheckAnchor == null)
        {
            foreach(Transform childTransform in transform.GetComponentsInChildren<Transform>())
            {
                if(childTransform.CompareTag("PlayerGroundCheck"))
                {
                    groundCheckAnchor = childTransform;
                    break;
                }
            }

            if(groundCheckAnchor == null) { Debug.Log("Warning! Player groundCheckAnchor not found!"); }
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
        if (playerInput == null || rBody == null) { return; }

        ProcessPlayerMovement();
    }

    private void ProcessPlayerMovement()
    {
        float _xVelocity = playerInput.HorizontalInputVaue * 10.0f;
        //TODO - clamp speed

        //is the player character facing int the direction of input
        if (_xVelocity * facingDirection < 0.0f)
        {
            TogglePlayerDirection();
        }

        rBody.velocity = new Vector2(_xVelocity, rBody.velocity.y);
    }

    private void TogglePlayerDirection()
    {

        facingDirection *= -1;

        Vector3 _scale = transform.localScale;
        _scale.x = originalScale * facingDirection;

        transform.localScale = _scale;
    }

    private void PlayerJump()
    {
        //Debug.DrawLine(groundCheckAnchor.position, groundCheckAnchor.position + (Vector3.down * groundCheckDistance), Color.red, 2.0f);
        if (!Physics2D.Raycast(groundCheckAnchor.position, Vector2.down, groundCheckDistance, groundLayerMask)) return;

        rBody.velocity = new Vector2(rBody.velocity.x, jumpVelocity);

    }
}
