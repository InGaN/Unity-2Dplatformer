using UnityEngine;
using System.Collections;
using System;

public class PlayerControl : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;    

    public float maxSpeed = 10f;        
    public bool grounded = false;
    public Transform groundCheck;    
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public float jumpForce = 700f;
    public bool doubleJump = false;

    private Rigidbody2D rb2d;
    private Animator animator;
    
	void Awake ()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
	}
	
	void Update ()
    {
        if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Grounded", false);
            rb2d.AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !grounded)
                doubleJump = true;
        }
	}

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        animator.SetBool("Grounded", grounded);

        float move = Input.GetAxis("Horizontal"); // left = -1, right = 1

        if (grounded)
            doubleJump = false;

        animator.SetFloat("Speed", Mathf.Abs(move)); // Math.Abs will always return positive
        
        animator.SetFloat("vSpeed", rb2d.velocity.y);

        rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);

        if (move > 0 && !facingRight)
            FlipSprite();
        else if (move < 0 && facingRight)
            FlipSprite();      
               
    }

    void FlipSprite()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
