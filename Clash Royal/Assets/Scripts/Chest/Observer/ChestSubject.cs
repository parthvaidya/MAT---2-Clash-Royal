using System.Collections.Generic;


public class ChestSubject
{
    private List<IChestObserver> observers = new();

    public void Register(IChestObserver observer) => observers.Add(observer);
    public void Unregister(IChestObserver observer) => observers.Remove(observer);
    public void Notify(ChestModel model, int slotIndex)
    {
        foreach (var obs in observers)
            obs.OnChestSpawned(model, slotIndex);
    }
}
