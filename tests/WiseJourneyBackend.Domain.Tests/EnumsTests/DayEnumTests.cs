using WiseJourneyBackend.Domain.Enums;

namespace WiseJourneyBackend.Domain.Tests.EnumsTests;

public class DayEnumTests
{
    private readonly Day _monday;
    private readonly Day _friday;
    private readonly Day _sunday;

    public DayEnumTests()
    {
        _monday = Day.Monday;
        _friday = Day.Friday;
        _sunday = Day.Sunday;
    }

    [Fact]
    public void Day_ShouldHaveCorrectIntegerValues()
    {
        Assert.Equal(0, (int)_monday);
        Assert.Equal(4, (int)_friday);
        Assert.Equal(6, (int)_sunday);
    }

    [Fact]
    public void Day_ShouldBeEnumType()
    {
        Assert.IsType<Day>(_monday);
        Assert.IsType<Day>(_friday);
        Assert.IsType<Day>(_sunday);
    }

    [Fact]
    public void Day_ShouldContainAllDays()
    {
        var allDays = Enum.GetValues(typeof(Day)).Cast<Day>().ToList();
        Assert.Equal(7, allDays.Count);
        Assert.Contains(Day.Monday, allDays);
        Assert.Contains(Day.Sunday, allDays);
    }
}