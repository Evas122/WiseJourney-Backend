﻿namespace WiseJourneyBackend.Application.Dtos.Places;

public record PlaceDto(
    string Id,
    string Name,
    string FullAddress,
    string ShortAddress,
    double Rating,
    int UserRatingTotal,
    int PriceLevel,
    string? PhotoId,
    GeometryDto Geometry,
    OpeningHourDto OpeningHour,
    List<PlaceTypeDto> PlaceTypes);