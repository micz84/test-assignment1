using Data;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Modules
{
    /// <summary>
    /// Module responsible for handling game input
    /// </summary>
    public class InputSubModule : GameSubModule, ITickable
    {
        private Controls _Controls = null;
        private GameModule _GameModule = null;
        private float _Move = 0;

        public override void Initialize(GameModule gameModule)
        {
            _Controls = new Controls();
            _Controls.Player.Move.performed += OnMovePerformed;
            _Controls.Player.Move.canceled += OnMoveCanceled;
            _Controls.Player.Jump.performed += OnJumpPerformed;
            _Controls.Player.Fire.performed += OnFirePerformed;
            _Controls.Player.Pause.performed += OnPausePerformed;
            _Controls.Player.Enable();
            _GameModule = gameModule;
            _GameModule.GameStateChanged += GameModuleOnGameStateChanged;
        }

        public void Tick(float deltaTime)
        {
            if(Mathf.Approximately(_Move, 0))
                return;
            _GameModule.Player.Move(_Move);
        }
        private void OnDestroy()
        {
            if(_GameModule != null)
                _GameModule.GameStateChanged -= GameModuleOnGameStateChanged;

            if (_Controls != null)
            {
                _Controls.Player.Move.performed -= OnMovePerformed;
                _Controls.Player.Move.canceled -= OnMoveCanceled;
                _Controls.Player.Jump.performed -= OnJumpPerformed;
                _Controls.Player.Fire.performed -= OnFirePerformed;
                _Controls.Player.Pause.performed -= OnPausePerformed;
                _Controls.Dispose();
            }
        }

        private void OnMovePerformed(InputAction.CallbackContext context) => _Move = context.ReadValue<float>();
        private void OnMoveCanceled(InputAction.CallbackContext context) => _Move = 0;
        private void OnJumpPerformed(InputAction.CallbackContext context) => _GameModule.Player.Jump();

        private void OnFirePerformed(InputAction.CallbackContext context) => _GameModule.Player.Fire();

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            if (_GameModule.GameState == GameStates.Running)
                _GameModule.PauseGame();
            else if (_GameModule.GameState == GameStates.Paused)
                _GameModule.ResumeGame();
        }
        private void GameModuleOnGameStateChanged(GameStates state)
        {
            if (state != GameStates.Running)
                _Controls.Player.Disable();
            else
                _Controls.Player.Enable();
        }

    }
}