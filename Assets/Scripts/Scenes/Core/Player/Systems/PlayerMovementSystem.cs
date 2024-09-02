using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Moon.Core.Player
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PlayerMovementSystem))]
    public sealed class PlayerMovementSystem : FixedUpdateSystem
    {
        private Entity _playerEntity;

        public override void OnAwake()
        {
            _playerEntity = World.Filter.With<PlayerMovement>().With<PlayerInput>().Build().First();
        }

        public override void OnUpdate(float deltaTime)
        {
            ref var inputComponent = ref _playerEntity.GetComponent<PlayerInput>();
            ref var movementComponent = ref _playerEntity.GetComponent<PlayerMovement>();
            movementComponent.Rigidbody.velocity = inputComponent.Direction * movementComponent.Speed;
        }
    }
}