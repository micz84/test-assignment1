using Modules;
using UnityEngine;
namespace GameElements
{
    /// <summary>
    /// Component used to add zones that kill player
    /// </summary>
    public class KillZone:MonoBehaviour,IResettableElement
    {
        private GameModule _GameModule = null;
        public void InitializeState(GameModule gameModule)
        {
            _GameModule = gameModule;
        }
        public void ResetState()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if(player != null)
                _GameModule.GameOver(true);
        }
    }
}