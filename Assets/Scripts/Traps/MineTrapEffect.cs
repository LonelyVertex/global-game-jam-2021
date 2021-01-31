using UnityEngine;
using Zenject;

public class MineTrapEffect : MonoBehaviour
{
    public class EffectFactory : MonoMemoryPool<Vector3, MineTrapEffect>
    {
        protected override void Reinitialize(Vector3 position, MineTrapEffect item)
        {
            item.Initialize(position);
        }
    }

    [Inject]
    EffectFactory effectFactory;

    [SerializeField] 
    AudioSource audioSource;
    
    private void Initialize(Vector3 position)
    {
        transform.position = position;

        audioSource.Play();
        Invoke(nameof(Despawn), 5.0f);
    }

    private void Despawn()
    {
        effectFactory.Despawn(this);
    }
}
