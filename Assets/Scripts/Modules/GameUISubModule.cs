using Data;
using Data.Settings;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules
{
    /// <summary>
    /// Sub module responsible for handling Game UI
    /// </summary>
    public class GameUISubModule:GameSubModule
    {
        [SerializeField]
        private GameObject _PauseMenu = null;
        [SerializeField]
        private GameObject _GameOverMenu = null;
        [SerializeField]
        private TextMeshProUGUI _EndHeader = null;
        [SerializeField]
        private SceneLoader _SceneLoader = null;
        [SerializeField]
        private LevelsSettings _LevelsSettings = null;
        [SerializeField]
        private string _GameOverText = "Gameover";
        [SerializeField]
        private string _GameFinishedText = "Level Finished";

        private GameModule _GameModule = null;
        private Controls _Controls = null;
        public override void Initialize(GameModule gameModule)
        {
            _Controls = new Controls();
            _Controls.Menu.ResumeGame.performed += ResumePerformed;
            _Controls.Menu.Restart.performed += RestartPerformed;
            _Controls.Menu.Exit.performed += ExitPerformed;
            _GameModule = gameModule;
            _PauseMenu.gameObject.SetActive(false);
            _GameOverMenu.gameObject.SetActive(false);

            _GameModule.GameStateChanged += GameModuleOnGameStateChanged;
        }
        private void ResumePerformed(InputAction.CallbackContext obj) => Resume();

        private void RestartPerformed(InputAction.CallbackContext obj) => Restart();

        private void ExitPerformed(InputAction.CallbackContext obj) => QuitToMainMenu();


        public void Resume()
        {
            _GameModule.ResumeGame();
        }
        public void Restart()
        {
            _GameModule.Restart();
        }
        public void QuitToMainMenu()
        {
            StartCoroutine(_SceneLoader.LoadScene(_LevelsSettings.MainMenuId));
        }
        /// <summary>
        /// Handles UI state based on game state
        /// </summary>
        /// <param name="state"></param>
        private void GameModuleOnGameStateChanged(GameStates state)
        {
            _PauseMenu.gameObject.SetActive(false);
            _GameOverMenu.gameObject.SetActive(false);

            switch (state)
            {
                case GameStates.Paused:
                    _PauseMenu.gameObject.SetActive(true);
                    _Controls.Menu.Enable();
                    break;
                case GameStates.GameOver:
                case GameStates.GameFinished:
                    _GameOverMenu.gameObject.SetActive(true);
                    _EndHeader.text = state == GameStates.GameOver ? _GameOverText : _GameFinishedText;
                    _Controls.Menu.Enable();
                    break;
                case GameStates.Running:
                    _Controls.Menu.Disable();
                    break;
            }
        }

        private void OnDestroy()
        {
            if (_Controls != null)
            {
                _Controls.Menu.ResumeGame.performed += ResumePerformed;
                _Controls.Menu.Restart.performed += RestartPerformed;
                _Controls.Menu.Exit.performed += ExitPerformed;
                _Controls.Dispose();
            }
        }
    }
}