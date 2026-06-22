namespace MetaheuristicOptimizationNTP.Structures;

public class TownsList
{
    private List<Town> Towns { get; } = [];
    private List<ITownsListener> Listeners { get; } = [];
    public int Count => Towns.Count;

    public void Add(Town town)
    {
        Towns.Add(town);
        NotifyListeners();
    }

    public void Remove(Town town)
    {
        Towns.Remove(town);
        NotifyListeners();
    }

    public IReadOnlyList<Town> TownsView => Towns.AsReadOnly();

    public void Register(ITownsListener listener)
    {
        Listeners.Add(listener);
    }

    private void NotifyListeners()
    {
        Listeners.ForEach(listener => listener.OnTownsChanged());
    }
}