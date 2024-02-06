using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField, Readonly] private Vector3 _movementInput;

    public event Action<Vector3> OnMoveAction;

    private void OnEnable()
    {
        JoyStick.OnMove -= OnMove;
        JoyStick.OnMove += OnMove;
    }

    private void OnDisable()
    {
        JoyStick.OnMove -= OnMove;
    }

    public void OnMove(Vector3 movementInput)
    {
        _movementInput = movementInput;
        OnMoveAction?.Invoke(_movementInput);
    }
}
