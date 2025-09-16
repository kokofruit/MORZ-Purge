// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Input manager for the main player object

using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor.Callbacks;

public class Player_Controller : MonoBehaviour
{
    //////////////////// Public Variables /////////////////////
    // Static reference to this player
    public static Player_Controller instance;
    // Player horizontal look sensitivity
    public float lookSensX = 0.1f;
    // Player vertical look sensitivity
    public float lookSensY = 0.1f;
    // Player's starting health
    public float _health { get; private set; } = 100;
    // Head object that contains the first person camera
    public Transform head;
    
    //////////////////// Private Variables /////////////////////
    // Default movement speed
    [SerializeField] private float _walkSpeed = 1.5f;
    // Default speed multiplier for when the player is running
    [SerializeField] private float _runSpeedMultiplier = 2;
    // Amount of force added to the player when jumping
    [SerializeField] private float _jumpForce = 5;
    // Player rigidbody
    private Rigidbody _rb;
    // Store the players height for runtime calculations
    private float _playerHeight;
    // Raw input vector from move function
    private Vector2 _movementVector;
    // Raw input vector from look function
    private Vector2 _lookVector;
    // Keeps track of whether the player is currently sprinting or not
    private bool _isSprinting;
    // Keeps track of whether the player is on the ground or not
    private bool _isGrounded;
    // Horizontal look velocity with sensitivity applied
    private float _lookX;
    // Vertical look velocity with sensitivity applied
    private float _lookY;
    // Maximum allowed player health
    private int MAX_HEALTH = 100;
    // Maximum allowed player speed
    private int MAX_SPEED = 10;

    ///////////////////////////////// Monobehvaior Methods ////////////////////////////////
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find unnassigned runtime objects and variables
        head = transform.Find("Head");
        _rb = GetComponent<Rigidbody>();
        _playerHeight = transform.localScale.y * 2;

        // Lock the cursor to the center of the screen during gameplay
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ///////////////// Look update /////////////////
        // Apply the players look sensitivity preferences to the raw input vectors
        _lookX += _lookVector.x * lookSensX;
        _lookY += _lookVector.y * lookSensY;

        // Clamp the players looking range to straight up and down
        _lookY = Mathf.Clamp(_lookY, -90, 90);

        // Change player's horizontal rotation to reflect player input
        transform.rotation = Quaternion.Euler(0, _lookX, 0);
        // Change head's vertical rotation to reflect player input
        head.localRotation = Quaternion.Euler(-_lookY, 0, 0);

        ///////////////// Move update /////////////////
        // If the player is on the ground
        if (_isGrounded)
        {
            // Set the players speed depending on whether they are sprinting or not
            float _speed = _isSprinting ? _walkSpeed * _runSpeedMultiplier : _walkSpeed;

            // Change the raw input into player velocity by adding player speed
            Vector3 velocity = _movementVector * _speed;
            // Get the local vector to reflect changes in player rotation
            Vector3 localVelocity = transform.TransformDirection(new Vector3(velocity.x, _rb.linearVelocity.y, velocity.y));
            // Set the rigidbody's velocity to the new local velocity
            _rb.linearVelocity = localVelocity;
        }
    }

    // Called every time the player continues to collide with another object
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ///////////////// Grounded Check /////////////////
            RaycastHit hit;
            // Get the distance to the floor below the player
            float distance = _playerHeight / 1.8f;
            // Raycast downwards 
            Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, distance);
            // Check if the player is standing on something
            if (hit.collider != null)
            {
                _isGrounded = true;
            }
        }
    }

    // Called when the player stops contacting another object
    void OnCollisionExit(Collision collision)
    {
        // Check if the player has left the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

    // Release the cursor when the player dies
    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }


    ///////////////////////////////// Player Specific Methods ////////////////////////////////

    // Takes a signed float as the desired change in the player's health
    public void AddHealth(float amount)
    {
        _health += amount;
        if (_health > MAX_HEALTH)
        {
            _health = MAX_HEALTH;
        }
    }

    public void SubtractHealth(float amount)
    {
        _health -= amount;

        if (_health < 0)
        {
            // DIE!!!!!
            Debug.Log("Player has died");
        }
    }

    ///////////////////////////////// Input  Management ////////////////////////////////

    // Movement input from the input manager
    public void OnMove(InputValue input)
    {
        _movementVector = input.Get<Vector2>();
    }

    // Look input from the input manager
    public void OnLook(InputValue input)
    {
        _lookVector = input.Get<Vector2>();
    }

    // Sprint input from the input manager
    public void OnSprint(InputValue input)
    {
        float value = input.Get<float>();
        // If the button is pressed
        if (value == 1)
            _isSprinting = true;
        // If the button is released
        else
            _isSprinting = false;
    }

    // Called when player presses the jump button
    public void OnJump()
    {
        // Checks if the player is on the ground
        if (_isGrounded)
        {
            // Add an sudden upwards force
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
}
