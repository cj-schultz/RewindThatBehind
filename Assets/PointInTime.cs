using System;
using UnityEngine;

[Serializable]
public class PointInTime
{
    public Vector3 position;
    public Quaternion rotation;

    public Vector3 velocity;
    public Vector3 angularVelocity;

    public PointInTime(Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;        
    }

    public PointInTime(Vector3 _position, Quaternion _rotation, Vector3 _velocity, Vector3 _angularVelocity)
    {
        position = _position;
        rotation = _rotation;
        velocity = _velocity;
        angularVelocity = _angularVelocity;
    }
}
