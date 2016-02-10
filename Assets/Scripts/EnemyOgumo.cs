using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2DEnemy))]

public class EnemyOgumo : MonoBehaviour {
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.4f;
    public int direction;
    public float moveSpeed;
    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;    
    bool facingRight;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    AudioSource audio;

    //Animator animator;
    bool grounded = false;

    Controller2DEnemy controller;

    void Start()
    {
        controller = GetComponent<Controller2DEnemy>();
        //animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        print("Ogumo - Gravity: " + gravity + " Jump Velocity: " + maxJumpVelocity);
    }

    void FixedUpdate()
    {
        /*animator.SetBool("Grounded", grounded);
        animator.SetFloat("vSpeed", velocity.y);
        animator.SetFloat("hSpeed", Mathf.Abs(velocity.x));
        animator.SetBool("inputMove", Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0);*/
    }

    void Update()
    {
        if (controller.collisions.right)
            direction = -1;
        else if (controller.collisions.left)
            direction = 1;

        int wallDirectionX = (controller.collisions.left) ? -1 : 1;

        if (direction > 0 && !facingRight)
            FlipSprite();
        else if (direction < 0 && facingRight)
            FlipSprite();

        grounded = controller.collisions.below;

        float targetVelocityX = direction * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne
            );
        
        if (Input.GetAxis("Jump") <= 0) {
            if (velocity.y > minJumpVelocity)
                velocity.y = minJumpVelocity;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }

    public struct Direction {
        public bool up, down, left, right;
        public bool ceiling, floor, wallLeft, wallRight;
    }

    void FlipSprite()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}