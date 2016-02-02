using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller2D))]

public class Player : MonoBehaviour {
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;
    float moveSpeed = 6;

    public Vector2 wallJumbClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = 0.25f;
    float timeToWallUnstick; 

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    void Start () {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        print("Gravity: " + gravity + " Jump Velocity: " + maxJumpVelocity);
	}

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirectionX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne
            );

        bool wallSliding = false;
        if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
            wallSliding = true;

            if(velocity.y < - wallSlideSpeedMax) {
                velocity.y = -wallSlideSpeedMax;
            }

            if(timeToWallUnstick > 0) {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if(input.x != wallDirectionX && input.x != 0) {
                    timeToWallUnstick -= Time.deltaTime;
                }                
                else {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else {
                timeToWallUnstick = wallStickTime;
            }
        }
        
        if (Input.GetAxis("Jump") > 0) { 
            if (wallSliding) {
                if(wallDirectionX == input.x) {
                    velocity.x = -wallDirectionX * wallJumbClimb.x;
                    velocity.y = wallJumbClimb.y;
                }
                else if (input.x == 0) {
                    velocity.x = -wallDirectionX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else {
                    velocity.x = -wallDirectionX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if(controller.collisions.below) { // if player is standing on surface
                velocity.y = maxJumpVelocity;
            }            
        }
        if(Input.GetAxis("Jump") <= 0) {
            if(velocity.y > minJumpVelocity)
                velocity.y = minJumpVelocity;
        }
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime, input);
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }
}
