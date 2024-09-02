using System;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Moon.Core.Base
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BaseResourcesSystem))]
    public sealed class BaseResourcesSystem : UpdateSystem
    {
        private const float Delay = 0.5f;

        private Filter _baseFilter;
        private bool _isDelay;

        public override void OnAwake()
        {
            _baseFilter = World.Filter.With<BaseResources>().Build();

            Reset();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!_isDelay)
            {
                foreach (var entity in _baseFilter)
                {
                    ref var baseResourcesComponent = ref entity.GetComponent<BaseResources>();
                    ref var baseStorageInComponent =
                        ref baseResourcesComponent.BaseStorageInProvider.Entity.GetComponent<BaseStorage>();
                    ref var baseStorageOutComponent =
                        ref baseResourcesComponent.BaseStorageOutProvider.Entity.GetComponent<BaseStorage>();

                    if (baseResourcesComponent.IsInResourcesInfinity || baseStorageInComponent.ResourcesCount > 0)
                    {
                        SpendOldResources(ref baseResourcesComponent, ref baseStorageInComponent,
                            ref baseStorageOutComponent);
                    }
                }

                DelayBuildNewResources().Forget();
            }
        }

        private void Reset()
        {
            _isDelay = false;

            foreach (var entity in _baseFilter)
            {
                ref var baseResourcesComponent = ref entity.GetComponent<BaseResources>();
                ref var baseStorageInComponent =
                    ref baseResourcesComponent.BaseStorageInProvider.Entity.GetComponent<BaseStorage>();
                ref var baseStorageOutComponent =
                    ref baseResourcesComponent.BaseStorageOutProvider.Entity.GetComponent<BaseStorage>();

                baseStorageInComponent.ResourcesText.text = baseResourcesComponent.IsInResourcesInfinity
                    ? "∞"
                    : $"{baseStorageInComponent.ResourcesCount}";
                baseStorageOutComponent.ResourcesText.text = $"{baseStorageOutComponent.ResourcesCount}";
            }
        }

        private void SpendOldResources(ref BaseResources baseResourcesComponent, ref BaseStorage baseStorageInComponent,
            ref BaseStorage baseStorageOutComponent)
        {
            if (!baseResourcesComponent.IsInResourcesInfinity)
            {
                baseStorageInComponent.ResourcesCount =
                    Mathf.Clamp(baseStorageInComponent.ResourcesCount - baseResourcesComponent.Speed, 0,
                        baseStorageInComponent.MaxResourcesCount);
                baseStorageInComponent.ResourcesText.text = $"{baseStorageInComponent.ResourcesCount}";
            }

            if (baseStorageOutComponent.ResourcesCount < baseStorageOutComponent.MaxResourcesCount)
            {
                BuildNewResources(ref baseResourcesComponent, ref baseStorageOutComponent);
            }
        }

        private void BuildNewResources(ref BaseResources baseResourcesComponent,
            ref BaseStorage baseStorageOutComponent)
        {
            baseStorageOutComponent.ResourcesCount =
                Mathf.Clamp(baseStorageOutComponent.ResourcesCount + baseResourcesComponent.Speed, 0, 100);
            baseStorageOutComponent.ResourcesText.text = $"{baseStorageOutComponent.ResourcesCount}";
        }

        private async UniTaskVoid DelayBuildNewResources()
        {
            _isDelay = true;

            await UniTask.Delay(TimeSpan.FromSeconds(Delay));

            _isDelay = false;
        }
    }
}