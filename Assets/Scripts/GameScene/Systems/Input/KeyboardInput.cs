using System;
using UnityEngine;
using Zenject;

public class KeyboardInput : IInput, ITickable
{
    public event Action<int> OnMoveButtonDown;
    public event Action OnMoveButtonUp;
    public event Action OnFallButtonDown;
    public event Action OnFallButtonUp;
    public event Action OnRotateButtonDown;

    private float prevHorizontalAxis;
    public void Tick()
    {
        CheckMove();
        CheckFall();
        CheckRotation();
    }
    private void CheckMove()
    {
        if (Input.GetKeyDown("d"))
            OnMoveButtonDown?.Invoke(1);

        if (Input.GetKeyDown("a"))
            OnMoveButtonDown?.Invoke(-1);

        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            OnMoveButtonUp?.Invoke();
    }
    private void CheckFall()
    {
        if (Input.GetKeyDown("s"))
            OnFallButtonDown?.Invoke();

        if (Input.GetKeyUp("s"))
            OnFallButtonUp?.Invoke();
    }
    private void CheckRotation()
    {
        if (Input.GetKeyDown("w"))
            OnRotateButtonDown?.Invoke();
    }
}
