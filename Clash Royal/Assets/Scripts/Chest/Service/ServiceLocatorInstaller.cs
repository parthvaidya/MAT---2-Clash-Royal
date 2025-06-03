using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorInstaller : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(new ChestSubject());
    }
}
