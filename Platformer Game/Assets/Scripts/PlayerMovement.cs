using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public Animator animator;

    private bool fallingState = false;

    public float runSpeed = 25f;
    public bool hasJumpPotion = false;
    public bool hasSpeedPotion = false;
    public bool hasInvulPotion = false;
    public int potionModAmount = 0;

    float horizontalMove = 0f;
    bool jumpFlag = false;
    bool jump = false;

    private float potionTimeMax = 10f;
    private float potionTimeCur = 0f;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //set animation to recognize that our player is running with a speed
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        //have they jumped? was jump triggered? only jump once
        if (jumpFlag)
        {
            animator.SetBool("IsJumping", true);
            jumpFlag = false;
        }

        //checking "jump keys" (spacebar) to see if it was pressed down
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
            fallingState = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasInvulPotion)
        {
            if (other.gameObject.tag == "Enemy")
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void onLanding()
    {
        animator.SetBool("IsJumping", false);
        jump = false;
        fallingState = true;
    }


    void FixedUpdate()
    {
        if(hasJumpPotion && potionTimeCur < potionTimeMax)
        {
            controller.m_JumpForceMod = potionModAmount;
            potionTimeCur += Time.fixedDeltaTime;
        }
        else
        {
            potionTimeCur = 0f;
            controller.m_JumpForceMod = 0;
            hasJumpPotion = false;
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);

        //we are currently jumping, make sure we only jump once
        if (jump)
        {
            jumpFlag = true;
        }

    }

}
