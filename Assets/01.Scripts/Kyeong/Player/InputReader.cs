using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Input/InputReader", fileName = "InputReader")]
public class InputReader : ScriptableObject, PlayerInput.IPlayerActions
{
    public event Action<Vector2> PositionEvent;
    
    private PlayerInput _playerInputAction;

    private void OnEnable()
    {
        if (_playerInputAction == null)
        {
            _playerInputAction = new PlayerInput();
            _playerInputAction.Player.SetCallbacks(this);
        }
        
        _playerInputAction.Player.Enable();
    }

    public void OnTouchPos(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        PositionEvent?.Invoke(value);
    }
}
