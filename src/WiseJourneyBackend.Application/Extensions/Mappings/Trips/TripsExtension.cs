﻿using WiseJourneyBackend.Application.Dtos.Trip;
using WiseJourneyBackend.Application.Extensions.Mappings.Places;
using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Application.Extensions.Mappings.Trips;

public static class TripExtension
{
    public static TripDto ToDto(this Trip trip)
    {
        return new TripDto(
        trip.Id,
        trip.Name);
    }

    public static TripDetailsDto ToDetailsDto(this Trip trip)
    {
        return new TripDetailsDto(
            trip.Name,
            trip.TripDays.Select(td => new TripDayDto(
               td.Day,
                td.TripPlaces.Select(tp => new TripPlaceDto(
                    tp.PlaceId,
                    tp.Place.ToDto()
                )).ToList()
            )).ToList()
        );
    }
}