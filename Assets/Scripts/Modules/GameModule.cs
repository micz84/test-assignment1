using System;
using System.Collections.Generic;
using GameElements;
using UnityEngine;
namespace Modules
{
    /// <summary>
    /// Main game module responsible for game state management, initialization and game elements updating
    /// </summary>
    public class GameModule:MonoBehaviour
    {
        [SerializeField]
        private GameObject _DynamicObjectsParent = null;
        [SerializeField]
        private Player _Player = null;
        [SerializeField]
        private GameSubModule[] _GameSubModules = null;

        private List<IResettableElement> _ResettableElements = new List<IResettableElement>();
        private List<ITickable> _TickableElements = new List<ITickable>();

        public GameStates GameState { get; private set; } = GameStates.Uninitialized;
        public IPlayer Player => _Player;
        public Transform DynamicElementsParent => _DynamicObjectsParent.transform;
        public event Action<GameStates> GameStateChanged;
        /// <summary>
        /// Application main entry point
        /// </summary>
        public void Awake()
        {
            for (var i = 0; i < _GameSubModules.Length; i++)
            {
                var module = _GameSubModules[i];
                module.Initialize(this);
                if(module is ITickable tickableElement)
                    RegisterTickableElement(tickableElement);
            }
            // Gather all resettable elements
            _DynamicObjectsParent.GetComponentsInChildren(_ResettableElements);
            // Gather all tickable elements
            var tickableElements = _DynamicObjectsParent.GetComponentsInChildren<ITickable>();
            _TickableElements.AddRange(tickableElements);

            // Initialize all resettable elements
            for (var i = 0; i < _ResettableElements.Count; i++)
                _ResettableElements[i].InitializeState(this);

            GameState = GameStates.Running;
        }

        /// <summary>
        /// Allows to register new tickable
        /// </summary>
        /// <param name="tickable"></param>
        public void RegisterTickableElement(ITickable tickable) => _TickableElements.Add(tickable);

        /// <summary>
        /// Allows to unregister new tickable
        /// </summary>
        /// <param name="tickable"></param>
        public void UnregisterTickableElement(ITickable tickable) => _TickableElements.Remove(tickable);

        public void PauseGame() => UpdateGameState(GameStates.Paused);

        public void ResumeGame() => UpdateGameState(GameStates.Running);

        public void GameOver(bool lost) => UpdateGameState(lost ? GameStates.GameOver : GameStates.GameFinished);

        /// <summary>
        /// Restart level to its initial state
        /// </summary>
        public void Restart()
        {
            for (var i = 0; i < _ResettableElements.Count; i++)
                _ResettableElements[i].ResetState();
            UpdateGameState(GameStates.Running);
        }

        private void Update()
        {
            if(GameState != GameStates.Running)
                return;
            // tick all tickable objects
            for (var i = 0; i < _TickableElements.Count; i++)
                _TickableElements[i].Tick(Time.deltaTime);
        }
        private void FixedUpdate()
        {
            if(GameState != GameStates.Running)
                return;
            // Physics should be updated only when game is running
            Physics.Simulate(Time.fixedDeltaTime);
        }

        private void UpdateGameState(GameStates state)
        {
            GameState = state;
            GameStateChanged?.Invoke(GameState);
        }
    }

    public enum GameStates
    {
        Uninitialized,
        Running,
        Paused,
        GameOver,
        GameFinished
    }
}