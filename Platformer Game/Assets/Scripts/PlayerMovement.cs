using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public Animator animator;

    public GameObject player;

    public float runSpeed = 25f;
    public bool hasJumpPotion = false;
    public bool hasSpeedPotion = false;
    public bool hasInvulPotion = false;
    public int potionModAmount = 0;

    public AudioClip JumpClip;
    public AudioClip CoinClip;
    public AudioClip EnemyKillClip;

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
            if(animator.GetBool("isJumping") == false)
            {
                AudioSource.PlayClipAtPoint(JumpClip, transform.position);
                jump = true;
                animator.SetBool("IsJumping", true);
            }
           
        }

        //game over when player fall out of world
        if(player.gameObject.transform.position.y <= -8)
        {
            EditorSceneManager.LoadScene("gameOver");
        }

        //game win when player gets to vending machine
        if(player.gameObject.transform.position.x >= 37 && player.gameObject.transform.position.y >= 31)
        {
            EditorSceneManager.LoadScene("winScreen");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "Enemy" && animator.GetBool("IsJumping"))
        {
            AudioSource.PlayClipAtPoint(EnemyKillClip, transform.position);
            Destroy(other.gameObject);
        }
        else
        {
            if(other.gameObject.tag == "Enemy")
            {
                Destroy(player.gameObject);
                EditorSceneManager.LoadScene("gameOver");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Coins"))
        {
            AudioSource.PlayClipAtPoint(CoinClip, transform.position);
            Destroy(other.gameObject);

        }else if (other.gameObject.CompareTag("invulStopper"))
        {
            //hasInvulPotion = false;
        }
    }


    public void onLanding()
    {
        animator.SetBool("IsJumping", false);
        jump = false;
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

        if (hasSpeedPotion)
        {
            runSpeed = 50f;
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);

        //we are currently jumping, make sure we only jump once
        if (jump)
        {
            jumpFlag = true;
        }

    }

}
