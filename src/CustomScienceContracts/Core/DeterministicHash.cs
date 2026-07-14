namespace CustomScienceContracts.Core
{
    /// <summary>Stable FNV-1a hashing for persisted/randomized mission state. String.GetHashCode is
    /// runtime-dependent and must never be used for save-visible seeds.</summary>
    public static class DeterministicHash
    {
        public static int Of(string value)
        {
            unchecked
            {
                uint hash = 2166136261u;
                if (value != null)
                    for (int i = 0; i < value.Length; i++)
                    {
                        hash ^= value[i];
                        hash *= 16777619u;
                    }
                return (int)(hash & 0x7fffffff);
            }
        }
    }
}
