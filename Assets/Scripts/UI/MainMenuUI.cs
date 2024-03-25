using Data.Settings;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Component responsible for handling Main Menu interactions
    /// </summary>
    public class MainMenuUI:MonoBehaviour
    {
        [SerializeField]
        private SceneLoader _SceneLoader = null;
        [SerializeField]
        private GameObject _Controls = null;

        [SerializeField]
        private LevelsSettings _LevelsSettings = null;
        public void LoadGame() => StartCoroutine(_SceneLoader.LoadScene(_LevelsSettings.GameId));
        public void Exit() => Application.Quit();

        public void ShowControls() => _Controls.SetActive(true);

        public void HideControls() => _Controls.SetActive(false);

    }
}