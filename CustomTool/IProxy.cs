using Zenject;
using System;
using System.Collections.Generic;
using System.Reflection;
public class IProxy
{
    [Inject] protected Listener listener;

    [Inject]
    private void Initialize()
    {
        listener.RegisterListener(this);
    }
}
