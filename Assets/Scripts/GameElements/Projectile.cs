using System;
using Modules;
using UnityEngine;
namespace GameElements
{
    public class Projectile:MonoBehaviour,ITickable
    {
        private Transform _ThisTransform = null;
        private float _Velocity = 0;
        public void Initialize(float velocity)
        {
            _ThisTransform = transform;
            _Velocity = velocity;
            gameObject.SetActive(true);
        }
        public void Tick(float deltaTime)
        {
            var forward = Vector3.forward * (_Velocity * deltaTime);
            _ThisTransform.position += _ThisTransform.rotation * forward;
        }
        public event Action<Projectile> ReleaseRequested;
        private void OnTriggerEnter(Collider collider)
        {
            ReleaseRequested?.Invoke(this);
        }
    }
}