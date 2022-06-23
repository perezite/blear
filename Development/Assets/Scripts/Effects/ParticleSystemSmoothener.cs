using UnityEngine;

// Particles systems produce sparks at sudden velocity changes of the parenting game object. This class smoothens this effect.
public class ParticleSystemSmoothener : MonoBehaviour
{
    [Tooltip("Hard limit for particle velocity magnitude")]
    public float MaxParticleSpeed = 20f;

    [Tooltip("Print the magnitude of the all-time fastest particle to console")]
    public bool ShowStats = false;

    [Tooltip("Sensitivity of velocity change smoothing")]
    public float VelocityChangeSensitivity = 0.1f;

    // magnitude of fastest particle ever
    private float alltimeMaxVelocity = 0f;

    // if smoothing was applied last frame
    private bool isSmoothing = false;

    // velocity of the parent rigid body at last update
    private Vector2 lastVelocity;

    // rigid body of the parent
    private Rigidbody2D parentRigidbody2D;

    // particles in the system
    private ParticleSystem.Particle[] particles;

    // print alltime fastest particle speed
    private void ComputeStats()
    {
        var numParticles = GetComponent<ParticleSystem>().GetParticles(particles);

        float maxVelocity = 0f;
        for (int i = 0; i < numParticles; i++)
        {
            Vector3 velocity = particles[i].velocity;
            if (velocity.magnitude > maxVelocity)
            {
                maxVelocity = velocity.magnitude;
            }
        }

        if (maxVelocity > alltimeMaxVelocity)
        {
            alltimeMaxVelocity = maxVelocity;
            Debug.Log(alltimeMaxVelocity);
        }
    }

    // Use this for initialization
    private void Start()
    {
        // get parent rigidbody info
        parentRigidbody2D = GetComponentInParent<Rigidbody2D>();
        lastVelocity = parentRigidbody2D.velocity;

        // allocate particle array
        int maxParticles = GetComponent<ParticleSystem>().maxParticles;
        particles = new ParticleSystem.Particle[maxParticles];
    }

    // Update is called once per frame
    private void Update()
    {
        // calculate velocity change
        Vector2 currentVelocity = parentRigidbody2D.velocity;
        float velocityDiff = (currentVelocity - lastVelocity).magnitude;

        // apply smoothing at sudden velocity change
        if (velocityDiff > VelocityChangeSensitivity && GetComponent<ParticleSystem>().enableEmission == true)
        {
            GetComponent<ParticleSystem>().enableEmission = false;
            isSmoothing = true;
        }
        else if (isSmoothing == true)
        {
            GetComponent<ParticleSystem>().enableEmission = true;
            isSmoothing = false;
        }

        // get particles
        var particleSystem = GetComponent<ParticleSystem>();
        var numParticlesAlive = particleSystem.GetParticles(particles);

        // slow down particles to hard speed limit
        for (int i = 0; i < numParticlesAlive; i++)
        {
            Vector3 velocity = particles[i].velocity;
            if (velocity.magnitude > MaxParticleSpeed)
            {
                particles[i].velocity = particles[i].velocity.normalized * MaxParticleSpeed;
            }
        }

        particleSystem.SetParticles(particles, numParticlesAlive);

        // show stats info
        if (ShowStats == true)
        {
            ComputeStats();
        }

        // track velocity
        lastVelocity = currentVelocity;
    }
}