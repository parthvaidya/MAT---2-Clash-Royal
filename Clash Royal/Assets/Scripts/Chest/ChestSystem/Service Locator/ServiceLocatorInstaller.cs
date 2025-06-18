using UnityEngine;
using ChestSystem.Utility;

public class ServiceLocatorInstaller : MonoBehaviour
{
    //Install the service locator
    private void Awake()
    {
        ServiceLocator.Register(new ChestSubject()); 
    }
}
