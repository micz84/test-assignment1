using UnityEngine;
namespace Gameplay
{
    public interface IGizmoDrawable
    {
        Color TargetGizmoColor { get; }

        TargetShapes TargetGizmoShape { get; }

        Vector3 TargetGizmoSize { get; }

        Vector3 TargetGizmoOffset { get; }
    }

    public enum TargetShapes
    {
        Cube,
        WireCube,
        Sphere,
        WireSphere
    }
}