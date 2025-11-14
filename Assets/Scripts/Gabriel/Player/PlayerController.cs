// Main Contributor: Gabriel Heiser
// Other Contributors: Domenic, Phil, Vin (temporary upgrades)
// Reviewer: 
// Description: Input manager for the main player object

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //////////////////// Public Variables /////////////////////
    // Static instance of the player for other scripts to reference
    public static PlayerController instance;
    // Head object that contains the first person camera
    public Transform head;
    public Collider movementLimiter;

    //////////////////// Private Variables /////////////////////
    [Header("Player Variables")]
    ///     // Player horizontal look sensitivity
    [SerializeField] private float _lookSensX = 0.1f;
    // Player vertical look sensitivity
    [SerializeField] private float _lookSensY = 0.1f;
    // Player's starting health
    [SerializeField] private float _health = 100;
    // Default movement speed
    [SerializeField] private float _walkSpeed = 1.5f;
    // Default speed multiplier for when the player is running
    [SerializeField] private float _runSpeedMultiplier = 2;
    // Stimulant Upgrade Multiplier
    [SerializeField] private float stimMultiplier = 2;
    // Amount of force added to the player when jumping
    [SerializeField] private float _jumpForce = 5;
    // Maximum distance within which the player can interact with other objects
    [SerializeField] private float _interactDistance = 5;
    // Minimum speed the player can travel in the air
    [SerializeField] private float _minAirSpeed = 3;
    // Amount of acceleration player input has in the air
    [SerializeField] private float _midairAcceleration = 10;
    // Maximum slope the player can walk on
    [SerializeField] private float maxSlopeAngle = 45;
    
    
    // Player Heads Up Display
    private HUDController HUD;
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
    // Maximum speed the player can travel in the air
    private float _maxAirSpeed;
    // Horizontal look velocity with sensitivity applied
    private float _lookX;
    // Vertical look velocity with sensitivity applied
    private float _lookY;
    // Maximum allowed player health
    private int MAX_HEALTH = 100;
    // Upgrade bools
    private bool armorUpActivated = false;
    private bool stimulantUpActivated = false;

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
        // Tell the game manager to load level information
        GameManager.instance.StartLevel();

        // Find unnassigned runtime objects and variables
        head = transform.Find("Head");
        _rb = GetComponent<Rigidbody>();
        _playerHeight = transform.localScale.y * 2;
        HUD = HUDController.instance;

        // Lock the cursor to the center of the screen during gameplay
        Cursor.lockState = CursorLockMode.Locked;

        // Set the players health to full
        HUD.SetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        ///////////////// Look update /////////////////
        // Apply the players look sensitivity preferences to the raw input vectors
        _lookX += _lookVector.x * _lookSensX;
        _lookY += _lookVector.y * _lookSensY;

        // Clamp the players looking range to straight up and down
        _lookY = Mathf.Clamp(_lookY, -90, 90);

        // Change player's horizontal rotation to reflect player input
        transform.rotation = Quaternion.Euler(0, _lookX, 0);
        // Change head's vertical rotation to reflect player input
        head.localRotation = Quaternion.Euler(-_lookY, 0, 0);

        ///////////////// Move update /////////////////
        // If the player is on the ground
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _playerHeight/1.8f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            if (slopeAngle < maxSlopeAngle) {
                // Set the players speed depending on whether they are sprinting or not
                float _speed = _isSprinting ? _walkSpeed * _runSpeedMultiplier : _walkSpeed;

                HUD.AnimateWeapon(_rb.linearVelocity.magnitude);

                // Check if stimulant is activated
                if (stimulantUpActivated)
                {
                    // Increase speed by multiplying by the multiplier
                    _speed *= stimMultiplier;
                } // Speed goes back to normal once stimulantUpActivated is false

                // Change the raw input into player velocity by adding player speed
                Vector3 velocity = _movementVector * _speed;
                // Get the local vector to reflect changes in player rotation
                Vector3 localVelocity = transform.TransformDirection(new Vector3(velocity.x, _rb.linearVelocity.y, velocity.y));
                // Get the normal of the ground we are standing on
                Vector3 groundNormal = hit.normal;

                // Step 2: Project velocity onto the slope plane
                Vector3 slopeDirection = Vector3.ProjectOnPlane(localVelocity, groundNormal);

                _rb.linearVelocity = slopeDirection;
            }
        } else {
            if (_rb.linearVelocity.magnitude < _maxAirSpeed || _rb.linearVelocity.magnitude < _minAirSpeed) {
                Vector3 velocity = _movementVector * _midairAcceleration;
                Vector3 localVelocity = transform.TransformDirection(new Vector3(velocity.x, _rb.linearVelocity.y, velocity.y));
                _rb.AddForce(localVelocity);
            }
        }
    }

    // Called when the player collides with a trigger object
    void OnTriggerEnter(Collider collider)
    {
        // If the object is a pickup
        if (collider.gameObject.CompareTag("Pickup"))
        {
            // Get out the pickup object's pickup controller script
            collider.gameObject.TryGetComponent(out PickupController pickup);

            pickup.PickupObject();
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

        // Update health bar with new health amount
        HUD.DisplayHealth(_health);
    }

    public void SubtractHealth(float amount)
    {
        // Check to make sure invincibility armor isn't active
        if (!armorUpActivated)
        {
            _health -= amount;

            if (_health < 0)
            {
                GameManager.instance.PlayerDied();
                return;
            }
            // Update health bar with new health amount
            HUD.DisplayHealth(_health);
            HUD.IndicateDamage();
        }
    }
    
    public float GetVelocity()
    {
        return _rb.linearVelocity.magnitude;
    }

    ///////////////////////////////// Input  Management ////////////////////////////////

    // Movement input from the input manager
    public void OnMove(InputValue input)
    {
        _movementVector = input.Get<Vector2>();
        if (_rb.linearVelocity.magnitude < 0.1f) HUD.resetDistance();
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
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f)) {
            // Add an sudden upwards force
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _maxAirSpeed = _rb.linearVelocity.magnitude;
        }
    }
    
    /* Vin Lettich
     * Functions to deal with armor and stimulant upgrades */
    public void ActivateUpgrade(int upgradeType)
    {
        // If upgrade is armor, set armor activated to true
        if (upgradeType == 0)
        {
            armorUpActivated = true;
        }
        // If upgrade is stim, set stim activated to true
        else if (upgradeType == 1)
        {
            stimulantUpActivated = true;
        }
    }

    public void DeactivateUpgrade(int upgradeType)
    {
        if(upgradeType == 0)
        {
            armorUpActivated = false;
        }
        else if (upgradeType == 1)
        {
            stimulantUpActivated = false;
        }
    }

    public void OnInteract()
    {
        RaycastHit hit;
        Physics.Raycast(head.position, head.forward, out hit, _interactDistance);
        //Debug.Log(hit);

        //if raycast hits
        if (hit.collider != null)
            //and hits a pickup
            if (hit.collider.CompareTag("Pickup"))
            {
                //pickup
                hit.collider.TryGetComponent(out PickupController pickup);
                pickup.PickupObject();
                //later dropping gun will be implemented in PickupObject()
            }
    }
}
