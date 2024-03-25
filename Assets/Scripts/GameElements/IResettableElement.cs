using Modules;

namespace GameElements
{
    /// <summary>
    /// Interface for elements that can be initialized on level load and reset on level restart;
    /// </summary>
    public interface IResettableElement
    {
        /// <summary>
        /// Called on level load. Can be used to store initial state
        /// </summary>
        /// <param name="gameModule">Main game module</param>
        void InitializeState(GameModule gameModule);
        /// <summary>
        /// Called when level is reset. Can be used to reset element to initial state.
        /// </summary>
        void ResetState();
    }
}