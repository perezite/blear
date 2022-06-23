using System.Collections;

using UnityEngine;

public class PaddlePropulsionEffect : MonoBehaviour
{
    [Tooltip("Determines whether the effect is attached to the left paddle propulsion")]
    public bool IsLeftPropulsion;

    [Tooltip("Minimal emission rate for the propulsion")]
    public float MinimalEmissionRate;

    [Tooltip("Rate of the particle emission")]
    public float ParticleEmissionRate;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        // determine whether propulsion is moving left and it is active
        Vector3 currentVelocity = GetComponent<Velocimeter>().CurrentVelocity;
        bool isMovingLeft = currentVelocity.x < 0f;
        bool isPropulsionActive = (IsLeftPropulsion && !isMovingLeft) || (!IsLeftPropulsion && isMovingLeft);

        // adjust particle system emission rate
        if (isPropulsionActive)
        {
            float emissionRate = Mathf.Clamp(Mathf.Abs(currentVelocity.x) * ParticleEmissionRate, MinimalEmissionRate, Mathf.Infinity);
            GetComponent<ParticleSystem>().emissionRate = emissionRate;
        }
        else
        {
            GetComponent<ParticleSystem>().emissionRate = MinimalEmissionRate;
        }
    }
}