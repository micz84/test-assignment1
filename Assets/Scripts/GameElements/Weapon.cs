using System.Collections;
using System.Collections.Generic;
using Data.Settings;
using Modules;
using UnityEngine;
using UnityEngine.Pool;
namespace GameElements
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private WeaponSettings _Settings = null;
        [SerializeField]
        private Transform _BulletSpawnPoint = null;

        private bool _CanFire = true;
        private GameModule _GameModule;
        private ObjectPool<Projectile> _Pool;
        private List<Projectile> _ActiveProjectiles = new List<Projectile>(20);
        private GameObject _PoolGraveyard = null;

        public void Fire()
        {
            if (!_CanFire)
                return;
            var projectile = _Pool.Get();
            _GameModule.RegisterTickableElement(projectile);
            projectile.transform.parent = _GameModule.DynamicElementsParent;
            projectile.transform.SetPositionAndRotation(_BulletSpawnPoint.position, _BulletSpawnPoint.rotation);
            projectile.Initialize(_Settings.ProjectileVelocity);
            _ActiveProjectiles.Add(projectile);
            StartCoroutine(ActivateCooldown(_Settings.Cooldown));
        }
        public void Initialize(GameModule gameModule)
        {
            if (_PoolGraveyard == null)
            { // create game object as a parent for inactive projectiles
                _PoolGraveyard = new GameObject();
                #if UNITY_EDITOR
                _PoolGraveyard.name = $"Pool[{nameof(Projectile)}]";
                #endif
            }
            _GameModule = gameModule;
            _Pool = new ObjectPool<Projectile>(CreateParticle);
        }
        public void ResetWeapon()
        { // Remove all active projectiles on level restart
            for (var i = _ActiveProjectiles.Count - 1; i >=0; i--)
                ReleaseProjectile(_ActiveProjectiles[i]);
        }
        private void ReleaseProjectile(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            _GameModule.UnregisterTickableElement(projectile);
            projectile.transform.parent = _PoolGraveyard.transform;
            _Pool.Release(projectile);
            _ActiveProjectiles.Remove(projectile);
        }
        private void ProjectileOnReleaseRequested(Projectile projectile) => ReleaseProjectile(projectile);

        private IEnumerator ActivateCooldown(float time)
        {
            _CanFire = false;
            yield return new WaitForSeconds(time);
            _CanFire = true;
        }
        private Projectile CreateParticle()
        {
            var projectile = Instantiate(_Settings.ProjectilePrefab, _GameModule.DynamicElementsParent);
            projectile.ReleaseRequested += ProjectileOnReleaseRequested;
            return projectile;
        }

    }
}