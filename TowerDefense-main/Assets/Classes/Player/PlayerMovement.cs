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
        Debug.Log("PlayerMovement script started.");
    }

    // Update is called once per frame
    void Update()
    {
        // Capture input
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        

        // Call movement methods
        MovePlayer();
        MoveCamera();
    }

   private void MovePlayer()
{
    // Get horizontal movement input (X and Z) and apply it to X and Z only
    Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);
    MoveVector.y = 0f; // Prevent any vertical (Y) movement from WASD input

    // Apply gravity and jumping logic
    if (Controller.isGrounded)
    {
        // Reset Y velocity when grounded
        Velocity.y = -1f; // Slight downward force to ensure we stay grounded

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Apply jump force when space is pressed
            Velocity.y = Jumpforce;
        }
    }
    else
    {
        // Apply gravity when not grounded
        Velocity.y += Gravity * Time.deltaTime;
      

    }

    

    // Apply only horizontal movement (X and Z axes) through WASD input
    Controller.Move(MoveVector * Speed * Time.deltaTime);
    
    // Apply vertical movement (Y axis) separately with gravity and jumping
    Controller.Move(Velocity * Time.deltaTime);
}


    private void MoveCamera()
    {
        if(Input.GetMouseButton(1))
        {
            // Rotate the camera based on mouse input
        xRot -= PlayerMouseInput.y * Sensitivity;
        xRot = Mathf.Clamp(xRot, -90f, 90f); // Clamps vertical rotation

        // Apply rotation to player and camera
        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }    // 1 means a right click 0 means a left click on the mouse
    }
}
