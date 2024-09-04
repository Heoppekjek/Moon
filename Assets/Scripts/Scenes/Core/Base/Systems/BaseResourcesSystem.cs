using System;
using System.Collections.Generic;
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
                        ref baseResourcesComponent.BaseStorageInProvider.GetData();
                    ref var baseStorageOutComponent =
                        ref baseResourcesComponent.BaseStorageOutProvider.GetData();

                    if ((baseResourcesComponent.IsInResourcesInfinity ||
                         ThisCanCreateNewResources(ref baseResourcesComponent,
                             baseStorageInComponent.PortableResources)) &&
                        !ThisStorageIsFull(baseStorageOutComponent.PortableResources))
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

                int inBaseStorageResourcesCount = GetStorageResourceCount(baseStorageInComponent.PortableResources);
                int outBaseStorageResourcesCount = GetStorageResourceCount(baseStorageOutComponent.PortableResources);

                baseStorageInComponent.ResourcesText.text = baseResourcesComponent.IsInResourcesInfinity
                    ? "∞"
                    : $"{inBaseStorageResourcesCount}";
                baseStorageOutComponent.ResourcesText.text = $"{outBaseStorageResourcesCount}";
            }
        }

        private int GetStorageResourceCount(List<PortableResources> portableResources)
        {
            int baseStorageResourcesCount = 0;

            for (int i = 0; i < portableResources.Count; i++)
            {
                var baseStorageResources = portableResources[i];
                baseStorageResourcesCount += baseStorageResources.ResourceCount;
            }

            return baseStorageResourcesCount;
        }

        private bool ThisCanCreateNewResources(ref BaseResources baseResourcesComponent,
            List<PortableResources> portableResources)
        {
            int speedSpendResources = baseResourcesComponent.Speed / portableResources.Count;

            for (int i = 0; i < portableResources.Count; i++)
            {
                if (portableResources[i].ResourceCount - speedSpendResources < 0 || speedSpendResources <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ThisStorageIsFull(List<PortableResources> portableResources)
        {
            for (int i = 0; i < portableResources.Count; i++)
            {
                var portableResource = portableResources[i];
                if (portableResource.ResourceCount >= portableResource.MaxResourceCount)
                {
                    return true;
                }
            }

            return false;
        }

        private void SpendOldResources(ref BaseResources baseResourcesComponent, ref BaseStorage baseStorageInComponent,
            ref BaseStorage baseStorageOutComponent)
        {
            if (!baseResourcesComponent.IsInResourcesInfinity)
            {
                int speedSpendResources = baseResourcesComponent.Speed / baseStorageInComponent.PortableResources.Count;

                List<PortableResources> portableResourcesList = new List<PortableResources>();

                for (int i = 0; i < baseStorageInComponent.PortableResources.Count; i++)
                {
                    var baseStorageResources = baseStorageInComponent.PortableResources[i];
                    portableResourcesList.Add(new PortableResources(baseStorageResources.ResourceType,
                        baseStorageResources.ResourceCount - speedSpendResources,
                        baseStorageResources.MaxResourceCount));
                }

                baseStorageInComponent.PortableResources = new List<PortableResources>(portableResourcesList);

                baseStorageInComponent.ResourcesText.text =
                    $"{GetStorageResourceCount(baseStorageInComponent.PortableResources)}";
            }

            BuildNewResources(ref baseResourcesComponent, ref baseStorageOutComponent);
        }

        private void BuildNewResources(ref BaseResources baseResourcesComponent,
            ref BaseStorage baseStorageOutComponent)
        {
            int speedSpendResources = baseResourcesComponent.Speed / baseStorageOutComponent.PortableResources.Count;

            List<PortableResources> portableResourcesList = new List<PortableResources>();

            for (int i = 0; i < baseStorageOutComponent.PortableResources.Count; i++)
            {
                var baseStorageResources = baseStorageOutComponent.PortableResources[i];
                portableResourcesList.Add(new PortableResources(baseStorageResources.ResourceType,
                    baseStorageResources.ResourceCount + speedSpendResources, baseStorageResources.MaxResourceCount));
            }

            baseStorageOutComponent.PortableResources = new List<PortableResources>(portableResourcesList);

            baseStorageOutComponent.ResourcesText.text =
                $"{GetStorageResourceCount(baseStorageOutComponent.PortableResources)}";
        }

        private async UniTaskVoid DelayBuildNewResources()
        {
            _isDelay = true;

            await UniTask.Delay(TimeSpan.FromSeconds(Delay));

            _isDelay = false;
        }
    }
}