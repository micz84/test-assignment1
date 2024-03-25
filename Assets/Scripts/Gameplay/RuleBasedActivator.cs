using System;
using Data;
using GameElements;
using Modules;
using UnityEngine;
using UnityEngine.Events;
using Utils;
namespace Gameplay
{
    /// <summary>
    /// Class responsible for storing gameplay rules and notifying other elements when all rules are fulfilled
    /// </summary>
    public class RuleBasedActivator:MonoBehaviour, IGizmoDrawable, IResettableElement
    {
        [SerializeField]
        [Tooltip("List of rules that need to be fulfilled to activate")]
        private RuleState[] _Rules = null;
        [SerializeField]
        [Tooltip("Actions that should be called when all rules are fulfilled")]
        private RulesFulfilledEvent _RulesFulfilled = null;
        [SerializeField]
        private GizmoSettings _GizmoSettings = null;

        public Color TargetGizmoColor => _GizmoSettings.TargetGizmoColor;
        public TargetShapes TargetGizmoShape => _GizmoSettings.TargetGizmoShape;
        public Vector3 TargetGizmoSize => _GizmoSettings.TargetGizmoSize;
        public Vector3 TargetGizmoOffset => _GizmoSettings.TargetGizmoOffset;
        public void InitializeState(GameModule gameModule)
        {
            for (var i = 0; i < _Rules.Length; i++)
            {
                var rule = _Rules[i];
                rule.RuleStateChanged += OnRuleStateChanged;
                rule.Initialize();
            }
        }
        /// <summary>
        /// Reset rules on level reset
        /// </summary>
        public void ResetState()
        {
            for (var i = 0; i < _Rules.Length; i++)
                _Rules[i].Reset();
        }
        private void OnDestroy()
        {
            for (var i = 0; i < _Rules.Length; i++)
                _Rules[i].RuleStateChanged -= OnRuleStateChanged;
        }
        /// <summary>
        ///  When rule is fulfilled check if all rules are fulfilled
        /// </summary>
        private void OnRuleStateChanged()
        {
            for (var i = 0; i < _Rules.Length; i++)
            {
                var rule = _Rules[i];
                if(!rule.Fulfilled)
                    return; // rule not fulfilled; abort
            }
            // all rules fulfilled; notify
            _RulesFulfilled.Invoke();
        }

        /// <summary>
        /// Draw gizmos for rules and connected action game object for better visualization in editor
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (_Rules != null)
            {
                for (var i = 0; i < _Rules.Length; i++)
                {
                    var ruleState = _Rules[i];
                    if (ruleState.Rule == null)
                        continue;
                    ruleState.Rule.DrawGizmos();
                }
            }
            var eventTargetsCount = _RulesFulfilled.GetPersistentEventCount();
            for (var i = 0; i < eventTargetsCount; i++)
            {
                var target = _RulesFulfilled.GetPersistentTarget(i) as Component;
                if(target == null)
                    continue;
                Gizmos.color = _GizmoSettings.LineColor;
                var targetPosition = target.transform.position;
                Gizmos.DrawLine(targetPosition, transform.position);
                GizmoDrawer.DrawGizmo(targetPosition, this);
            }
        }
    }

    [Serializable]
    public class RulesFulfilledEvent : UnityEvent
    {

    }
}