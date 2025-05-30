using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform leftDoor;  // Lewa czêœæ œluzy
    public Transform rightDoor; // Prawa czêœæ œluzy
    public Light statusLight;   // Œwiat³o nad œluz¹
    public Color lockedColor = Color.red;
    public Color unlockedColor = Color.green;
    public float openDistance = 2f;
    public float openSpeed = 2f;
    public bool isUnlocked = false;
    private bool playerInTrigger = false;
    private Vector3 leftClosedPos, rightClosedPos;

    private void Start()
    {
        leftClosedPos = leftDoor.position;
        rightClosedPos = rightDoor.position;
        statusLight.color = lockedColor;
    }

    private void Update()
    {
        if (isUnlocked && playerInTrigger)
        {
            leftDoor.position = Vector3.Lerp(leftDoor.position, leftClosedPos - Vector3.right * openDistance, Time.deltaTime * openSpeed);
            rightDoor.position = Vector3.Lerp(rightDoor.position, rightClosedPos + Vector3.right * openDistance, Time.deltaTime * openSpeed);
        }
        else
        {
            leftDoor.position = Vector3.Lerp(leftDoor.position, leftClosedPos, Time.deltaTime * openSpeed);
            rightDoor.position = Vector3.Lerp(rightDoor.position, rightClosedPos, Time.deltaTime * openSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    public void UnlockDoor()
    {
        isUnlocked = true;
        statusLight.color = unlockedColor;
    }
    public void LockDoor()
    {
        isUnlocked = false;
        statusLight.color = lockedColor;
    }
}
