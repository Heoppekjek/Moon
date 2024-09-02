using UnityEngine;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

namespace Moon.Core.Player
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PlayerInput : IComponent
    {
        public InputActionReference Movement;
        public float Acceleration;
        public float Deacceleration;
        public Vector3 Direction;
    }
}