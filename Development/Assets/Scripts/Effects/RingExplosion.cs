using UnityEngine;

public class RingExplosion : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
        Destroy(gameObject, particleSystem.startLifetime);
        hideFlags = HideFlags.HideInHierarchy;
    }
}