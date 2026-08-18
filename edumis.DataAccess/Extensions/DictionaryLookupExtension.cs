namespace edumis.DataAccess.Extensions;

public static class DictionaryLookupExtension
{
    public static TValue GetValueOrDefault<TKey, TValue>
        (this IReadOnlyDictionary<TKey, TValue> dictionary, TKey? key, TValue defaultValue = default!)
         where TKey : struct
    {
        return key.HasValue ? dictionary.GetValueOrDefault(key.Value, defaultValue) : defaultValue;
    }
}
