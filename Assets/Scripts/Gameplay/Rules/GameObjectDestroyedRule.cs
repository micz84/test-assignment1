using GameElements.Interactable;
using Modules;
using UnityEngine;
namespace Gameplay
{
    /// <summary>
    /// Rule for item destruction. It is fulfilled when item is destoyed
    /// </summary>
    public class GameObjectDestroyedRule:Rule
    {
        [SerializeField]
        private DestructibleObject _Target = null;

        private bool _Fulfilled;
        public override bool PermanentFulfill => true;
        public override bool Fulfilled => _Fulfilled;
        public override GameObject MainTarget
        {
            get
            {
                if (_Target == null)
                    return null;
                return _Target.gameObject;
            }
        }
        public override void InitializeState(GameModule gameModule)
        {
            if(_Target != null)
                _Target.Destroyed += GameObjectOnDestroyed;
        }
        public override void ResetState()
        {
            _Fulfilled = false;
        }

        private void OnDestroy()
        {
            if(_Target != null)
                _Target.Destroyed -= GameObjectOnDestroyed;
        }
        private void GameObjectOnDestroyed()
        {
            _Fulfilled = true;
            InvokeChangedFulfillState(this);
        }

    }
}