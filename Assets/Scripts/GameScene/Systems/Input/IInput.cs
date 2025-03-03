using System;
public interface IInput
{
    public event Action<int> OnMoveButtonDown;
    public event Action OnMoveButtonUp;
    public event Action OnFallButtonDown;
    public event Action OnFallButtonUp;
    public event Action OnRotateButtonDown;
}
