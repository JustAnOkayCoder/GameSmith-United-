using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables we need
    private float xRot;
    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;

    // Below are the references you need to call back on later
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private CharacterController Controller;
    [Space]
    [SerializeField] private float Speed = 5f;
    [SerializeField] private float Jumpforce = 5f;
    [SerializeField] private float Sensitivity = 2f;
    [SerializeField] private float Gravity = -9.81f; // Normal gravity setting

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MoveCamera();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput); // Move with the direction of your input

        // Apply gravity based on whether the player is grounded
        if (Controller.isGrounded)
        {
            Velocity.y = -1f; // Slight downward force to keep grounded

            // Make the player jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Velocity.y = Jumpforce; // Apply jump force
            }
        }
        else
        {
            // Apply gravity over time
            Velocity.y += Gravity * Time.deltaTime; // Continuously apply gravity when not grounded
        }

        // Apply movement vectors
        Controller.Move(MoveVector * Speed * Time.deltaTime); // Move Vector determines speed and direction
        Controller.Move(Velocity * Time.deltaTime); // Apply vertical movement with gravity
    }

    private void MoveCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;
        xRot = Mathf.Clamp(xRot, -90f, 90f); // Clamp the vertical camera rotation

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
}
