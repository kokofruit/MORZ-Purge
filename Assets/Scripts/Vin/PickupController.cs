using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// Main Contributor: Vin
// Secondary Contributor:
// Reviewer: 
// Description: Parent script for pickups (health, ammo, etc.)

/*
 * TODO:
 * Trigger Event - child ?
 *      Deletion - on trigger
 * Spawning
 * Use raycasting to determine bobbing start position (so it doesnt float through the floor)
 */

public class PickupController : MonoBehaviour
{

    // Variables
    private float movementSpeed = 5f;
    private bool DEBUG = true;
    private bool _isGrounded;

    void Start()
    {
        
    }

    void Update()
    {
        // Rotates object
        transform.Rotate(0, 0, 30 * Time.deltaTime);

        // Bobbles object
        Vector3 pos = transform.position;
        float newY = Mathf.Sin(Time.time * movementSpeed);
        transform.position = new Vector3(pos.x, newY, pos.z);

        
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO:
        // trigger event
        if (DEBUG) Debug.Log("Triggered");
    }
}
