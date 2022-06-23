using System;
using UnityEngine;

// Inspector entry for a responsive sprite
[Serializable]
public class ResponsiveSpriteEntry
{
    public float AspectRatio;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2235:MarkAllNonSerializableFields", Justification = "Serialized by Unity")]
    public Sprite Sprite;
}