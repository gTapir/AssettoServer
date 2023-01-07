﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AssettoServer.Network.Http.Responses;
using AssettoServer.Server;
using AssettoServer.Server.Admin;
using AssettoServer.Server.Configuration;
using AssettoServer.Server.GeoParams;
using AssettoServer.Server.OpenSlotFilters;
using AssettoServer.Server.Weather;
using Microsoft.AspNetCore.Mvc;

namespace AssettoServer.Network.Http;

[ApiController]
public class HttpController : ControllerBase
{
    private readonly ACServerConfiguration _configuration;
    private readonly CSPServerScriptProvider _serverScriptProvider;
    private readonly WeatherManager _weatherManager;
    private readonly SessionManager _sessionManager;
    private readonly EntryCarManager _entryCarManager;
    private readonly GeoParamsManager _geoParamsManager;
    private readonly CSPFeatureManager _cspFeatureManager;
    private readonly IAdminService _adminService;
    private readonly OpenSlotFilterChain _openSlotFilter;
    private readonly HttpInfoCache _cache;

    public HttpController(CSPServerScriptProvider serverScriptProvider, WeatherManager weatherManager, SessionManager sessionManager, ACServerConfiguration configuration, EntryCarManager entryCarManager, GeoParamsManager geoParamsManager, CSPFeatureManager cspFeatureManager, IAdminService adminService, OpenSlotFilterChain openSlotFilter, HttpInfoCache cache)
    {
        _serverScriptProvider = serverScriptProvider;
        _weatherManager = weatherManager;
        _sessionManager = sessionManager;
        _configuration = configuration;
        _entryCarManager = entryCarManager;
        _geoParamsManager = geoParamsManager;
        _cspFeatureManager = cspFeatureManager;
        _adminService = adminService;
        _openSlotFilter = openSlotFilter;
        _cache = cache;
    }

    [HttpGet("/INFO")]
    public InfoResponse GetInfo()
    {
        InfoResponse responseObj = new InfoResponse
        {
            Cars = _cache.Cars,
            Clients = _entryCarManager.ConnectedCars.Count,
            Country = _cache.Country,
            CPort = _configuration.Server.HttpPort,
            Durations = _cache.Durations,
            Extra = _configuration.Server.HasExtraLap,
            Inverted = _configuration.Server.InvertedGridPositions,
            Ip = _geoParamsManager.GeoParams.Ip,
            MaxClients = _configuration.Server.MaxClients,
            Name = _cache.ServerName,
            Pass = !string.IsNullOrEmpty(_configuration.Server.Password),
            Pickup = true,
            Pit = false,
            Session = _sessionManager.CurrentSession.Configuration.Id,
            Port = _configuration.Server.UdpPort,
            SessionTypes = _cache.SessionTypes,
            Timed = false,
            TimeLeft = _sessionManager.CurrentSession.TimeLeftMilliseconds / 1000,
            TimeOfDay = (int)WeatherUtils.SunAngleFromTicks(_weatherManager.CurrentDateTime.TimeOfDay.TickOfDay),
            Timestamp = _sessionManager.ServerTimeMilliseconds,
            TPort = _configuration.Server.TcpPort,
            Track = _cache.Track,
            PoweredBy = _cache.PoweredBy
        };

        return responseObj;
    }

    [HttpGet("/JSON{guid}")]
    public async Task<EntryListResponse> GetEntryList(string guid)
    {
        guid = guid.Substring(1);
        bool guidValid = ulong.TryParse(guid, out ulong ulongGuid);
        bool isAdmin = guidValid && await _adminService.IsAdminAsync(ulongGuid);

        EntryListResponse responseObj = new EntryListResponse
        {
            Cars = _entryCarManager.EntryCars.Select(ec => new EntryListResponseCar
            {
                Model = ec.Model,
                Skin = ec.Skin,
                IsEntryList = isAdmin || _openSlotFilter.IsSlotOpen(ec, ulongGuid),
                DriverName = ec.Client?.Name,
                DriverTeam = ec.Client?.Team,
                IsConnected = ec.Client != null
            }),
            Features = _cspFeatureManager.Features.Keys
        };

        return responseObj;
    }

