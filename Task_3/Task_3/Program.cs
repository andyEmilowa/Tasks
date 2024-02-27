using System;
using System.Collections.Generic;

class HashTable<TKey, TValue>
{
    private const int InitialCapacity = 16;
    private const double LoadFactorThreshold = 0.75;

    private LinkedList<KeyValuePair<TKey, TValue>>[] buckets;
    private int count;

    public HashTable()
    {
        buckets = new LinkedList<KeyValuePair<TKey, TValue>>[InitialCapacity];
        count = 0;
    }

    public void Add(TKey key, TValue value)
    {
        GrowIfNeeded();

        int hashCode = key.GetHashCode();
        int index = Math.Abs(hashCode) % buckets.Length;

        if (buckets[index] == null)
        {
            buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }

        foreach (var pair in buckets[index])
        {
            if (pair.Key.Equals(key))
            {
                throw new InvalidOperationException("Duplicate key found.");
            }
        }

        buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
        count++;
    }

    private void GrowIfNeeded()
    {
        if ((double)count / buckets.Length >= LoadFactorThreshold)
        {
            int newCapacity = buckets.Length * 2;
            var newBuckets = new LinkedList<KeyValuePair<TKey, TValue>>[newCapacity];

            foreach (var bucket in buckets)
            {
                if (bucket != null)
                {
                    foreach (var pair in bucket)
                    {
                        int newHash = pair.Key.GetHashCode() % newCapacity;
                        if (newBuckets[newHash] == null)
                        {
                            newBuckets[newHash] = new LinkedList<KeyValuePair<TKey, TValue>>();
                        }
                        newBuckets[newHash].AddLast(pair);
                    }
                }
            }

            buckets = newBuckets;
        }
    }
}

class Program
{
    static void Main()
    {
        HashTable<string, int> myHashTable = new HashTable<string, int>();

        myHashTable.Add("one", 1);
        myHashTable.Add("two", 2);
        myHashTable.Add("three", 3);
    }
}
