using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private InputActionSystem playerInputActions;
    private void Awake()
    {
        playerInputActions = new InputActionSystem();
        playerInputActions.Player.Enable();
    }
    public Vector2 GetMomentVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
