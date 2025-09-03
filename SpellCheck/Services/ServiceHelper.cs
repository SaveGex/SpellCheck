using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellCheck.Services;

public static class ServiceHelper
{
    public static T GetService<T>()
    {
        if(Current is null)
        {
            throw new Exception("Application.Current.Handler.MauiContext.Services is null");
        }
        T? service = Current.GetService<T>();
        if(service is null)
        {
            throw new Exception("service does not found in ServiceHelper");
        }
        return service;
    }

    public static IServiceProvider? Current =>
        Application.Current?.Handler?.MauiContext?.Services;
}
