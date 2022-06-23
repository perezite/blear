using UnityEngine;

// This class contains in-code instantiable transform information
public struct TransformData
{
    // the position
    public Vector3 Position;

    // the rotation
    public Quaternion Rotation;

    // Constructor
    public TransformData(Transform trans)
    {
        Position = trans.position;
        Rotation = trans.rotation;
    }

    // Apply transform configuraton to transform
    public void ApplyTo(Transform trans)
    {
        trans.position = Position;
        trans.rotation = Rotation;
    }
}