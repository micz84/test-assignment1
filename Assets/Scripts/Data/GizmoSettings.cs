using System;
using Gameplay;
using UnityEngine;
namespace Data
{
    /// <summary>
    /// Settings for class that uses GizmoDrawer
    /// </summary>
    [Serializable]
    public class GizmoSettings
    {
        public Color LineColor = Color.red;
        public Color TargetGizmoColor = Color.red;
        public TargetShapes TargetGizmoShape = TargetShapes.Cube;
        public Vector3 TargetGizmoSize = Vector3.one;
        public Vector3 TargetGizmoOffset = Vector3.zero;
    }
}