using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Brick : MonoBehaviour
{
    public static readonly string LightSpriteNamePostfix = "Light";
    public static readonly string MediumSpriteNamePostfix = "Medium";
    public static readonly string HeavySpriteNamePostfix = "Heavy";
    public static readonly string ExtraHeavySpriteNamePostfix = "ExtraHeavy";

    // destruction pending action
    public Action<UnityEngine.Object> DestructionPending;

    [Tooltip("list of all available brick sprites")]
    public List<Sprite> BrickSprites;

    [Tooltip("list of all available debris materials")]
    public List<Material> DebrisMaterials;

    private static readonly string BrickLabel = "Brick";
    private static readonly string DebrisLabel = "Debris";

    // hitpoints
    private int hitpoints;

    // color of the brick
    private string color;

    // sprite renderer
    private SpriteRenderer spriteRenderer;

    // effects
    private BrickEffects effects;

    // brick marker effect
    private BrickMarkerEffect brickMarkerEffect;

    // is the brick destroyed
    private bool isDestructionPending = false;

    // is playing effect
    private bool isPlayingEffect = false;

    public int GetHitpoints()
    {
        return hitpoints;
    }

    // decrease the hitpoints
    public void DecreaseHitpoints()
    {
        hitpoints -= 1;
        var spriteName = GetSpriteName();
        var newSprite = BrickSprites.Single(x => x.name == spriteName);
        spriteRenderer.sprite = newSprite;
    }

    public bool IsDestructionPending()
    {
        return isDestructionPending;
    }

    protected virtual void Start()
    {
        // prepare components
        effects = GetComponent<BrickEffects>();
        brickMarkerEffect = GetComponent<BrickMarkerEffect>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // determine color and durability by naming convention
        hitpoints = GetInitialHitpoints(spriteRenderer.sprite.name);
        color = GetColor(spriteRenderer.sprite.name);

        // set the proper debris material
        SetDebrisMaterial();
    }

    protected virtual void ReactToBallCollision()
    {
    }

    protected virtual IEnumerator OnCollisionEnter2D(Collision2D coll)
    {
        if (!isPlayingEffect && coll.gameObject.tag == "Ball")
        {
            ReactToBallCollision();

            isPlayingEffect = true;

            if (hitpoints <= 1)
            {
                isDestructionPending = true;

                // call event
                DestructionPending.TryInvoke(gameObject);

                // handle effects
                brickMarkerEffect.Try(x => x.enabled = false);
                yield return effects.TryStartCoroutine(this, x => x.PlayExplosion());

                gameObject.SetActive(false);
            }
            else
            {
                // play the transmute effect
                yield return effects.TryStartCoroutine(this, x => x.PlayShrinkEffect());
                DecreaseHitpoints();
                yield return effects.TryStartCoroutine(this, x => x.PlayGrowEffect());
            }

            isPlayingEffect = false;
        }
    }

    protected virtual int GetInitialHitpoints(string spriteName)
    {
        var postfixes = new List<string> { LightSpriteNamePostfix, MediumSpriteNamePostfix, HeavySpriteNamePostfix, ExtraHeavySpriteNamePostfix };
        var postfixString = StringHelper.FromLastOccurence(spriteName, BrickLabel);
        return postfixes.IndexOf(postfixString) + 1;
    }

    protected virtual string GetSpriteName()
    {
        var spriteNamePostfix = GetSpriteNamePostfix(hitpoints);
        return color + BrickLabel + spriteNamePostfix;
    }

    protected virtual string GetDebrisMaterialName()
    {
        return color + DebrisLabel;
    }

    private string GetColor(string spriteName)
    {
        return StringHelper.UntilLastOccurence(spriteName, BrickLabel);
    }

    private string GetSpriteNamePostfix(int hitpoints)
    {
        var postfixes = new List<string> { LightSpriteNamePostfix, MediumSpriteNamePostfix, HeavySpriteNamePostfix, ExtraHeavySpriteNamePostfix };
        return postfixes[hitpoints - 1];
    }

    private void SetDebrisMaterial()
    {
        var debrisMaterialName = GetDebrisMaterialName();
        var debrisMaterial = DebrisMaterials.Single(x => x.name == debrisMaterialName);
        var particleSystems = GetComponentsInChildren<ParticleSystemRenderer>().ToList();
        particleSystems.ForEach(x => x.material = debrisMaterial);
    }
}