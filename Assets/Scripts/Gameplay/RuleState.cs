using System;
using UnityEngine;
namespace Gameplay
{
    /// <summary>
    /// Rule definition class
    /// </summary>
    [Serializable]
    public class RuleState
    {
        [SerializeField]
        private Rule _Rule;

        public event Action RuleStateChanged;
        public Rule Rule => _Rule;
        public bool Fulfilled { get; private set; }

        public void Initialize()
        {
            _Rule.ChangedFulfillState += RuleOnChangedFulfillState;
            Fulfilled = false;
        }

        public void Reset()
        {
            if (_Rule.PermanentFulfill && Fulfilled)
                _Rule.ChangedFulfillState += RuleOnChangedFulfillState;
            Fulfilled = false;
        }
        private void RuleOnChangedFulfillState(Rule rule)
        {
            Fulfilled = rule.Fulfilled;
            RuleStateChanged?.Invoke();
            if (rule.PermanentFulfill)
                _Rule.ChangedFulfillState -= RuleOnChangedFulfillState;
        }

    }
}