using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof (CharacterController))]
public class Move : MonoBehaviour
{
    private Rigidbody rb;
    private InputManager inputManager;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] Camera cam;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private bool jumpReady = true;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airMultiplier=0.5f;

    void Awake()
    {
        if(cam==null &&GameObject.FindWithTag("Main Camera").GetComponent<Camera>()!=null)
        {
            cam=GameObject.FindWithTag("Main Camera").GetComponent<Camera>();
        }
    }
    private void Start()
    {
        rb=GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;
        controller = GetComponent<CharacterController>();
        Cursor.lockState =CursorLockMode.Locked;
        Cursor.visible=false;
    }

    void Update()
    {
        DragControl();
        Movement();
        Jump();
        SpeedControl();
    }
    private void Movement()
    {
        Vector2 movement = inputManager.move;
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move =cam.transform.forward*move.z + cam.transform.right *move.x;
        move.y=0f;
        if(groundedPlayer)
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
        }
        else
        {
            controller.Move(move * Time.deltaTime * airMultiplier* playerSpeed);
        }
         
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
    private void Jump()
    {
        if (inputManager.buttonJump>0f && groundedPlayer && jumpReady)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumpReady=false;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        Invoke(nameof(ResetJump),jumpCooldown);
    }
    private void ResetJump(){
        jumpReady=true;
    }
    private void DragControl()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            rb.drag=groundDrag;
            if(playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;

            }
        }
        else 
        {
            rb.drag=0f;
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel=new Vector3(rb.velocity.x,0f,rb.velocity.z);
        if(flatVel.magnitude >playerSpeed)
        {
            Vector3 limitVel =flatVel.normalized*playerSpeed;
            rb.velocity =new Vector3(limitVel.x,rb.velocity.y,limitVel.z);
        }
    }
}