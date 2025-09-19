//Main Contributor: Domenic Cannella
//Secondary Contributor:
//Reviewer: Phil Cano
//Description: A state machine that uses a coroutine to control the opening and closing of doors.
//Dates: 9/16/2025 - 9/17/2025

using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    //Door customizable variables
    public float slideDistance = 3f;
    public float slideSpeed = 2f;

    private Transform door;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private Coroutine slideCoroutine = null;

    void Start()
    {
        //Grabs ONLY the cube game object, not the trigger zone
        door = GetComponentsInChildren<Transform>()[1];
        closedPosition = door.position;
        openPosition = closedPosition + transform.right * slideDistance;
    }

    void OnTriggerEnter(Collider other)
    {
        //Checks if player has entered trigger zone to close door
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered door range");
            StartSliding(openPosition);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Checks if player has left trigger zone to close door
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player left door range");
            StartSliding(closedPosition);
        }
    }

    private void StartSliding(Vector3 targetPosition)
    {
        //Stop any existing slide coroutine before starting a new one
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }
        slideCoroutine = StartCoroutine(SlideDoor(targetPosition));
    }

    private IEnumerator SlideDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(door.position, targetPosition) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, targetPosition, slideSpeed * Time.deltaTime);
            //Wait for the next frame
            yield return null;  
        }

        //Ensure exact position at the end
        door.position = targetPosition; 
        slideCoroutine = null;
    }
}
