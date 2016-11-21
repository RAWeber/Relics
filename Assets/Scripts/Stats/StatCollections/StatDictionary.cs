using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

[Serializable]
public class StatDictionary<TKey, TValue>: Dictionary<TKey, TValue> {

    public StatDictionary() : base() { }
    public StatDictionary(int capacity) : base(capacity) { }
    public StatDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
    public StatDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
    public StatDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }
    public StatDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }
    protected StatDictionary(SerializationInfo info, StreamingContext context) : base(info, context) {

    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        StatDictionary<TKey,TValue> dict2 = obj as StatDictionary<TKey, TValue>;
        if (dict2 == null) return false;
        bool equal = false;
        if (this.Count == dict2.Count) // Require equal count.
        {
            equal = true;
            foreach (var pair in this)
            {
                TValue value;
                if (dict2.TryGetValue(pair.Key, out value))
                {
                    // Require value be equal.
                    if (!value.Equals(pair.Value))
                    {
                        equal = false;
                        break;
                    }
                }
                else
                {
                    // Require key be present.
                    equal = false;
                    break;
                }
            }
        }
        return equal;
    }

    public override int GetHashCode()
    {
        if (this == null)
            return 0;
        int h = 0x14345843;//some arbitrary number
        foreach (var pair in this)
        {
            h = h + this.Comparer.GetHashCode(pair.Key);
        }
        return h;
    }
}
