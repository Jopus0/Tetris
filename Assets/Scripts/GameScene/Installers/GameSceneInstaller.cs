using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private FieldView _fieldView;
    [SerializeField] private ScoreView _scoreView;

    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Block _blockPrefab;
    public override void InstallBindings()
    {
        BindCommonServices();
        BindField();
        BindScore();
    }
    private void BindCommonServices()
    {
        Container.BindInterfacesAndSelfTo<KeyboardInput>().FromNew().AsSingle();
        Container.Bind<Camera>().FromInstance(_mainCamera);
    }
    private void BindField()
    {
        Container.BindFactory<Cell, CellFactory>().FromComponentInNewPrefab(_cellPrefab);
        Container.BindFactory<Block, BlockFactory>().FromComponentInNewPrefab(_blockPrefab);

        Container.BindInterfacesAndSelfTo<Grid>().FromNew().AsSingle();

        Container.BindInterfacesAndSelfTo<Field>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<FieldView>().FromInstance(_fieldView);

        Container.BindInterfacesAndSelfTo<FigureGenerator>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<FigureMover>().FromNew().AsSingle().NonLazy();
    }
    private void BindScore()
    {
        Container.BindInterfacesAndSelfTo<Score>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<ScoreView>().FromInstance(_scoreView);
        Container.BindInterfacesAndSelfTo<ScoreHandler>().FromNew().AsSingle().NonLazy();
    }
}
