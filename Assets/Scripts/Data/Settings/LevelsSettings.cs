using UnityEngine;
namespace Data.Settings
{
    [CreateAssetMenu(fileName = "LevelsSettings", menuName = "Settings/Levels")]
    public class LevelsSettings:ScriptableObject
    {
        [SerializeField]
        private int _MainMenuId = -1;
        [SerializeField]
        private int _GameId = -1;

        public int MainMenuId => _MainMenuId;
        public int GameId => _GameId;

    }
}