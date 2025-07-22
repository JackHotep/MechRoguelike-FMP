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

    // Find actions on start
    private InputAction moveAct;
    private InputAction lookAct;
    private InputAction jumpAct;
    private InputAction boostAct;
    
    // Value of input (Detect whether it is pressed)
    // Get and Set to protect the value
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Jump { get; private set; }
    public float Boost { get; private set; }
    
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
        InputRegistrar();
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
    }

    private void OnEnable()
    {
        moveAct.Enable();
        lookAct.Enable();
        jumpAct.Enable();
        boostAct.Enable();
    }

    private void OnDisable()
    {
        moveAct.Disable();
        lookAct.Disable();
        jumpAct.Disable();
        boostAct.Disable();
    }
}
