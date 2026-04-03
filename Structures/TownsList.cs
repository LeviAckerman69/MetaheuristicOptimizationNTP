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

    //daj gpt-u da kaze kako napraviti kompletno immutable listu ukljucujuci i same gradove u listi
    public IReadOnlyList<Town> Get()
    {
        return Towns;
    }

    public void Register(ITownsListener listener)
    {
        Listeners.Add(listener);
    }

    private void NotifyListeners()
    {
        Listeners.ForEach(listener => listener.OnTownsChanged());
    }
}