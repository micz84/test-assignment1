using System;
using GameElements;

namespace Data
{
    /// <summary>
    /// Settings used by class using Waypoints provider for movement
    /// </summary>
    [Serializable]
    public class WaypointsSettings
    {
        public WaypointsProvider WaypointsProvider = null;
        public int StartingPosition = 0;
        public bool StartingDirectionForward = true;
    }
}