using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static Dictionary<Type, object> services = new();

    public static void Register<T>(T service) where T : class
    {
        var type = typeof(T);
        if (!services.ContainsKey(type))
            services[type] = service;
    }

    public static T Get<T>() where T : class
    {
        var type = typeof(T);
        return services[type] as T;
    }

    public static void Reset() => services.Clear();
}