using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class FigureGenerator : IInitializable
{
    public event Action<FigureSettings> OnEnqueueFigure;
    public event Action OnDequeueFigure;

    private FigureGeneratorSettings _settings;
    private System.Random _random;
    private int _seed = 1321321;

    private int _figuresQueueCount;
    private Queue<FigureSettings> _figuresQueue;
    public FigureGenerator(FigureGeneratorSettings settings)
    {
        _settings = settings;
        _figuresQueueCount = _settings.FiguresQueueCount;
        _figuresQueue = new Queue<FigureSettings>();

        _random = new System.Random(_seed);
    }
    public void Initialize()
    {
        for(int i = 0; i < _figuresQueueCount; i++)
        {
            CreateNextFigure();
        }
    }
    public FigureSettings GetNextFigure()
    {
        var figure = _figuresQueue.Dequeue();
        OnDequeueFigure?.Invoke();
        CreateNextFigure();
        return figure;
    }
    private void CreateNextFigure()
    {
        var figure = GetRandomFigure();
        _figuresQueue.Enqueue(figure);
        OnEnqueueFigure?.Invoke(figure);
    }
    private FigureSettings GetRandomFigure()
    {
        int randomNumber = _random.Next(0, _settings.FiguresSettings.Length);
        return _settings.FiguresSettings[randomNumber];
    }
}