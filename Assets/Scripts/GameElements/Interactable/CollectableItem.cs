using System;
using UnityEngine;
namespace GameElements.Interactable
{
    /// <summary>
    /// Item that can be collected. Used by Item called rule to validate collection.
    /// </summary>
    public class CollectableItem:MonoBehaviour
    {
        public event Action<CollectableItem> ItemCollected;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            if (player != null)
            {
                ItemCollected?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }


}