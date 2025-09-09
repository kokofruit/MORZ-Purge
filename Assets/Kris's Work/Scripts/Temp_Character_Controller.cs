using UnityEngine;

public class Temp_Character_Controller : MonoBehaviour
{
    private float speed = 3.0f;
    private float rotationSpeed = 90.0f; // degrees per second

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Rotate character
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

        // Move character
        Vector3 moveDirection = transform.forward * verticalInput * speed;

        characterController.SimpleMove(moveDirection);
    }
}
