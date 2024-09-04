using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Moon.Core.Base
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BaseResources : IComponent
    {
        public BaseStorageProvider BaseStorageInProvider;
        public BaseStorageProvider BaseStorageOutProvider;
        public int Speed;
        public bool IsInResourcesInfinity;
    }
}
