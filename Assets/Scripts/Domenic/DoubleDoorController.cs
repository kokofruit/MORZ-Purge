//Main Contributor: Domenic Cannella
//Secondary Contributor:
//Reviewer:
//Description: A state machine that uses a coroutine to control the opening and closing of double doors.
//Dates: 9/29/2025 - 9/29/2025

using UnityEngine;
using System.Collections;

public class DoubleDoorController : MonoBehaviour
{
    //Distance and speed of sliding
    public float slideDistance = 3f;
    public float slideSpeed = 2f;

    //Assign the two door panels in the Inspector
    public Transform leftDoor;
    public Transform rightDoor;

    private Vector3 leftClosedPosition;
    private Vector3 rightClosedPosition;
    private Vector3 leftOpenPosition;
    private Vector3 rightOpenPosition;

    private Coroutine slideCoroutine = null;

    void Start()
    {
        //Store the initial closed positions
        leftClosedPosition = leftDoor.position;
        rightClosedPosition = rightDoor.position;

        //Left door slides left
        leftOpenPosition = leftClosedPosition - transform.right * slideDistance;
        //Right door slides right
        rightOpenPosition = rightClosedPosition + transform.right * slideDistance;
    }

    void OnTriggerEnter(Collider other)
    {
        //Checks if player has entered trigger zone to open door
        if (other.CompareTag("Player"))
        {
            StartSliding(leftOpenPosition, rightOpenPosition);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Checks if player has left trigger zone to close door
        if (other.CompareTag("Player"))
        {
            StartSliding(leftClosedPosition, rightClosedPosition);
        }
    }

    private void StartSliding(Vector3 leftTarget, Vector3 rightTarget)
    {
        //Stop any existing slide coroutine before starting a new one
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }
        slideCoroutine = StartCoroutine(SlideDoors(leftTarget, rightTarget));
    }

    private IEnumerator SlideDoors(Vector3 leftTarget, Vector3 rightTarget)
    {
        while (Vector3.Distance(leftDoor.position, leftTarget) > 0.01f ||
               Vector3.Distance(rightDoor.position, rightTarget) > 0.01f)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftTarget, slideSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightTarget, slideSpeed * Time.deltaTime);

            //Wait for the next frame
            yield return null;
        }

        //Ensure exact position at the end
        leftDoor.position = leftTarget;
        rightDoor.position = rightTarget;

        slideCoroutine = null;
    }
}
