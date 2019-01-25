using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public int playerID;

    [Header("Movement values")]
    public float accelerationTimeAir = .2f;
    public float accelerationTimeGround = .1f;
    public float moveSpeed = 6;

    [Header("Jump values")]
    public float timeToJumpApex = .4f;
    public float jumpHeight = 4;
    public int numberOfJumps = 2;
    public float extraJumpDecrease;
    public float jumpDelay = 0.1f;
    public bool wallJumpsCountAsDoubleJumps;
    [HideInInspector] public int jumpsRemaining;
    [HideInInspector] public float lastJumpTime;

    [Header("Wall controlles")]
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    private float timeToWallUnstick;
    
    private float gravity;
    private float jumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private Controller2D controller;

    private void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    private void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGround : accelerationTimeAir);

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below &&
            velocity.y < 0)
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

                if (input.x != wallDirX && input.x != 0)
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

        //Head check
        if (controller.collisions.above)
        {
            velocity.y = 0;
        }

        //Ground check
        if (controller.collisions.below)
        {
            velocity.y = 0;
            jumpsRemaining = numberOfJumps;
        }


        if (Input.GetButton("Jump"))
        {
            bool walljumped = false;

            if (wallSliding)
            {
                walljumped = true;
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below || ((Time.time - lastJumpTime > jumpDelay) && (jumpsRemaining > 0)))
            {
                lastJumpTime = Time.time;
                float deacrease = (numberOfJumps - jumpsRemaining) * extraJumpDecrease;
                velocity.y = jumpVelocity - deacrease;
                if (walljumped && !wallJumpsCountAsDoubleJumps)
                    jumpsRemaining++;

                jumpsRemaining--;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}