using System.Collections;
using System.Linq;
using UnityEngine;

public class DeathPowerDown : Extra
{
    [Tooltip("Explosion to play at touch")]
    public ParticleSystem Explosion;

    protected override IEnumerator Affect()
    {
        Ball.GetAllBalls().ForEach(x => x.QueueState(Ball.State.Dying));

        Explosion.Play();
        while (Explosion.isPlaying)
        {
            yield return null;
        }

        yield return null;
    }
}
