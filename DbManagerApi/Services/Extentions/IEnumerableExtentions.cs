using System.Collections.ObjectModel;

namespace DbManagerApi.Services.Extentions;

public static class IEnumerableExtentions
{
    public static ICollection<T> AsCollection<T>(this IEnumerable<T> source)
    {
        ICollection<T> collection = new Collection<T>();
        foreach (T item in source)
        {
            collection.Add(item);
        }
        return collection;
    }
}
