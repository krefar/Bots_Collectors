using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SelectManager))]
public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;
    private SelectManager _selectManager;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _selectManager = GetComponent<SelectManager>();

        _playerInput.Player.MouseClick.performed += MouseClick;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void MouseClick(InputAction.CallbackContext context)
    {
        if (Mouse.current.leftButton.isPressed)
        {
            ProcessLeftButtonClick();
        }
        else if (Mouse.current.rightButton.isPressed)
        {
            ProcessRightButtonClick();
        }
    }

    private void ProcessRightButtonClick()
    {
        _selectManager.ProcessActionWithSelected();
    }

    private void ProcessLeftButtonClick()
    {
        _selectManager.ProcessSelection();
    }
}