using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorInstaller : MonoBehaviour
{
    //Install the service locator
    private void Awake()
    {
        ServiceLocator.Register(new ChestSubject());
    }
}
