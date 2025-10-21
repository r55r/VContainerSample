using UnityEngine;

namespace View
{
    [RequireComponent(typeof(ParticleSystem))]
    // It looks like no way to use ITickable for factory registration by default,
    // so use regular Update or implement additional logics to handle it (second option is better)
    // In such cases nested lifetimes are potentially used, but it introduces additional overhead
    public sealed class ClickEffectView : MonoBehaviour
    {
        [SerializeField] float maxLifetime = 3;
        [SerializeField] ParticleSystem particle;

        float _lifetime;

        void Reset() =>
            particle = GetComponent<ParticleSystem>();

        public void Init(int multiplier)
        {
            var particleSystemEmission = particle.emission;
            particleSystemEmission.rateOverTimeMultiplier *= multiplier;
        }

        void Update()
        {
            _lifetime += Time.deltaTime;
            if (_lifetime > maxLifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}