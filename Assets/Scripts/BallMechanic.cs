using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMechanic : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rb;
    private float xInput;
    public GameObject arrow;
    private bool isBallMoving = true; // Boolean to track if the ball is moving
    private Vector3 ballPositionAtStop; // Store the ball's position when it stops
    private float rotationSpeed = 100f; // Speed of rotation
    private int gamePhase = 0; // 0: Initial phase, 1: Arrow rotation, 2: Ball shooting, 3: Ball stopped or fallen
    public float shootingForce = 1000f; //second phase force
    public float yThreshold = 10f; // Y position threshold to transition to phase 3
    public float velocityThreshold = 0.1f; // Velocity threshold to consider the ball has no momentum
    public GameObject[] pins; // Array of pin GameObjects
    private int fallenPinsCount = 0; // Counter for fallen pins
    private int maxFalls = 3; // Maximum number of times pins can fall
    private int pinsPushed = 0; // Counter for pins pushed
    private bool pinsChecked = false; // Flag to track if pins have been checked

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        xInput = 0.0f;
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for space key press
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            switch (gamePhase)
            {
                case 0: //Ball stops moving, arrow shown
                    isBallMoving = false;
                    arrow.SetActive(true);
                    ballPositionAtStop = transform.position;
                    gamePhase = 1;
                    // Store the ball's position when it stops
                    ballPositionAtStop = transform.position;
                    // Update the arrow's x position to match the ball's x position
                    arrow.transform.position = new Vector3(ballPositionAtStop.x, arrow.transform.position.y, arrow.transform.position.z);
                    Debug.Log("Phase: " + gamePhase);
                    break;
                case 1: // Ball shooting
                    Vector3 forceDirection = (arrow.transform.position - ballPositionAtStop).normalized;
                    rb.AddForce(forceDirection * shootingForce, ForceMode.Impulse);
                    isBallMoving = false;
                    arrow.SetActive(false);
                    gamePhase = 2;
                    Debug.Log("Phase: " + gamePhase);
                    break;
                case 2:

                    gamePhase = 3;
                    Debug.Log("Phase: " + gamePhase);
                    break;
            }
        }

        if (gamePhase == 1)
        {
            float rotation = 0f;
            if (Keyboard.current.wKey.isPressed)
            {
                rotation = rotationSpeed * Time.deltaTime;
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                rotation = -rotationSpeed * Time.deltaTime;
            }

            // Rotate the arrow around the ball
            arrow.transform.RotateAround(ballPositionAtStop, Vector3.up, rotation);

        }

        // Check if the ball has stopped or fallen and has no momentum
        if (gamePhase == 2 && (transform.position.y < yThreshold || rb.velocity.magnitude < velocityThreshold))
        {
            gamePhase = 3;
        }

        if (gamePhase == 3 && !pinsChecked)
        {
            HandleFallenPins();
            pinsChecked = true; // Set flag to true after checking pins
        }
    }

    void OnMove(InputValue movementValue)
    {
        if (gamePhase == 0) // Only allow moving the ball in the initial phase
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            xInput = movementVector.x;
        }
    }

    void FixedUpdate()
    {
        if (gamePhase == 0 && isBallMoving) // Only apply force in the initial phase if the ball is moving
        {
            Vector3 movement = new Vector3(xInput, 0, 0);
            rb.AddForce(movement * speed);
        }
        else if (gamePhase == 1) // Ensure the ball stops in the shooting phase
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void HandleFallenPins()
    {
        foreach (GameObject pin in pins)
        {
            if (pin.activeSelf) // Assuming pins are active when standing
            {
                // Check if pin has fallen (e.g., based on position or collision)
                if (HasPinFallen(pin))
                {
                    pin.SetActive(false); // Hide the pin
                    fallenPinsCount++;
                    pinsPushed++;
                    Debug.Log("Pin fell, current count: " + pinsPushed);
                }
            }
        }

        // Check if all pins have been pushed
        if (pinsPushed >= 10)
        {
            Debug.Log("Congratulations!");
            // Reset game or proceed to next level
        }

        // Reset game phase if all pins have fallen
        if (fallenPinsCount >= maxFalls)
        {
            gamePhase = 0; // Reset to initial phase
            fallenPinsCount = 0; // Reset fallen pins counter
            pinsChecked = false; // Reset pins checked flag
            // Optionally, reset pins visibility or position here
            Debug.Log("Resetting game phase to 0");
        }
    }

    bool HasPinFallen(GameObject pin)
    {
        // Implement logic to determine if a pin has fallen
        // This could be based on position, collision, or other criteria
        // For demonstration, let's assume a pin falls if it's below a certain Y position
        return pin.transform.rotation.z != 0; // Example condition
    }
}