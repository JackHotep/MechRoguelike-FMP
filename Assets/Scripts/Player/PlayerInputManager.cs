using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Input Action Asset and Action Map Name")]
    [SerializeField] private InputActionAsset pControls;
    [SerializeField] private string actionMap = "Player Controls";
    
    [Header("Action Names")]
    [SerializeField] private string move = "Movement";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string boost = "Boost";
    [SerializeField] private string leftShoot = "Left Shoot";
    [SerializeField] private string rightShoot = "Right Shoot";

    // Find actions on start
    private InputAction moveAct;
    private InputAction lookAct;
    private InputAction jumpAct;
    private InputAction boostAct;
    private InputAction leftShootAct;
    private InputAction rightShootAct;

    private int attacking = 0;
    
    // Value of input (Detect whether it is pressed)
    // Get and Set to protect the value
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Jump { get; private set; }
    public float Boost { get; private set; }
    
    //public bool LeftShoot { get; private set; }
    //public bool RightShoot { get; private set; }
    
    // Force script to be singleton for easy access
    public static PlayerInputManager Inst { get; private set; }

    private void Awake()
    {
        //Check whether this is only instance
        if (Inst == null) { Inst = this; DontDestroyOnLoad((gameObject)); }
        else { Destroy(gameObject); }

        moveAct = pControls.FindActionMap(actionMap).FindAction(move);
        lookAct = pControls.FindActionMap(actionMap).FindAction(look);
        jumpAct = pControls.FindActionMap(actionMap).FindAction(jump);
        boostAct = pControls.FindActionMap(actionMap).FindAction(boost);
        leftShootAct = pControls.FindActionMap(actionMap).FindAction(leftShoot);
        rightShootAct = pControls.FindActionMap(actionMap).FindAction(rightShoot);
        InputRegistrar();

        DeviceRegister();
    }

    void DeviceRegister()
    {
        foreach (var device in InputSystem.devices) { if (device.enabled) { Debug.Log("Device active: " + device.name); } }
    }

    // The Registrar manages the inputs registration by defining what is and isn't an input
    private void InputRegistrar()
    {
        moveAct.performed += context => Move = context.ReadValue<Vector2>();
        moveAct.canceled += context => Move = Vector2.zero;
        
        lookAct.performed += context => Look = context.ReadValue<Vector2>();
        lookAct.canceled += context => Look = Vector2.zero;

        jumpAct.performed += context => Jump = true;
        jumpAct.canceled += context => Jump = false;

        boostAct.performed += context => Boost = context.ReadValue<float>();
        boostAct.canceled += context => Boost = 0f;

        leftShootAct.performed += LeftAttackPerformed;
        leftShootAct.canceled += LeftAttackCancelled;

        rightShootAct.performed += RightAttackPerformed;
        rightShootAct.canceled += RightAttackCancelled;
    }

    public int GetAttack()
    {
        return attacking;
    }
    
    private void LeftAttackPerformed(InputAction.CallbackContext context)
    {
        attacking += 1;
    }

    private void LeftAttackCancelled(InputAction.CallbackContext context)
    {
        attacking -= 1;
    }
    
    private void RightAttackPerformed(InputAction.CallbackContext context)
    {
        attacking += 2;
    }

    private void RightAttackCancelled(InputAction.CallbackContext context)
    {
        attacking -= 2;
    }
    
    private void OnEnable()
    {
        moveAct.Enable();
        lookAct.Enable();
        jumpAct.Enable();
        boostAct.Enable();
        leftShootAct.Enable();
        rightShootAct.Enable();

        InputSystem.onDeviceChange += DeviceChange;
    }

    private void OnDisable()
    {
        moveAct.Disable();
        lookAct.Disable();
        jumpAct.Disable();
        boostAct.Disable();
        leftShootAct.Disable();
        rightShootAct.Disable();

        InputSystem.onDeviceChange -= DeviceChange;
    }

    private void DeviceChange(InputDevice device, InputDeviceChange newDevice)
    {
        switch (newDevice)
        {
            case InputDeviceChange.Disconnected: Debug.Log("Disconnected device: " + device.name);
                // Create UI popup?
                break;
            case InputDeviceChange.Reconnected: Debug.Log("Reconnected device: " + device.name);
                // Create UI popup?
                break;
        }
    }

    private void Start()
    {
        //Weapon weaponLeft = 
    }
}
