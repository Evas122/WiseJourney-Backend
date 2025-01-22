using MediatR;
using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Commands.GeneratePlaces;

public record GeneratePlacesCommand(
    string UserPreferencesText,
    string DestinationType, // aboard or local
    List<string> TravelTypes, // Relax, Adventure, Cultural, Nature, Urban, Beach, Mountain, RoadTrip
    List<string> TransportTypes, // Car, Bus, Train, Plane, Boat
    string ClimatPreference, // Cold, Hot, Mild
    string AccommodationType, // Hotel, Hostel, Camping, Airbnb, Couchsurfing
    List<string> Cuisine, // Local, International, Vegetarian, Vegan, GlutenFree
    List<string> Activities // Sightseeing, Hiking, Swimming, Shopping, Nightlife, Museums, Relaxing, Eating, Drinking
    ) : ICommand<List<PlaceDto>>;