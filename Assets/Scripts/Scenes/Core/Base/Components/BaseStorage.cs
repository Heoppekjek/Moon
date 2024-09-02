using Scellecs.Morpeh;
using TMPro;
using Unity.IL2CPP.CompilerServices;

namespace Moon.Core.Base
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BaseStorage : IComponent
    {
        public TextMeshPro ResourcesText;
        public int ResourcesCount;
        public int MaxResourcesCount;
    }
}
