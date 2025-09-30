using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// Main Contributor: Vin
// Secondary Contributor: Mark
// Reviewer: 
// Description: Parent script for pickups (health, ammo, etc.)

public class PickupController : MonoBehaviour
{
    // Private Variables
    private float movementSpeed = 3f;
    private float yPosition;
    private float addHeight = .75f;
    private float raycastDistance = 10f;

    void Start()
    {
        // Raycast to the ground to spawn above it
        RaycastHit hit;
        // Store the object transform
        Vector3 pos = transform.position;
        // Raycast down to ground
        Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, raycastDistance);
        // Save the hitPoint (ground)
        Vector3 hitPoint = hit.point;
        // Calculate new yPosition
        yPosition = hitPoint.y + addHeight;
        // Set new object position above ground (addHeight)
        transform.position = new Vector3(pos.x, yPosition, pos.z);
    }

    void Update()
    {
        // Rotates object
        transform.Rotate(0, 0, 30 * Time.deltaTime);

        // Bobbles object up and down
        Vector3 pos = transform.position;
        float newY = (Mathf.Sin(Time.time * movementSpeed) / 4) + yPosition;    // add new yPosition to spawn above ground
        transform.position = new Vector3(pos.x, newY, pos.z);
    }

    public virtual void PickupObject()
    {
        Destroy(gameObject);
    }
}
