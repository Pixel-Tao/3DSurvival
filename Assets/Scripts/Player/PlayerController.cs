using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float moveSpeedMultiplier = 2;
    public float accelerationTime = 5f;
    public float cooldownTime = 5f;
    public float jumpForce = 5f;
    private Vector2 curMovementInput;
    private float lastAccelerationTime;
    private bool isAccelerating;

    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float maxXLook;
    public float minXLook;
    private float camCurXRot;
    [Range(0, 1)] public float lookSensitivity = 1f;
    private Vector2 mouseDelta;

    public bool canLook = true;
    public Action inventory;
    public Action setting;

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        if (isAccelerating)
        {
            if (Time.time - lastAccelerationTime > accelerationTime)
            {
                isAccelerating = false;
            }

            dir *= moveSpeed * moveSpeedMultiplier;
        }
        else
        {
            dir *= moveSpeed;
        }
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        Ray[] ray = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down),
        };

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ToggleSetting();
        }
    }

    public void ToggleSetting()
    {
        setting?.Invoke();
        ToggleCursor();
    }

    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void OnAcceleration(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isAccelerating = true;
            lastAccelerationTime = Time.time;
        }
    }

    public void SetLookSensitivity(float value)
    {
        lookSensitivity = value;
    }
}
