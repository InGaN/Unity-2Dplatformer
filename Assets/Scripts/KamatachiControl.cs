using UnityEngine;
using System.Collections;

public class KamatachiControl : MonoBehaviour {
    public float maxSpeed = 10f;
    public float jumpForce = 700f;

    public bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    private bool facingLeft = true;    

    private Rigidbody2D rb2d;
    private Animator animator;


    void Awake()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    void Start () {
	    
	}
	
	// Fixedupdate for physics so we dont need Delta Time
	void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        animator.SetBool("Grounded", grounded);

        animator.SetFloat("vSpeed", rb2d.velocity.y);

        float move = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(move));

        rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);

        if (move > 0 && facingLeft)
            Flip();
        else if (move < 0 && !facingLeft)
            Flip();
    }

    void Update()
    {
        //if put in FixedUpdate, pressing spacebar may be missed
        if (grounded && Input.GetAxis("Jump") > 0)
        {
            animator.SetBool("Grounded", false);
            rb2d.AddForce(new Vector2(0, jumpForce));
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
