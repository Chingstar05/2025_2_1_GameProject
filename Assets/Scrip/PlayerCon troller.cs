using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("점프 설정")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float landingDuration = 0.3f;                //착지 후 착지 지속 시간

    [Header("공격 설정")]
    public float attackDuration = 0.8f;                                             //공격 지속 시간
    public bool canMoveWhileAttacking = false;                                      

    [Header("컴포넌트")]
    public Animator animator;

    public CharacterController controller;
    private Camera playerCamera;

    //현재 상태
    public float currentSpeed;
    private bool isAttkacking = false;
    private bool isLanding = false;
    private float landingTimer;

    private Vector3 velcoity;
    private bool isGrounded;
    private bool wasGrounded;
    private float attackTimer;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleAttack();
        HandleJump();
        HandleAttack();
        UpdateAnimator();

    }
    void CheckGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if (!isGrounded && wasGrounded)
        {
            Debug.Log("떨어지기 시작");
        }

        if(isGrounded && velcoity.y < 0)
        {
            velcoity.y = -2f;

            if(!wasGrounded && animator != null)
            {
                //animator.SetTrigger("landTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("착지");
            }
        }

    }

    void HandleLanding()
    {
        if(isLanding)
        {
            landingTimer -= Time.deltaTime;
            if (landingTimer <= 0)
            {
                isLanding = false;
            }
        }
    }

    void HandleAttack()
    {
        if(isAttkacking)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0 )
            {
                isAttkacking = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttkacking)
        {
            isAttkacking = true;
            attackTimer = attackDuration;

            if(animator != null)
            {
                animator.SetTrigger("attackTrugger");
            }
        }
    }
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velcoity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if(animator != null)
            {
                animator.SetTrigger("jumpTrigger");
            }
        }
        if(!isGrounded)
        {
            velcoity.y += gravity * Time.deltaTime;
        }
        controller.Move(velcoity * Time.deltaTime);
    }
    void HandleMovement()
    {
        if((isAttkacking && !canMoveWhileAttacking) || isLanding)
        {
            currentSpeed = 0;
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float verical = Input.GetAxis("Vertical");

        if (horizontal != 0 || verical != 0)
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * verical + cameraRight * horizontal;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            Quaternion targetRotion = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotion, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;
        }

    

}
    void UpdateAnimator()
    {
        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
        animator.SetFloat("speed", animatorSpeed);
        animator.SetBool("isGrounded",isGrounded);

        bool isFalling = !isGrounded && velcoity.y < -0.1f;
        animator.SetBool("isFalling",isFalling);
        animator.SetBool("isLanding",isLanding);
    }

}
