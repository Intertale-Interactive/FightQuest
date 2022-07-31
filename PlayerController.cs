using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("UI")]
    public Joystick joy;
    public Text dashText;

    // [Header("Others scripts reference")]
    // public TimeManager timeManager;

    [Header("Transform")]
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    [Header("Player")]
    public Rigidbody2D rb;
    public Animator anim;
    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float dashTimer = 4;
    public float wallJumpForceX;
    public float wallJumpForceY;
    public ParticleSystem dust;

    public float wallJumpForce;

    [Header("Additional Settings")]
    public float movementInputDirection;
    // public float jumpTimer;
    public float turnTimer;
    // public float wallJumpTimer;

    [Header("Layer Mask")]
    public LayerMask whatIsGround;

    [Header("Private int xD")]
    public int amountOfJumps = 1;
    public int amountOfJumpsLeft;
    public int facingDirection = 1;
    public int lastWallJumpDirection;

    [Header("Bool")]
    public bool isFacingRight = true;
    public bool isWalking;
    public bool isGrounded;
    public bool isTouchingWall;
    public bool isWallSliding;
    public bool canNormalJump;
    public bool canWallJump = false;
    public bool isJumping;
    public bool isDashinng;
    // public bool isAttemptingToJump;
    public bool checkJumpMultiplier;
    public bool canMove;
    public bool canFlip;
    public bool hasWallJumped;
    public bool isTouchingLedge;
    public bool canClimbLedge = false;
    public bool ledgeDetected;
    public bool canDash = false;
    public bool uHasDash = false;
    public bool canSlide;

    [Header("Vector 2")]
    public Vector2 ledgePosBot;
    public Vector2 ledgePos1;
    public Vector2 ledgePos2;

    [Header("Floats")]
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    // public float jumpTimerSet = 0.15f;
    // public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;
    public float wallSlideTimer = 0.5f;

    [Header("GameObjects")]
    public GameObject dashPanel;
    public GameObject detectColliderEnemy;
    public GameObject triggerForEnemy;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;

        canDash = true;
        canSlide = true;
        isJumping = false;
        isDashinng = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckLedgeClimb();
        CheckIfCanSlide();

        // BY ME //
        CheckCanDash();
        DetectEnemy();

        dashText.text = dashTimer.ToString();

        // Debug.Log("facingDirection " + facingDirection + " " + movementInputDirection);
        // Debug.Log("uHasDash" + " " + uHasDash);
        // Debug.Log(isWallSliding);
        Debug.Log("Facing Direction = " + facingDirection);

        // Debug.Log(dashTimer);

        // ------------------------------------- FOR MY PC TEST ------------------------------------- //
        // WALL JUMP FOR PC //
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (canWallJump && facingDirection == 1) //  && amountOfJumpsLeft > 0 CA AUSSI !!
            {
                // timeManager.manageTime();
                transform.position += new Vector3(-2.2f, 2.2f, 0);
                facingDirection *= -1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
                // amountOfJumpsLeft--; // A ajouter après
            } else if (facingDirection == -1)
            {
                // Debug.Log("facing direction == -1");
                transform.position += new Vector3(2.2f, 2.2f, 0);
                facingDirection *= -1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }

        // JUMP FOR PC //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canNormalJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                amountOfJumpsLeft--;
                // jumpTimer = 0;
                // isAttemptingToJump = false;
                checkJumpMultiplier = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Vector2 forceToAddForPc = new Vector2(30 * -1, 23.5f);
            rb.AddForce(forceToAddForPc, ForceMode2D.Impulse);
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector2 forceToAddForPc1 = new Vector2(30 * 1, 23.5f);
            rb.AddForce(forceToAddForPc1, ForceMode2D.Impulse);
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        // Slide @t floor
        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     rb.velocity = new Vector2(20, rb.velocity.y);
        // }            
    }

    public void DetectEnemy()
    {
        RaycastHit2D hit = Physics2D.Raycast(detectColliderEnemy.transform.position, transform.right, .02f);

        Debug.DrawRay(detectColliderEnemy.transform.position, transform.right * .02f, Color.red);

        if (hit.collider)
        {
            if (hit.collider.isTrigger && (triggerForEnemy.tag == "detectEnemyCol")){
                Debug.Log("Dans la zone de collider !");               
            }
        }  
    }

    // IEnumerator slideCor()
    // {
        
    // }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    public void CheckIfCanSlide()
    {
        if (isWallSliding || isWallSliding || !isGrounded)
        {
            canSlide = false;
        }
        else 
        {
            canSlide = true;
        }
    }

    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if (isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;

            anim.SetBool("canClimbLedge", canClimbLedge);
        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimbLedge", canClimbLedge);
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if(isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall && !isGrounded)
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }

        if(amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
      
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if(Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    public void CheckInput()
    {
        movementInputDirection = joy.Horizontal;

        float verticalMove = joy.Vertical;

        if ((movementInputDirection > 0 || movementInputDirection < 0) && isTouchingWall) //-----------------------BY ME-----------------------//
        {
            if (!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                // turnTimer = turnTimerSet;
            }
        }

        if (!canMove)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }
    }
    public void ButtonJump()
    {
        if(!isGrounded && isTouchingWall)
        {
            WallJump();
        }
        else if (isGrounded)
        {
            NormalJump();
        }
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && rb.velocity.y < 0 && !canClimbLedge && wallSlideTimer <= 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
            wallSlideTimer -= Time.deltaTime;
        }
    }

    private void CheckCanDash()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            dashPanel.SetActive(true);
        }

        if (dashTimer <= 0)
        {
            dashPanel.SetActive(false);
            // Debug.Log("DESACTIVATION");
        }

        //Si tu as dash, on montre le dashTimer pendant 3s 
        if (uHasDash)
        {
            StartCoroutine(uHasDashCor());
            // Debug.Log("ACTIVATION");
        }

        if (isGrounded)
        {
            canDash = false;
        } else
        {
            canDash = true;                                // POTENTIEL BUG //
        }
    }

    IEnumerator uHasDashCor()
    {
        dashPanel.SetActive(true);
        yield return new WaitForSeconds(3);
        uHasDash = false;
        dashPanel.SetActive(false);
    }

    public void DashInAir()
    {
        if (facingDirection == 1 && canDash && dashTimer <= 0)
        {
            uHasDash = true;
            dashPanel.SetActive(false);
            rb.velocity = new Vector2((rb.velocity.x + 25), rb.velocity.y);
            dashTimer = 3;
            isDashinng = true;
        } else if (facingDirection == -1 && canDash && dashTimer <= 0)
        {
            uHasDash = true;
            dashPanel.SetActive(false);
            rb.velocity = new Vector2((rb.velocity.x - 25), rb.velocity.y);
            dashTimer = 3;
            isDashinng = true;
        }
        // } else if (movementInputDirection >= 0 && canDash && facingDirection == 1)
        // {
        //     uHasDash = true;
        //     dashPanel.SetActive(false);
        //     rb.velocity = new Vector2((rb.velocity.x + 50), rb.velocity.y);
        //     // transform.position += new Vector3(5, 0, 0);
        //     dashTimer = 3;
        //     Debug.Log("IMHERERIGHT");
        // } else if (movementInputDirection <= 0 && canDash && facingDirection == -1)
        // {
        //     uHasDash = true;
        //     dashPanel.SetActive(false);
        //     rb.velocity = new Vector2((rb.velocity.x - 50), rb.velocity.y);
        //     // transform.position += new Vector3(-5, 0, 0);
        //     dashTimer = 3;
        //     Debug.Log("IMHERELEFT");
        // }
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            // jumpTimer = 0;
            // isAttemptingToJump = false;
            checkJumpMultiplier = true;
            isJumping = true;
        }
    }

    private void WallJump()
    {
        Debug.Log("HHHH");
        if (canWallJump && facingDirection == 1) //  && amountOfJumpsLeft > 0 ?
            {
                Vector2 forceToAdd = new Vector2(15 * -1, 23.5f);
                rb.AddForce(forceToAdd, ForceMode2D.Impulse);
                facingDirection *= -1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
                // timeManager.manageTime();
            } else if (canWallJump && facingDirection == -1)
            {
                Vector2 forceToAdd = new Vector2(15 * 1, 23.5f);
                rb.AddForce(forceToAdd, ForceMode2D.Impulse);
                facingDirection *= -1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
            if (isTouchingWall && movementInputDirection != 0)
            {
                Debug.Log("HHHHHHHHHHHHHHHHHH");
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            }
    }

    private void ApplyMovement()
    {

        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else 
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            Dust();
        }
        

        if (isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);                // A FAIRE POUR L'ESCALADE //
            }
        }
    }

    private void Flip()
    {
        if (!isWallSliding)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    public void Dust()
    {
        dust.Play();
    }
}