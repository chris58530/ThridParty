using Zenject;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public class IProxy : ScriptableObject
{
    [Inject] protected Listener listener;

    [Inject]
    private void Initialize()
    {
        listener.RegisterListener(this);
    }
}
