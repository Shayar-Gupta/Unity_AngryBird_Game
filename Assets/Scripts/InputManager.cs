using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;
    private InputAction _mousePositionAction, _mouseAction;
    public static Vector2 MousePosition;
    public static bool WasLeftMouseButtonPressed, WasLeftMouseButtonReleased, IsLeftMousePressed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        _mousePositionAction = playerInput.actions["MousePosition"];
        _mouseAction = playerInput.actions["Mouse"];
    }

    private void Update()
    {
        MousePosition = _mousePositionAction.ReadValue<Vector2>();
        WasLeftMouseButtonPressed = _mouseAction.WasPressedThisFrame();
        WasLeftMouseButtonReleased = _mouseAction.WasReleasedThisFrame();
        IsLeftMousePressed = _mouseAction.IsPressed();
    }
}
