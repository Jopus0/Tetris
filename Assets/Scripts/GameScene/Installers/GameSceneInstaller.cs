using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private Transform _cellTransform;
    [SerializeField] private Transform _blockTransform;

    [SerializeField] private FieldSettings _fieldSettings;
    [SerializeField] private FieldViewSettings _fieldViewSettings;
    [SerializeField] private FigureSettings[] _figureSettings;

    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Block _blockPrefab;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<KeyboardInput>().FromNew().AsSingle();

        Container.Bind<Camera>().FromInstance(_mainCamera);

        Container.Bind<FieldSettings>().FromInstance(_fieldSettings);
        Container.Bind<FieldViewSettings>().FromInstance(_fieldViewSettings);

        Container.BindFactory<Cell, CellFactory>().FromComponentInNewPrefab(_cellPrefab);
        Container.BindFactory<Block, BlockFactory>().FromComponentInNewPrefab(_blockPrefab);

        Container.BindInterfacesAndSelfTo<Grid>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<GridView>().FromNew().AsSingle().WithArguments(_cellTransform);

        Container.BindInterfacesAndSelfTo<FigureGenerator>().FromNew().AsSingle().WithArguments(_figureSettings);

        Container.BindInterfacesAndSelfTo<FieldHandler>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<FieldView>().FromNew().AsSingle();
    }
}
