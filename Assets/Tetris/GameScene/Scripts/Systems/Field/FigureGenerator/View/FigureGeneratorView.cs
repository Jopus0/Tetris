using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FigureGeneratorView : MonoBehaviour
{
    [SerializeField] private Transform _groupTransform;
    [SerializeField] private BlockPanel _blockPanelPrefab;
    private FigureGenerator _figureGenerator;

    private Queue<BlockPanel> _blockPanelsQueue;

    [Inject]
    private void Construct(FigureGenerator figureGenerator)
    {
        _figureGenerator = figureGenerator;
        _blockPanelsQueue = new Queue<BlockPanel>();

        _figureGenerator.OnEnqueueFigure += AddBlockPanel;
        _figureGenerator.OnDequeueFigure += RemoveBlockPanel;
    }
    public void OnDestroy()
    {
        _figureGenerator.OnEnqueueFigure -= AddBlockPanel;
        _figureGenerator.OnDequeueFigure -= RemoveBlockPanel;
    }
    public void AddBlockPanel(FigureSettings figureSettings)
    {
        var blockPanel = Instantiate(_blockPanelPrefab, _groupTransform);
        blockPanel.SetFigureImage(figureSettings.FigureImage, figureSettings.Color);
        _blockPanelsQueue.Enqueue(blockPanel);
    }
    public void RemoveBlockPanel()
    {
        var blockPanel = _blockPanelsQueue.Dequeue();
        Destroy(blockPanel.gameObject);
    }
}
