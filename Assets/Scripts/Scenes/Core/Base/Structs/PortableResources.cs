using Unity.IL2CPP.CompilerServices;

namespace Moon.Core
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PortableResources
    {
        public ResourceType ResourceType;
        public int ResourceCount;
        public int MaxResourceCount;

        public PortableResources(ResourceType resourceType, int resourceCount, int maxResourceCount)
        {
            ResourceType = resourceType;
            ResourceCount = resourceCount;
            MaxResourceCount = maxResourceCount;
        }
    }
}
