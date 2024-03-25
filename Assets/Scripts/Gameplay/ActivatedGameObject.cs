using GameElements;
using Modules;
using UnityEngine;
namespace Gameplay
{
    /// <summary>
    /// Class used as activator target when rule is fulfilled.
    /// Game object is activated/deactivated according to _Default state
    /// </summary>
    public class ActivatedGameObject:MonoBehaviour,IResettableElement
    {
        [SerializeField]
        private bool _DefaultState = false;
        [SerializeField]
        private GameObject _Target = null;

        public void ChangeState() => _Target.SetActive(!_Target.activeInHierarchy);
        public void InitializeState(GameModule gameModule) => ResetState();
        public void ResetState() =>_Target.SetActive(_DefaultState);

    }
}