using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header(" Velocidades de movimiento")]
    public float MoveSpeed = 5f;
    public float RunSpeed = 10f;
    public float CrouchSpeed = 2.5f;
    public float JumpForce = 3f;
    public float jumpDelay = 2f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Rayo Salto")]
    public float RayJump = 1f;
    private Rigidbody rb;

    private bool isGrounded;
    private bool isCrouching = false;
    private float currentMoveSpeed;
    private bool isAttacking = false;


    public Transform cameraTransform;
    public float rotationSpeed = 3f;

    private Animator anim;

    [SerializeField] private bool IsWalk;

    private void Start()
    {
       
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    
        currentMoveSpeed = MoveSpeed;
    }

    private void Update()
    {
     
        HandleInput();
        UpdateAnimations();
        Attack();

        
    }

    private void FixedUpdate()
    {
     
        Move();
         RotateWithMouse();
    }

    private void HandleInput()
    {
    
        CheckSpeed();
        CheckJump();
        CheckCrouch();

       
        PerformMovement();
    }

    private void PerformMovement()
    {
     
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Move()
    {

        if (isAttacking)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.TransformDirection(movement).normalized;

        rb.velocity = new Vector3(movement.x * currentMoveSpeed, rb.velocity.y, movement.z * currentMoveSpeed);

        float horizontalSpeed = Mathf.Abs(rb.velocity.x);
        float forwardSpeed = Mathf.Abs(rb.velocity.z);

        IsWalk = horizontalSpeed > 0.1f || forwardSpeed > 0.1f;


    }

    private void Jump()
    { 
        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse); 
    }




    private void CheckJump()
    {
        isGrounded = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer).Length > 0;
    }

    private void CheckCrouch()
    {
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            
            isCrouching = !isCrouching;

            transform.localScale = isCrouching ? new Vector3(1f, 0.5f, 1f) : Vector3.one;
            currentMoveSpeed = isCrouching ? CrouchSpeed : MoveSpeed;
        }
    }

    private void CheckSpeed()
    {

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            currentMoveSpeed = RunSpeed;
        }
        else
        {
            currentMoveSpeed = MoveSpeed;
        }
    }

    private void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(Vector3.up * mouseX);

        cameraTransform.Rotate(Vector3.up * mouseX);
    }

    private void OnDrawGizmos()
    {
     
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void Attack()
    {

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("IsAttack");
            isAttacking = true;
            StartCoroutine(StopAttackingAfterDelay());
        }

        IEnumerator StopAttackingAfterDelay()
        {
            yield return new WaitForSeconds(2.2f); 
            isAttacking = false;
        }

    }
    private void UpdateAnimations()
    {

        anim.SetBool("IsWalk", IsWalk);
        anim.SetBool("IsGrounded", isGrounded);
    }
}