    [HttpGet("/api/details")]
    public async Task<DetailResponse> GetDetails(string? guid)
    {
        bool guidValid = ulong.TryParse(guid, out ulong ulongGuid);
        bool isAdmin = guidValid && await _adminService.IsAdminAsync(ulongGuid);
        
        DetailResponse responseObj = new DetailResponse
        {
            Cars = _cache.Cars,
            Clients = _entryCarManager.ConnectedCars.Count,
            Country = _cache.Country,
            CPort = _configuration.Server.HttpPort,
            Durations = _cache.Durations,
            Extra = _configuration.Server.HasExtraLap,
            Inverted = _configuration.Server.InvertedGridPositions,
            Ip = _geoParamsManager.GeoParams.Ip,
            MaxClients = _configuration.Server.MaxClients,
            Name = _configuration.Server.Name,
            Pass = !string.IsNullOrEmpty(_configuration.Server.Password),
            Pickup = true,
            Pit = false,
            Session = _sessionManager.CurrentSession.Configuration.Id,
            Port = _configuration.Server.UdpPort,
            SessionTypes = _cache.SessionTypes,
            Timed = false,
            TimeLeft = _sessionManager.CurrentSession.TimeLeftMilliseconds / 1000,
            TimeOfDay = (int)WeatherUtils.SunAngleFromTicks(_weatherManager.CurrentDateTime.TimeOfDay.TickOfDay),
            Timestamp = _sessionManager.ServerTimeMilliseconds,
            TPort = _configuration.Server.TcpPort,
            Track = _cache.Track,
            Players = new DetailResponsePlayerList
            {
                Cars = _entryCarManager.EntryCars.Select(ec => new DetailResponseCar
                {
                    Model = ec.Model,
                    Skin = ec.Skin,
                    IsEntryList = isAdmin || _openSlotFilter.IsSlotOpen(ec, ulongGuid),
                    DriverName = ec.Client?.Name,
                    DriverTeam = ec.Client?.Team,
                    DriverNation = ec.Client?.NationCode,
                    IsConnected = ec.Client != null,
                    ID = ec.Client?.HashedGuid
                })
            },
            Until = DateTimeOffset.Now.ToUnixTimeSeconds() + _sessionManager.CurrentSession.TimeLeftMilliseconds / 1000,
            Content = _configuration.ContentConfiguration,
            TrackBase = _configuration.Server.Track,
            City = _geoParamsManager.GeoParams.City,
            Frequency = _configuration.Server.RefreshRateHz,
            Assists = _cache.Assists,
            WrappedPort = _configuration.Server.HttpPort,
            AmbientTemperature = _weatherManager.CurrentWeather.TemperatureAmbient,
            RoadTemperature = _weatherManager.CurrentWeather.TemperatureRoad,
            CurrentWeatherId = _weatherManager.CurrentWeather.Type.WeatherFxType == WeatherFxType.None ? _weatherManager.CurrentWeather.Type.Graphics : _weatherManager.CurrentWeather.Type.WeatherFxType.ToString(),
            WindSpeed = (int)_weatherManager.CurrentWeather.WindSpeed,
            WindDirection = _weatherManager.CurrentWeather.WindDirection,
            Description = _configuration.Extra.ServerDescription,
            Grip = _weatherManager.CurrentWeather.TrackGrip * 100,
            Features = _cspFeatureManager.Features.Keys,
            PoweredBy = _cache.PoweredBy
        };
        
        return responseObj;
    }

    [HttpGet("/api/scripts/{scriptId:int}")]
    public ActionResult<string> GetScript(int scriptId)
    {
        if (scriptId < _serverScriptProvider.Scripts.Count)
        {
            return _serverScriptProvider.Scripts[scriptId];
        }

        return NotFound();
    }
}
