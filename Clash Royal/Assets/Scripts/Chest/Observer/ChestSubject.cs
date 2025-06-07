using System.Collections.Generic;
public class ChestSubject
{
    private List<IChestObserver> observers = new(); //Maintains a list of observer

    public void Register(IChestObserver observer) => observers.Add(observer); //Add an observer to the notification list
    public void Unregister(IChestObserver observer) => observers.Remove(observer); //Remove an observer

    // Sends updates to all registered observers
    public void Notify(ChestModel model, int slotIndex)
    {
        foreach (var obs in observers)
            obs.OnChestSpawned(model, slotIndex);
    }
}
