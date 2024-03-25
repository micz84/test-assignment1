using Modules;
namespace GameElements
{
    public interface IPlayer:ITickable,IResettableElement
    {
        void Move(float dir);
        void Jump();
        void Fire();
    }
}