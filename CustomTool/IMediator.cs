using Zenject;
using System.Collections.Generic;

public class IMediator
{
    [Inject] protected Listener listener;
    [Inject]
    private void Initialize()
    {
        listener.RegisterListener(this);
    }

    public virtual void Register(IView view) { }
    public virtual void DeRegister(IView view)
    {

    }

}
