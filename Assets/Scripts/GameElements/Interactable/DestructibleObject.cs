using System;
using UnityEngine;
namespace GameElements.Interactable
{
    /// <summary>
    /// Item that can be destroyed. Used by Item destroyed rule.
    /// </summary>
    public class DestructibleObject:MonoBehaviour
    {
        public event Action Destroyed;

        public void NotifyDestroyed() => Destroyed?.Invoke();

    }
}