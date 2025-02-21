using System;
public interface IInput
{
    public event Action<float> OnMove;
    public event Action<float> OnFall;
}
