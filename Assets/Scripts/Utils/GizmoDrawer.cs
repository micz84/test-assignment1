using Gameplay;
using UnityEngine;
namespace Utils
{
    /// <summary>
    /// Helper class for drawing gizmos
    /// </summary>
    public static class GizmoDrawer
    {
        public static void DrawGizmo(Vector3 position, IGizmoDrawable drawable)
        {
            var currentColor = Gizmos.color;
            Gizmos.color = drawable.TargetGizmoColor;
            switch (drawable.TargetGizmoShape)
            {

                case TargetShapes.Cube:
                    Gizmos.DrawCube(position + drawable.TargetGizmoOffset, drawable.TargetGizmoSize);
                    break;
                case TargetShapes.WireCube:
                    Gizmos.DrawWireCube(position + drawable.TargetGizmoOffset, drawable.TargetGizmoSize);
                    break;
                case TargetShapes.Sphere:
                    Gizmos.DrawSphere(position + drawable.TargetGizmoOffset, drawable.TargetGizmoSize.x);
                    break;
                case TargetShapes.WireSphere:
                    Gizmos.DrawWireSphere(position + drawable.TargetGizmoOffset, drawable.TargetGizmoSize.x);
                    break;
            }
            Gizmos.color = currentColor;
        }

    }
}