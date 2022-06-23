using System.Collections.Generic;
using UnityEngine;

// controller for boss brick
public class BossBrick : Brick
{
    private static readonly string DebrisLabel = "Debris";
    private static readonly string BossBrickLabel = "BossBrick";

    // the attached sprite renderer
    private SpriteRenderer spriteRenderer;

    // the color 
    private string theColor;

    protected override void Start()
    {
        base.Start();
    }

    protected override int GetInitialHitpoints(string spriteName)
    {
        return 16;
    }

    protected override string GetSpriteName()
    {
        var spriteNamePostfix = GetSpriteNamePostfix(GetHitpoints());
        return theColor + BossBrickLabel + spriteNamePostfix;
    }

    protected override string GetDebrisMaterialName()
    {
        return theColor + DebrisLabel;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        theColor = GetColor();
    }

    private string GetSpriteNamePostfix(int hitpoints)
    {
        var postfixes = new List<string> { LightSpriteNamePostfix, MediumSpriteNamePostfix, HeavySpriteNamePostfix, ExtraHeavySpriteNamePostfix };
        int index = Mathf.CeilToInt(hitpoints / 4f) - 1;
        return postfixes[index];
    }

    private string GetColor()
    {
        return StringHelper.UntilLastOccurence(spriteRenderer.sprite.name, BossBrickLabel);
    }
}
