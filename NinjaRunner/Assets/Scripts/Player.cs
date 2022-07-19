using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    public Weapon currentWeapon;
    public float speed = 1f;
    public float runSpeed = .5f;
    public float jumpHeight = 3f;
    public float gravityForce = -9.81f;
    public int maxJumps = 1;
    public float jumpCooldown = 0f;
    public Transform cam;
    public Vector3 camOffset;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    public float mouseSens = 250f;
    public float maxLookDownAngle = 80;
    public float maxLookUpAngle = 80;
    [SerializeField] private float currentSpeed;
    // private Rigidbody rb;
    private float initialAngleX;
    private bool isPaused = false;
    private int currentJumps = 0;
    private float lastJumpTime = 0;
    private bool onGround;
    private bool isRunning;
    private Transform head;
    private CharacterController characterController;
    private Vector3 currentMovement;
    [SerializeField] private Vector3 currentVelocity;
    private float xRotation;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // Set variables
        if (!cam) {
            cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        if (!currentWeapon) {
            Transform weaponTransform = cam.Find("Weapon");
            if (weaponTransform) {
                currentWeapon = weaponTransform.GetComponent<Weapon>();
            }
        }
        currentSpeed = speed;
        head = transform.Find("Head");
        characterController = GetComponent<CharacterController>();

        // Change cursor lock state
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void HandleRotation() {
        // Calculate camera rotation for mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookDownAngle, maxLookUpAngle);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        cam.localPosition = camOffset;
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement() {
        // Get movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        currentMovement = transform.right * x + transform.forward * z;

        // Move X and Z
        characterController.Move(currentMovement * currentSpeed * Time.deltaTime);

        // Get jump input
        if (Input.GetKeyDown(KeyCode.Space)) {
            bool canJump = false;
            if (onGround) {
                // On ground, reset current jumps
                // Debug.Log("Can jump because on ground");
                currentJumps = 0;
                currentWeapon.SendMessage("ChangeAvailableJumps", maxJumps);
                canJump = true;
            } else if (currentJumps < maxJumps) {
                // On air, don't reset current jumps
                canJump = true;
                // Debug.Log("Can jump because on air and has jumps available");
            } else {
                // Debug.Log("Can't jump");
            }

            // Check if jump cooldown has passed
            if (canJump && Time.time >= lastJumpTime + jumpCooldown) {
                currentJumps++;

                // Add velocity
                currentVelocity.y += Mathf.Sqrt(jumpHeight * -2 * gravityForce);
                // Debug.Log("Jumped");
                lastJumpTime = Time.time;
            }
        }

        // If grounded, set velocity to zero
        if (onGround && currentVelocity.y < 0f) {
            // Debug.Log("Set velocityY to zero");
            currentVelocity.y = -2f;
        }

        // Apply gravity
        currentVelocity.y += gravityForce * Time.deltaTime;
        characterController.Move(currentVelocity * Time.deltaTime);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        // If game is paused, don't update movement or camera
        if (isPaused) return;

        // Check if is grounded
        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
            onGround = true;
        else
            onGround = false;
        
        foreach (Collider coll in colliders)
        {
            // Debug.Log(coll.name);
        }

        HandleRotation();
        HandleMovement();

        // Running
        if (Input.GetKeyDown(KeyCode.LeftShift) && onGround) {
            if (!isRunning) {
                currentSpeed = runSpeed;
                isRunning = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            if (isRunning) {
                currentSpeed = speed;
                isRunning = false;
            }
        }
    }

    private void LateUpdate() {
        // Send message to weapon about available jumps
        if (!currentWeapon) {
            return;
        }
        currentWeapon.SendMessage("ChangeAvailableJumps", maxJumps - currentJumps);
    }

    private void FixedUpdate() {
        onGround = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckRadius, groundLayer);
        if (onGround) {
            //currentJumps = 0;
        }
    }

    private void OnApplicationFocus(bool focusStatus) {
        isPaused = !focusStatus;
        if (isPaused) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    private void OnApplicationPause(bool pauseStatus) {
        isPaused = pauseStatus;
        if (isPaused) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}