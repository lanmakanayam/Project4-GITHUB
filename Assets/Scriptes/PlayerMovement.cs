using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    
    [Header("Coyote Time")]
    [SerializeField]private float coyotoTime;
    private float coyoteCounter;
    
    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;
    
    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;
    
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    
    private void Awake()
    {
        //Grab refrencess for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
       
        //Flip player when moving left or right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKey(KeyCode.Space) && isGrounded())
            Jump();

        //Set animator paramaters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        
        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
            
        //Adjustble jump height
        if (Input.GetKeyDown(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
            
        if(onWall())
        {
            body.gravityScale =0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            
            if (isGrounded())
            {
                coyoteCounter = coyotoTime;
                jumpCounter = extraJumps;
            }
            else
                coyoteCounter -= Time.deltaTime;
            
        }
        
        //Wall jump logic
        if(wallJumpCooldown < 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed,body.velocity.y);
            
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;
                
            if (Input.GetKey(KeyCode.Space))
              Jump();

        }  
        else
           wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {   
        if(coyoteCounter <=0 && !onWall() && jumpCounter <= 0) return;
        
        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity =new Vector2(body.velocity.x, jumpPower);
            else    
            {
                if (coyoteCounter > 0)
                    body.velocity =new Vector2(body.velocity.x, jumpPower);
               else
                {
                    if (coyoteCounter > 0)
                    {
                        body.velocity =new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }     
            
            coyoteCounter = 0;
        }  
        
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            
            
        }
        else if (onWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)* 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)* 3, 6);
                
            wallJumpCooldown = 0;
            
        }
    }   
    private void WallJump()
    {
    
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}