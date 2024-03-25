using GameElements;
using UnityEngine;
namespace Data.Settings
{

    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "Settings/Weapon")]
    public class WeaponSettings:ScriptableObject
    {
        [SerializeField]
        private Projectile _ProjectilePrefab = null;
        [SerializeField]
        private float _ProjectileVelocity = 10;
        [SerializeField]
        private float _Cooldown = 1;

        public Projectile ProjectilePrefab => _ProjectilePrefab;
        public float ProjectileVelocity => _ProjectileVelocity;
        public float Cooldown => _Cooldown;
    }
}