using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float groundDrag = 3f;
    private bool isSprinting;

    [HideInInspector] public float movementX;
    [HideInInspector] public float movementY;
    private Vector3 moveDirection;
    [SerializeField] private Transform orientation;

    private MovementState movementState;
    private enum MovementState
    {
        walking,
        sprinting,
        air
    }

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    private float jumpCooldown;
    private float airMultiplier = 1f;
    private bool readyToJump = true;
    [SerializeField] private Transform groundCheck;
    private float groundDistance = 0.4f;
    private bool grounded;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Score System")]
    [HideInInspector] public int score = 0;
    [HideInInspector] public int totalScore;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float walkAnimationSpeed = 1.0f;
    [SerializeField] private float sprintAnimationSpeed = 5.0f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        totalScore = GameObject.FindGameObjectsWithTag("PickUp").Length;
        score = 0;
        UpdateScoreText();
    }

    private void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        SpeedControl();
        StateHandler();

        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (transform.position.y < -10)
        {
            transform.position = new Vector3(7, 1, -2);
        }
    }

    private void FixedUpdate()
    {
        moveDirection = orientation.forward * movementY + orientation.right * movementX;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }


    public void OnJump(InputValue movementValue)
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void OnSprint(InputValue movementValue)
    {
        isSprinting = movementValue.isPressed;
    }

    private void StateHandler()
    {
        if (grounded && isSprinting)
        {
            movementState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            animator.speed = sprintAnimationSpeed;
        }
        else if (grounded)
        {
            movementState = MovementState.walking;
            moveSpeed = walkSpeed;
            animator.speed = walkAnimationSpeed;
        }
        else
        {
            movementState = MovementState.air;
            animator.speed = walkAnimationSpeed;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = score + " / " + totalScore;
    }
}