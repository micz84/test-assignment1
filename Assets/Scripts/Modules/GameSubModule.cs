using UnityEngine;
namespace Modules
{
    /// <summary>
    /// Game sub module for implementing specific functionality
    /// </summary>
    public abstract class GameSubModule:MonoBehaviour
    {
        /// <summary>
        /// Initializes sub module.
        /// </summary>
        /// <param name="gameModule">Main game module</param>
        public abstract void Initialize(GameModule gameModule);
    }
}