namespace WiseJourneyBackend.Application.Dtos.Recommendation;

public record GooglePlacesQuery(
    List<string> PlaceTypes,
    int PriceLevel,         
    int Radius,             
    string Location,        
    string Keyword,         
    List<string> Queries);