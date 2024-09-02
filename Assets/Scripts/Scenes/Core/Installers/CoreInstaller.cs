using Joystick;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Moon.Core.Installers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class CoreInstaller : LifetimeScope
    {
        [SerializeField] private FixedJoystick _fixedJoystick;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_fixedJoystick);
        }
    }
}
