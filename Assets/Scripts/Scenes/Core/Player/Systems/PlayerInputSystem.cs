using Joystick;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace Moon.Core.Player
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PlayerInputSystem))]
    public sealed class PlayerInputSystem : UpdateSystem
    {
        [Inject] private FixedJoystick _fixedJoystick;
        private Entity _playerEntity;
        
        public override void OnAwake()
        {
            _playerEntity = World.Filter.With<PlayerMovement>().With<PlayerInput>().Build().First();
        }

        public override void OnUpdate(float deltaTime)
        {
            
            ref var inputComponent = ref _playerEntity.GetComponent<PlayerInput>();
            ref var movementComponent = ref _playerEntity.GetComponent<PlayerMovement>();

            if (_fixedJoystick.Direction != Vector2.zero)
            {
                inputComponent.Direction = new Vector3(_fixedJoystick.Direction.x, 0f, _fixedJoystick.Direction.y);
            }
            else
            {
                inputComponent.Direction = inputComponent.Movement.action.ReadValue<Vector3>();
            }

            float currentSpeed = movementComponent.Speed;
            
            if (inputComponent.Direction.magnitude > 0 && currentSpeed >= 0)
            {
                currentSpeed += inputComponent.Acceleration * movementComponent.MaxSpeed * Time.deltaTime;
            }
            else
            {
                currentSpeed -= inputComponent.Deacceleration * movementComponent.MaxSpeed * Time.deltaTime;
            }

            movementComponent.Speed = Mathf.Clamp(currentSpeed, 0, movementComponent.MaxSpeed);
        }
    }
}