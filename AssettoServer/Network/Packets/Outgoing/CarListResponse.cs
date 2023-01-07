﻿using AssettoServer.Server;
using System;
using System.Collections.Generic;

namespace AssettoServer.Network.Packets.Outgoing;

public class CarListResponse : IOutgoingNetworkPacket
{
    public int PageIndex;
    public int EntryCarsCount;
    public IEnumerable<EntryCar>? EntryCars;

    public void ToWriter(ref PacketWriter writer)
    {
        if (EntryCars == null)
            throw new ArgumentNullException(nameof(EntryCars));
            
        writer.Write((byte)ACServerProtocol.CarList);
        writer.Write((byte)PageIndex);
        writer.Write((byte)EntryCarsCount);
        foreach(EntryCar car in EntryCars)
        {
            writer.Write(car.SessionId);
            writer.WriteASCIIString(car.Model);
            writer.WriteASCIIString(car.Skin);
            writer.WriteASCIIString(car.Client?.Name);
            writer.WriteASCIIString(car.Client?.Team);
            writer.WriteASCIIString(car.Client?.NationCode);
            writer.Write(car.IsSpectator);

            for (int i = 0; i < car.Status.DamageZoneLevel.Length; i++)
                writer.Write(car.Status.DamageZoneLevel[i]);
        }
    }
}
