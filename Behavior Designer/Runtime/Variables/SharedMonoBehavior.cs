using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedMonoBehavior : SharedVariable<SharedMonoBehavior>
    {
        public static implicit operator SharedMonoBehavior(MonoBehaviour value) { return new SharedMonoBehavior { mValue = value }; }

    }
}