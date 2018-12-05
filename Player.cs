using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    HealthBar healthBar;

    bool isControlled;

    //public SpriteRenderer spriteRenderer;
    //public Animator animator;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        isControlled = true;
    }

    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        SetDirectionalInput(directionalInput);

        //TODO: use the GetAxisRaw command here instead so that the keybindings can change
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpInputUp();
        }

        controller.Move(velocity * Time.deltaTime, directionalInput);

        //animator.SetFloat("Speed", Mathf.Abs(velocity.x));

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
            //Falling here
            //animator.SetBool("IsJumping", false);
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
            //animator.SetBool("IsJumping", true);
        }
        if (controller.collisions.below)
        {
            //Problem: It only works when the platform isn't moving...
            if (controller.collisions.onThroughPlatform && directionalInput.y == -1)
            {
                velocity.y = -1;
            }
            else
            {
                velocity.y = maxJumpVelocity;
            }
        }
        //animator.SetBool("IsJumping", true);
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;

        //bool flipSprite = (spriteRenderer.flipX) ? (velocity.x > 0.01f) : (velocity.x < 0.01f);
        //if (flipSprite)
        //{
            //spriteRenderer.flipX = !spriteRenderer.flipX;
        //}
    }
}