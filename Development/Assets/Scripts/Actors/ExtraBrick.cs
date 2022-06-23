using System.Linq;

public class ExtraBrick : Brick
{
    private static readonly string DebrisLabel = "Debris";
    private static readonly string ColorLabel = "Violet";

    protected override int GetInitialHitpoints(string spriteName)
    {
        return 1;
    }

    protected override string GetSpriteName()
    {
        return BrickSprites.Single().name;
    }

    protected override string GetDebrisMaterialName()
    {
        return ColorLabel + DebrisLabel;
    }
}
