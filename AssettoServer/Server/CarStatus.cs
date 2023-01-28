﻿using System;
using System.Numerics;
using AssettoServer.Network.Packets.Outgoing;

namespace AssettoServer.Server;

public class CarStatus
{
    public float[] DamageZoneLevel { get; internal set; } = new float[5];
    public short P2PCount { get; internal set; }
    public bool P2PActive { get; internal set; }
    public bool MandatoryPit { get; internal set; }
    public string? CurrentTyreCompound { get; internal set; }

    public byte PakSequenceId { get; internal set; }
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; internal set; }
    public Vector3 Velocity { get; set; }
    public long Timestamp { get; internal set; }
    public byte[] TyreAngularSpeed { get; } = new byte[4];
    public byte SteerAngle { get; internal set; }
    public byte WheelAngle { get; internal set; }
    public ushort EngineRpm { get; internal set; }
    public byte Gear { get; internal set; }
    public CarStatusFlags StatusFlag { get; internal set; }
    public short PerformanceDelta { get; internal set; }
    public byte Gas { get; internal set; }
    public float NormalizedPosition { get; internal set; }

    public float GetRotationAngle()
    {
        float angle = (float)(Rotation.X * 180 / Math.PI);
        if (angle < 0)
            angle += 360;

        return angle;
    }

    public float GetVelocityAngle()
    {
        if (Math.Abs(Velocity.X) < 1 && Math.Abs(Velocity.Z) < 1)
            return GetRotationAngle();

        Vector3 normalizedVelocity = Vector3.Normalize(Velocity);
        float angle = (float)-(Math.Atan2(normalizedVelocity.X, normalizedVelocity.Z) * 180 / Math.PI);
        if (angle < 0)
            angle += 360;

        return angle;
    }
}
