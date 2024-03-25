namespace Modules
{
    /// <summary>
    /// Object that can be ticked for each simulation step (Usually once per frame)
    /// </summary>
    public interface ITickable
    {
        /// <summary>
        /// One simulation step
        /// </summary>
        /// <param name="deltaTime">delta time for simulation</param>
        void Tick(float deltaTime);
    }
}