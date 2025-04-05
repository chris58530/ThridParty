using UnityEngine;
using Zenject;
public abstract class ICommand : ScriptableObject
{
    [Inject]protected Listener listener;
    public bool isLazy;
    public bool isComplete { get; private set; }
    private InitController initController;
    protected DiContainer container;
    public virtual void Initialize(InitController initController, Listener listener, DiContainer container)
    {
        this.initController = initController;
        InjectService.Instance.Inject(this);
        listener.RegisterListener(this);
    }

    public void SetComplete()
    {
        isComplete = true;
        initController?.OnCmdComplete();
    }

    public abstract void Execute(MonoBehaviour mono);
}
