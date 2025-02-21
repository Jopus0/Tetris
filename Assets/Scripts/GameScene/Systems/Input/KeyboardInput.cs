using System;
using UnityEngine;
using Zenject;

public class KeyboardInput : IInput, ITickable
{
    public event Action<float> OnMove;
    public event Action<float> OnFall;
    public void Tick()
    {
        CheckMove();
        CheckFall();
    }
    private void CheckMove()
    {
        float axis = Input.GetAxis("Horizontal");
        if (axis != 0f)
            OnMove?.Invoke(axis);
    }
    private void CheckFall()
    {
        float axis = Input.GetAxis("Vertical");
        if (axis < 0f)
            OnFall?.Invoke(Mathf.Abs(axis));
    }
}
