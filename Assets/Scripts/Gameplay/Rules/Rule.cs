using System;
using Data;
using GameElements;
using Modules;
using UnityEngine;
using Utils;
namespace Gameplay
{
    /// <summary>
    /// Base rule class
    /// </summary>
    public abstract class Rule : MonoBehaviour, IGizmoDrawable, IResettableElement
    {
        [SerializeField]
        private GizmoSettings _GizmoSettings = null;

        public abstract GameObject MainTarget { get; }
        public abstract bool PermanentFulfill { get; }
        public abstract bool Fulfilled { get; }

        public Color TargetGizmoColor => _GizmoSettings.TargetGizmoColor;
        public TargetShapes TargetGizmoShape => _GizmoSettings.TargetGizmoShape;
        public Vector3 TargetGizmoSize => _GizmoSettings.TargetGizmoSize;
        public Vector3 TargetGizmoOffset => _GizmoSettings.TargetGizmoOffset;

        public event Action<Rule> ChangedFulfillState;

        public abstract void InitializeState(GameModule gameModule);
        public abstract void ResetState();

        /// <summary>
        /// Draw gizmo of rule
        /// </summary>
        public void DrawGizmos()
        {
            if (MainTarget == null)
                return;

            Gizmos.color = _GizmoSettings.LineColor;
            var targetPosition = MainTarget.transform.position;
            Gizmos.DrawLine(transform.position, targetPosition + _GizmoSettings.TargetGizmoOffset);
            GizmoDrawer.DrawGizmo(targetPosition, this);
        }
        protected void InvokeChangedFulfillState(Rule rule)
        {
            ChangedFulfillState?.Invoke(rule);
        }


    }

}