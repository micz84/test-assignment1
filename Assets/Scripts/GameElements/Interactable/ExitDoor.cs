using Modules;
using UnityEngine;
namespace GameElements
{
    /// <summary>
    /// Exit door. When player enters a trigger level is finished
    /// </summary>
    public class ExitDoor:MonoBehaviour,IResettableElement
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
            var player = other.GetComponent<IPlayer>();
            if(player != null)
                _GameModule.GameOver(false);
        }
    }
}