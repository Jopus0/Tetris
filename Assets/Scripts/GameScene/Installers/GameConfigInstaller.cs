using UnityEngine;
using Zenject;
public class GameConfigInstaller : MonoInstaller
{
    [SerializeField] private FieldSettings _fieldSettings;
    [SerializeField] private FieldViewSettings _fieldViewSettings;
    [SerializeField] private FigureGeneratorSettings _figureGeneratorSettings;
    [SerializeField] private FigureMoverSettngs _figureMoverSettngs;
    [SerializeField] private ScoreHandlerSettings _scoreHandlerSettings;
    [SerializeField] private LevelHandlerSettings _levelHandlerSettings;
    public override void InstallBindings()
    {
        BindConfigs();
    }
    private void BindConfigs()
    {
        Container.Bind<FieldSettings>().FromInstance(_fieldSettings);
        Container.Bind<FieldViewSettings>().FromInstance(_fieldViewSettings);
        Container.Bind<FigureGeneratorSettings>().FromInstance(_figureGeneratorSettings);
        Container.Bind<FigureMoverSettngs>().FromInstance(_figureMoverSettngs);
        Container.Bind<ScoreHandlerSettings>().FromInstance(_scoreHandlerSettings);
        Container.Bind<LevelHandlerSettings>().FromInstance(_levelHandlerSettings);
    }
}