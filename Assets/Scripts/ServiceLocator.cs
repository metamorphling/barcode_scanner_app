using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ServiceLocator
{
    private static ServiceLocator _instance;
    private Dictionary<Type, object> _services;

    public ServiceLocator()
    {
        _services = new Dictionary<Type, object>();
        _instance = this;
    }

    public static ServiceLocator Instance
    {
        get => _instance;
    }

    public T GetService<T>()
    {
        if (_services.TryGetValue(typeof(T), out var result))
        {
            return (T)result;
        }
        else
        {
            return default(T);
        }
    }

    public void SetService<T>(T service)
    {
        Type type = typeof(T);
        if (_services.ContainsKey(type))
        {
            Debug.LogWarning($"Locator already has service {nameof(service)}, overwriting it");
        }
        _services[type] = service;
    }
}
