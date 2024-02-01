using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    [field:Header("Values to pass")]
    public Vector2 move{get;private set;}
    public Vector2 deltamouse{get;private set;}
    public float buttonShoot{get;private set;}
    public float buttonEscape{get;private set;}
    public float buttonInteract{get;private set;}
    public float buttonJump{get;private set;}
    private static InputManager _Instance;
    private BasicInputActions inputActions;
    
    public static InputManager Instance => _Instance;

    private void Awake() {
        if(_Instance !=null && _Instance!=this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _Instance=this;
        }
    }
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        inputActions = new BasicInputActions();
    }
    public void GetMove(InputAction.CallbackContext context)
    {
        if(context.performed||context.canceled)
        {
            move = context.ReadValue<Vector2>();
        }
    }
    public void GetMouseDelta(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            deltamouse = context.ReadValue<Vector2>();
        }
    }
    public void GetButtonShoot(InputAction.CallbackContext context) 
    {
        if(context.performed||context.canceled)
        {
            Debug.Log($"Input Shoot: {context.ReadValue<float>()}");
            buttonShoot = context.ReadValue<float>();
        }
    }
    public void GetButtonEscape(InputAction.CallbackContext context)
    {
        if(context.performed||context.canceled)
        {
            Debug.Log($"Input Escape: {context.ReadValue<float>()}");
            buttonEscape = context.ReadValue<float>();
        }
    }   

    public void GetButtonInteract(InputAction.CallbackContext context)
    {
        if(context.performed||context.canceled)
        {
            Debug.Log($"Input Interact: {context.ReadValue<float>()}");
            buttonInteract = context.ReadValue<float>();

        }
    }
    public void GetButtonJump(InputAction.CallbackContext context)
    {
        if(context.performed||context.canceled)
        {
            Debug.Log(context.ReadValue<float>());
            buttonJump = context.ReadValue<float>();

        }
    }
}
