using UnityEngine;
using Zenject;

public class FieldInstaller : MonoInstaller
{
    [SerializeField] private FieldView _fieldView;
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private LevelView _levelView;
    [SerializeField] private FigureGeneratorView _figureGeneratorView;

    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Block _blockPrefab;
    public override void InstallBindings()
    {
        BindFactories();
        BindFigureGenerator();
        BindField();
        BindScore();
        BindLevel();
    }
    private void BindFactories()
    {
        Container.BindFactory<Cell, CellFactory>().FromComponentInNewPrefab(_cellPrefab);
        Container.BindFactory<Block, BlockFactory>().FromComponentInNewPrefab(_blockPrefab);
    }
    private void BindFigureGenerator()
    {
        Container.BindInterfacesAndSelfTo<FigureGeneratorView>().FromInstance(_figureGeneratorView);
        Container.BindInterfacesAndSelfTo<FigureGenerator>().FromNew().AsSingle();
    }
    private void BindField()
    {
        Container.BindInterfacesAndSelfTo<FieldView>().FromInstance(_fieldView);
        Container.BindInterfacesAndSelfTo<Field>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<FigureMover>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<FieldHandler>().FromNew().AsSingle().NonLazy();
    }
    private void BindScore()
    {
        Container.BindInterfacesAndSelfTo<ScoreView>().FromInstance(_scoreView);
        Container.BindInterfacesAndSelfTo<Score>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<ScoreHandler>().FromNew().AsSingle().NonLazy();
    }
    private void BindLevel()
    {
        Container.BindInterfacesAndSelfTo<LevelView>().FromInstance(_levelView);
        Container.BindInterfacesAndSelfTo<Level>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelHandler>().FromNew().AsSingle().NonLazy();
    }
}
