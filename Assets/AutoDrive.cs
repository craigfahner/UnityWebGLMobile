using UnityEngine;

public class AutoDrive : MonoBehaviour
{
    public RCC_Inputs newInputs = new RCC_Inputs();
    public float throttleValue = 0.0f;
    public float throttle = 0.5f;
    public RCC_CarControllerV3 targetVehicle;
    private Vector3 lastPosition;
    private float positionCheckDelay = 10f; // Time in seconds to wait before checking position change
    private float timeSinceLastPositionChange;

    void Start()
    {
        lastPosition = transform.position;
        timeSinceLastPositionChange = 0f;
    }

    void Update()
    {
        newInputs.throttleInput = throttleValue;
        targetVehicle.OverrideInputs(newInputs);

        // Check if the car has fallen below a certain height
        if (transform.position.y < -10.0f)
        {
            ResetCarPosition();
        }

        // Check if the car's position hasn't changed significantly in 10 seconds, ignoring the y-axis
        Vector3 currentPositionXZ = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 lastPositionXZ = new Vector3(lastPosition.x, 0f, lastPosition.z);

        if (Vector3.Distance(currentPositionXZ, lastPositionXZ) < 5f) // Using a small threshold for 'significant change'
        {
            timeSinceLastPositionChange += Time.deltaTime;
        }
        else
        {
            lastPosition = transform.position; // Update lastPosition to the current position
            timeSinceLastPositionChange = 0f; // Reset the timer since there's a significant position change
        }

        // If the position hasn't changed significantly for more than 10 seconds
        if (timeSinceLastPositionChange >= positionCheckDelay)
        {
            ResetCarPosition(true); // Reset the car's position with an offset on the z-axis
        }
    }

    void ResetCarPosition(bool addOffsetZ = false)
    {
        Vector3 newPos = transform.position;
        newPos.y = 10.0f; // Set the y position to 10
        if (addOffsetZ)
        {
            newPos.x += 20.0f; // Move car forward by 10 units along the z-axis if specified
        }
        transform.position = newPos; // Apply the new position
        lastPosition = transform.position; // Update the last known position to the new position
        timeSinceLastPositionChange = 0f; // Reset the time since the last position change
    }

}
