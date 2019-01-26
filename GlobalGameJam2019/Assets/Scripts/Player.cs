using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.PlayerLoop;

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
    
    [Header("Weapon values")]
    public GameObject weaponContainer;
    public GameObject weaponCenterpoint;
    public float weaponDistanceFromHolder = 10.0f;
    public float weaponRotationSpeed = 10.0f;
    public float pickupRange = 10.0f;
    public string pickupTag = "Weapon";
    private bool isHoldingWeapon = false;

    private float gravity;
    private float jumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;


    private Controller2D controller;

    private void Start()
    {
        controller = GetComponent<Controller2D>();

        //Setting up movement values
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    private void FixedUpdate()
    {
       UpdateMovement();
       UpdateWeaponDirection();

        if (Input.GetButtonDown("PickUp"+playerID))
        {
            if(isHoldingWeapon)
                DropWeapon();
            else
                FindClosestWeaponInRange();
        }
    }

    private void UpdateMovement()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"+playerID), Input.GetAxis("Vertical"+playerID));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGround : accelerationTimeAir);

        bool wallSliding = false;
        if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below &&
            velocity.y < 0)
        {
            wallSliding = true;

            if(velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if(timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if(input.x != wallDirX && input.x != 0)
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
        if(controller.collisions.above)
        {
            velocity.y = 0;
        }

        //Ground check
        if(controller.collisions.below)
        {
            velocity.y = 0;
            jumpsRemaining = numberOfJumps;
        }


        if(Input.GetButton("Jump"+playerID))
        {
            bool walljumped = false;

            if(wallSliding)
            {
                walljumped = true;
                if(wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if(input.x == 0)
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
            if(controller.collisions.below || ((Time.time - lastJumpTime > jumpDelay) && (jumpsRemaining > 0)))
            {
                lastJumpTime = Time.time;
                float deacrease = (numberOfJumps - jumpsRemaining) * extraJumpDecrease;
                velocity.y = jumpVelocity - deacrease;
                if(walljumped && !wallJumpsCountAsDoubleJumps)
                    jumpsRemaining++;

                jumpsRemaining--;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateWeaponDirection()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("WeaponHorizontal" + playerID), Input.GetAxisRaw("WeaponVertical" + playerID));
        if (input.x > 0.1f || input.y > 0.1f || input.x < -0.1f || input.y < -0.1f)
        {
            float TargetAngle = (Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg * -1.0f) + 90.0f;
            float CurrentAngle = weaponCenterpoint.transform.rotation.eulerAngles.z;

            float LerpResult = Mathf.LerpAngle(CurrentAngle, TargetAngle, Time.deltaTime * weaponRotationSpeed);

            Vector3 weaponRotation = weaponCenterpoint.transform.eulerAngles;
            weaponRotation.z = LerpResult;
            weaponCenterpoint.transform.eulerAngles = weaponRotation;
        }

    }

    private void FindClosestWeaponInRange()
    {
        Collider2D[] foundObjects = Physics2D.OverlapCircleAll(transform.position, pickupRange);

        for (int i = 0; i < foundObjects.Length; i++)
        {
            if (foundObjects[i].gameObject.tag == pickupTag)
            {
                PickupWeapon(foundObjects[i].gameObject);
                break;
            }
        }
    }

    private void PickupWeapon(GameObject newWeapon)
    {
        isHoldingWeapon = true;
        newWeapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        newWeapon.transform.parent = weaponContainer.transform;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localEulerAngles = newWeapon.transform.position - transform.position;
        newWeapon.GetComponent<WeaponBase>().SetCombatCollidersActive(true);
        newWeapon.GetComponent<WeaponBase>().SetPhysicalCollidersActive(false);
    }

    private void DropWeapon()
    {
        isHoldingWeapon = false;
        weaponContainer.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        weaponContainer.GetComponentInChildren<WeaponBase>().SetCombatCollidersActive(false);
        weaponContainer.GetComponentInChildren<WeaponBase>().SetPhysicalCollidersActive(true);
        weaponContainer.GetComponentInChildren<Rigidbody2D>().simulated = true;
        weaponContainer.transform.DetachChildren();
    }
}