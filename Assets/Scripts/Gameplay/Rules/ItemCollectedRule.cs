using GameElements.Interactable;
using Modules;
using UnityEngine;
namespace Gameplay
{
    /// <summary>
    /// Rule for item collection. It is fulfilled when item is collected
    /// </summary>
    public class ItemCollectedRule:Rule
    {
        [SerializeField]
        private CollectableItem _CollectableItem = null;
        private bool _Fulfilled;
        public override bool PermanentFulfill => true;

        public override bool Fulfilled => _Fulfilled;


        public override GameObject MainTarget
        {
            get
            {
                if (_CollectableItem == null)
                    return null;
                return _CollectableItem.gameObject;
            }
        }
        public override void InitializeState(GameModule gameModule)
        {
            if(_CollectableItem != null)
                _CollectableItem.ItemCollected += CollectableItemOnItemCollected;
        }
        public override void ResetState()
        {
            _Fulfilled = false;
        }
        private void OnDestroy()
        {
            if(_CollectableItem != null)
                _CollectableItem.ItemCollected -= CollectableItemOnItemCollected;
        }
        private void CollectableItemOnItemCollected(CollectableItem obj)
        {
            _Fulfilled = true;
            InvokeChangedFulfillState(this);
        }

    }
}