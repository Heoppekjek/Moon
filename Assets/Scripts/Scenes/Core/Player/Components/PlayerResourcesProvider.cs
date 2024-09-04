using System.Collections.Generic;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Moon.Core.Player
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class PlayerResourcesProvider : MonoProvider<PlayerResources>
    {
        private void Awake()
        {
            GetData().PortableResources ??= new Dictionary<ResourceType, int>();
        }
    }
}
