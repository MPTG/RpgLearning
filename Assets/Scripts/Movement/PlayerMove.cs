using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private Vector3 moveDirection;
    private Rigidbody2D rb;
    private Animator animator;
    string[] playerMoveAnim = { "dontMove", "DownStay", "downWalk", "LeftWalk", "MoveUp", "SideStay" };
    int lastMoveAnimIndex = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastMoveAnimIndex = 0;
        animator.Play(playerMoveAnim[lastMoveAnimIndex]);
    }

    private void Update()
    {
        // Get input axes
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate the movement direction
        moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        // Update the animation index based on the input
        UpdateAnimationIndex();
    }

    private void FixedUpdate()
    {
        // Move the character using rigidbody physics
        rb.velocity = moveDirection * moveSpeed;
    }

    private void UpdateAnimationIndex()
    {
        if (moveDirection.magnitude > 0)
        {
            // Character is moving, determine the animation based on direction
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                // Horizontal movement
                RotatePlayer();
                if (moveDirection.x > 0)
                {
                    lastMoveAnimIndex = 3; // RightWalk
                }
                else
                {
                    lastMoveAnimIndex = 3; // LeftWalk
                }
            }
            else
            {
                // Vertical movement
                if (moveDirection.y > 0)
                {
                    lastMoveAnimIndex = 4; // MoveUp
                }
                else
                {
                    lastMoveAnimIndex = 2; // DownWalk
                }
            }
        }
        else
        {
            // Character is not moving if cases...
            if(lastMoveAnimIndex == 3)
            {
                RotatePlayer();
                lastMoveAnimIndex = 5;
            }
            else if(lastMoveAnimIndex == 4)
            {
                lastMoveAnimIndex = 0;
            }
            else if(lastMoveAnimIndex == 2)
            {
                lastMoveAnimIndex = 1;
            }
        }

        // Update the animator with the new animation index
        animator.Play(playerMoveAnim[lastMoveAnimIndex]);
    }

    void RotatePlayer()
    {
        if (moveDirection.x == 0) return;
        int rotY = moveDirection.x < 0 ? 0 : 180;
        transform.localRotation = Quaternion.Euler(0f, rotY, 0f);

    }
}
