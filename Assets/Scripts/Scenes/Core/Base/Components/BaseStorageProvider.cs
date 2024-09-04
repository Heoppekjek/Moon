using System.Collections.Generic;
using Moon.Core.Player;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Moon.Core.Base
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class BaseStorageProvider : MonoProvider<BaseStorage>
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerResourcesProvider playerResourcesProvider))
            {
                ref var baseStorageComponent = ref GetData();
                ref var playerResourcesComponent = ref playerResourcesProvider.GetData();

                Debug.Log("Check baseStorageComponent.IsInStorage: " + baseStorageComponent.IsInStorage);
                if (!baseStorageComponent.IsInStorage)
                {
                    AddResourcesToPlayer(ref baseStorageComponent, ref playerResourcesComponent);
                }
                else
                {
                    GetResourcesFromPlayer(ref baseStorageComponent, ref playerResourcesComponent);
                }
            }
        }

        private void AddResourcesToPlayer(ref BaseStorage baseStorageComponent,
            ref PlayerResources playerResourcesComponent)
        {
            List<PortableResources> portableResourcesList = new List<PortableResources>();

            for (int i = 0; i < baseStorageComponent.PortableResources.Count; i++)
            {
                var baseStorageResources = baseStorageComponent.PortableResources[i];

                if (!playerResourcesComponent.PortableResources.ContainsKey(baseStorageResources.ResourceType))
                {
                    playerResourcesComponent.PortableResources.Add(baseStorageResources.ResourceType, 0);
                }

                if (baseStorageResources.ResourceCount > 0 &&
                    playerResourcesComponent.CommonResourceCount < playerResourcesComponent.MaxResourceCount)
                {
                    playerResourcesComponent.PortableResources[baseStorageResources.ResourceType] +=
                        baseStorageResources.ResourceCount;
                    playerResourcesComponent.CommonResourceCount += baseStorageResources.ResourceCount;
                    baseStorageResources.ResourceCount = 0;
                }

                portableResourcesList.Add(new PortableResources(baseStorageResources.ResourceType,
                    baseStorageResources.ResourceCount, baseStorageResources.MaxResourceCount));
            }

            baseStorageComponent.PortableResources = new List<PortableResources>(portableResourcesList);
            baseStorageComponent.ResourcesText.text =
                $"{GetStorageResourceCount(baseStorageComponent.PortableResources)}";
        }

        private void GetResourcesFromPlayer(ref BaseStorage baseStorageComponent,
            ref PlayerResources playerResourcesComponent)
        {
            List<PortableResources> portableResourcesList = new List<PortableResources>();

            for (int i = 0; i < baseStorageComponent.PortableResources.Count; i++)
            {
                var baseStorageResources = baseStorageComponent.PortableResources[i];

                if (playerResourcesComponent.PortableResources.ContainsKey(baseStorageResources.ResourceType) &&
                    baseStorageResources.ResourceCount < baseStorageResources.MaxResourceCount)
                {
                    baseStorageResources.ResourceCount += playerResourcesComponent.PortableResources[baseStorageResources.ResourceType];
                    int extraResources = 0;
                        
                    if (baseStorageResources.ResourceCount > baseStorageResources.MaxResourceCount)
                    {
                        extraResources = baseStorageResources.ResourceCount - baseStorageResources.MaxResourceCount;
                        baseStorageResources.ResourceCount -= extraResources;
                    }
                    
                    playerResourcesComponent.PortableResources[baseStorageResources.ResourceType] = 0;
                    playerResourcesComponent.CommonResourceCount = 0;
                }

                portableResourcesList.Add(new PortableResources(baseStorageResources.ResourceType,
                    baseStorageResources.ResourceCount, baseStorageResources.MaxResourceCount));
            }

            baseStorageComponent.PortableResources = new List<PortableResources>(portableResourcesList);
            baseStorageComponent.ResourcesText.text =
                $"{GetStorageResourceCount(baseStorageComponent.PortableResources)}";
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
    }
}