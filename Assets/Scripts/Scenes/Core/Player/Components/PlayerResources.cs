using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Moon.Core.Player
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PlayerResources : IComponent
    {
        public Dictionary<ResourceType, int> PortableResources;
        public int CommonResourceCount;
        public int MaxResourceCount;
    }
}
