using System;
using System.Data;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float boostMult = 2f;
    
    [Header("Jump Values")]
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private float grav = 9.81f;

    [Header("View Values")] 
    [SerializeField] private float aimSens = 2f;
    [SerializeField] private float vertClamp = 70f;

    private CharacterController charCon;
    private Camera camera;
    private PlayerInputManager inputMan;
    private Vector3 movementCurrent = Vector3.zero;
    private float vertRot;
    
    private void Awake()
    {
        charCon = GetComponent<CharacterController>();
        camera = Camera.main;
        inputMan = PlayerInputManager.Inst;
    }

    private void Update()
    {
        MovementManager();
        ViewManager();
    }

    void MovementManager()
    {
        float speed = moveSpeed * (inputMan.Boost > 0 ? boostMult : 1f);
        
        // x for x, 0 for y and y for z because vector2, and we dont need to use y for this
        Vector3 inputDir = new Vector3(inputMan.Move.x, 0f, inputMan.Move.y);
        Vector3 worldDir = transform.TransformDirection(inputDir);
        worldDir.Normalize();

        movementCurrent.x = worldDir.x * speed;
        movementCurrent.z = worldDir.z * speed;

        JumpManager();
        charCon.Move(movementCurrent * Time.deltaTime);
    }

    void JumpManager()
    {
        if (charCon.isGrounded)
        {
            movementCurrent.y = -0.5f;

            if (inputMan.Jump)
            {
                movementCurrent.y = jumpPower;
            }
        }
        else
        {
            movementCurrent.y -= grav * Time.deltaTime;
        }
    }

    void ViewManager()
    {
        float xRot = inputMan.Look.x * aimSens;
        transform.Rotate(0, xRot, 0);

        vertRot -= inputMan.Look.y * aimSens;
        vertRot = Mathf.Clamp(vertRot, -vertClamp, vertClamp);
        camera.transform.localRotation = Quaternion.Euler(vertRot, 0, 0);
    }
}
