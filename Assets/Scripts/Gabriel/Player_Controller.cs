
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float runSpeedMultiplier = 2;
    public float lookSensX = 0.1f;
    public float lookSensY = 0.1f;
    private Rigidbody _rb;
    private Transform _head;
    private Vector2 _movementVector;
    private Vector2 _lookVector;
    private bool _isSprinting;
    private float _lookX;
    private float _lookY;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _head = transform.Find("Head");

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        _lookX += _lookVector.x * lookSensX;
        _lookY += _lookVector.y * lookSensY;
        _lookY = Mathf.Clamp(_lookY, -90, 90);

        transform.rotation = Quaternion.Euler(0, _lookX, 0);
        _head.localRotation = Quaternion.Euler(-_lookY, 0, 0);

        float _speed = _isSprinting ? walkSpeed * runSpeedMultiplier : walkSpeed;
        Vector3 _velocity = _movementVector * _speed;

        Vector3 localVector = transform.TransformDirection(new Vector3(_velocity.x, 0, _velocity.y));
        Vector3 targetPos = _rb.position + localVector * walkSpeed * Time.deltaTime;
        _rb.MovePosition(targetPos);
    }

    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnMove(InputValue input)
    {
        _movementVector = input.Get<Vector2>();
    }

    public void OnLook(InputValue input)
    {
        _lookVector = input.Get<Vector2>();
    }

    public void OnSprint(InputValue input)
    {
        float value = input.Get<float>();
        if (value == 1)
            _isSprinting = true;
        if (value == 0)
            _isSprinting = false;
    }
}
