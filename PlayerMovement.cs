using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Collider2D myCollider;
    Animator myAnimator;
    Collider2D myBody;

    //Serialized fields to change jump speed, climbing speed, and run speed
    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float speed = 0;
    [SerializeField] float climbSpeed = 5;

    float startingGravity = 2; 
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    bool isAlive = true; //Bool so that if player dies controls can be suspended

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        startingGravity = myRigidbody.gravityScale;
        myBody = GetComponent<CapsuleCollider2D>();
        myCollider.sharedMaterial.friction = 0f;
    }

    void Update()
    {
       if(!isAlive)
        {
            return;
        }

        CheckDeath();
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        //Gets the value of the movement vector when moving using WASD
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }

        //If jump button is pressed while on the ground, player moves up by the jumpspeed
        if (!isAlive)
        {
            return;
        }

        if (value.isPressed && myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }

    }

    void Run()
    {
        //Checks to see if the player has horizontal speed
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        //Creates a new vector that multiplies the moveinput by the run speed to make the player run
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        //If the player has horizontal speed the running animation plays
        if(playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        }
        
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void ClimbLadder()
    { 
        //Reduces gravity to zero and moves the player up if on the ladder
        if(myCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            myRigidbody.velocity = climbVelocity;
            myRigidbody.gravityScale = 0;
            myAnimator.SetBool("isClimbing", true);
        }

        //Once off ladder gravity resumes it's starting value and upward climb is no longer possible
        else
        {
            myRigidbody.gravityScale = startingGravity;
            myAnimator.SetBool("isClimbing", false);
        }
    }

    void FlipSprite()
    {
        //Flips sprite if horizontal speed is found to be negative (player moves to the left to make the value negative)
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    
    void CheckDeath()
    {
        //If the enemy or hazard collides with the player, kill the player
            if(myBody.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) || myCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
            {
            isAlive = false;
            myAnimator.SetTrigger("Death");//Plays death animation
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }
}

