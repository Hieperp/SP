using System;
using System.Collections.Generic;

namespace TotalBase
{
    /// <summary>
    /// In AutoMapper.dll, v3.2.1.0: this Extension is PUBLIC
    /// In New Version 5.1.1, this Extension is INTERNAL
    /// SO: I copy this Extension for myself, from this site
    /// https://github.com/paulbatum/automapper/blob/master/src/AutoMapper/Internal/EnumerableExtensions.cs
    /// </summary>
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }
    }
}
