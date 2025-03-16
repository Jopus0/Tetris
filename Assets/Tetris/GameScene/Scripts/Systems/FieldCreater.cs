using UnityEngine;
using Zenject;

public class FieldCreater : IInitializable
{
    private FieldManagerFactory _fieldHandlerFactory;
    public FieldCreater(FieldManagerFactory fieldHandlerFactory)
    {
        _fieldHandlerFactory = fieldHandlerFactory;
    }
    public void Initialize()
    {
       var fieldHandler = _fieldHandlerFactory.Create();
       //fieldHandler.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
       //fieldHandler.transform.position = new Vector3(2f, 1f, 0f);
    }
}
