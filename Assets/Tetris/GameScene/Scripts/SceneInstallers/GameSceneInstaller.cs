using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CoroutinePerformer _coroutinePerformer;
    [SerializeField] private FieldManager _fieldHandler;
    public override void InstallBindings()
    {
        BindCommonServices();
        BindField();
    }
    private void BindCommonServices()
    {
        Container.Bind<Camera>().FromInstance(_mainCamera);
        Container.Bind<CoroutinePerformer>().FromInstance(_coroutinePerformer);
        Container.BindInterfacesAndSelfTo<KeyboardInput>().FromNew().AsSingle();
    }
    private void BindField()
    {
        Container.BindFactory<FieldManager, FieldManagerFactory>().FromComponentInNewPrefab(_fieldHandler);
        Container.BindInterfacesAndSelfTo<FieldCreater>().FromNew().AsSingle();
    }
}
