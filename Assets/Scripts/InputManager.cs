using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private static InputManager _Instance;
    public static InputManager Instance
    {
        get
        {
            return _Instance;
        }
    }
    
    private BasicInputActions inputActions;
    //public Vector2 move;sss
    
    [SerializeField]public Vector2 move{get;private set;}
    [SerializeField]public Vector2 deltamouse{get;private set;}
    [SerializeField]public float buttonShoot{get;private set;}
    [SerializeField]public float buttonEscape{get;private set;}
    [SerializeField]public float buttonInteract{get;private set;}
    [SerializeField]public float buttonJump{get;private set;}
    

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
        if(context.performed)
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
        if(context.performed)
        {
            Debug.Log(context.ReadValue<float>());
            buttonShoot = context.ReadValue<float>();
        }
    }
    public void GetButtonEscape(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log(context.ReadValue<float>());
            buttonEscape = context.ReadValue<float>();
        }
    }   

    public void GetButtonInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log(context.ReadValue<float>());
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
