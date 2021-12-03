using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public bool isMoving = true;
    public bool lightOn = true;

    [Header("PLAYER SETTINGS")]
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float rotateSpeed = 12f;
    [SerializeField] float jumpSpeed;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode runKey;
    [SerializeField] private KeyCode lightToggle;

    [SerializeField] bool lockCursor = true;
    [SerializeField] private Transform cameraTransform;

    CharacterController controller = null;
    private float originalStepOffset;
    [SerializeField] private float ySpeed;
    Vector3 velocity;

    Animator animator;
    public GameObject playerLightGO;

    void Start()
    {
        instance = this;
        animator = GetComponentInChildren<Animator>();

        controller = GetComponent<CharacterController>();
        originalStepOffset = controller.stepOffset;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            UpdateMovement();
            UpdateStat();
        }
    }

    void UpdateStat()
    {
        if (Input.GetKeyDown(lightToggle))
        {
            lightOn = !lightOn;
            playerLightGO.SetActive(lightOn);
        }
    }

    void UpdateMovement()
    {
        bool running = Input.GetKey(runKey);
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        moveDir = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDir;
        moveDir.Normalize();
        ySpeed += Physics.gravity.y / 3 * Time.deltaTime;

        if (moveDir != Vector3.zero && controller.isGrounded)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }

        JumpInput();

        float currentSpeed = (running) ? runSpeed : walkSpeed; //Sets the speed when moving

        velocity = moveDir * currentSpeed;
        velocity.y = ySpeed;
        controller.Move(velocity * Time.deltaTime);

        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }
    }

    void JumpInput()
    {
        if (controller.isGrounded)
        {
            animator.SetBool("jump", false);
            controller.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Input.GetKeyDown(jumpKey))
            {
                animator.SetBool("jump", true);
                animator.SetBool("run", false);
                ySpeed = jumpSpeed;
            }
        }
        else
        {
            controller.stepOffset = 0;
        }
    }
}
