//Terrain Slicing & Dynamic Loading Kits v1.5 copyright © 2012 Kyle Gillen. All rights reserved. Redistribution is not allowed.
namespace DynamicLoadingKitEditors
{
    using UnityEditor;
    using UnityEngine;
    using DynamicLoadingKit;

    public static class DynamicLoadingManagerCreator
    {
        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Non-Pooling Prefab Instantiator")]
        static void CreateNonPoolingPrefabInstantiator()
        {
            CreateComponents<NonPoolingPrimaryCellObjectSubController, PrefabInstantiator>();
        }

        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Non-Pooling Async Scene Loader (4 Pro or Unity 5)")]
        static void CreateNonPoolingAsyncSceneLoader()
        {
            CreateComponents<NonPoolingPrimaryCellObjectSubController, AsyncSceneLoader>();
        }

        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Non-Pooling Scene Loader")]
        static void CreateNonPoolingSceneLoader()
        {
            CreateComponents<NonPoolingPrimaryCellObjectSubController, SceneLoader>();
        }

        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Non-Pooling Asset Bundle Loader")]
        static void CreateNonPoolingAssetBundleLoader()
        {
            CreateComponents<NonPoolingPrimaryCellObjectSubController, AssetBundleLoader>();
        }

        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Non-Pooling Async Prefab Instantior")]
        static void CreateNonPoolingAsyncPrefabInstantiator()
        {
            CreateComponents<NonPoolingPrimaryCellObjectSubController, PrefabInstantiatorAsync>();
        }

        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Pooling Prefab Instantiator")]
        static void CreatePoolingPrefabInstantiator()
        {
            CreateComponents<PoolingPrimaryCellObjectSubController, PrefabInstantiator>();
        }

        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Pooling Async Scene Loader (4 Pro or Unity 5)")]
        static void CreatePoolingAsyncSceneLoader()
        {
            CreateComponents<PoolingPrimaryCellObjectSubController, AsyncSceneLoader>();
        }

        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Pooling Scene Loader")]
        static void CreatePoolingSceneLoader()
        {
            CreateComponents<PoolingPrimaryCellObjectSubController, SceneLoader>();
        }

        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Pooling Asset Bundle Loader")]
        static void CreatePoolingAssetBundleLoader()
        {
            CreateComponents<PoolingPrimaryCellObjectSubController, AssetBundleLoader>();
        }

        [MenuItem("GameObject/Create Other/Dynamic Loading Kit/Pooling Async Prefab Instantior")]
        static void CreatePoolingAsyncPrefabInstantiator()
        {
            CreateComponents<PoolingPrimaryCellObjectSubController, PrefabInstantiatorAsync>();
        }

        static void CreateComponents<TSubController, TCellObjectLoader>()
            where TSubController : PrimaryCellObjectSubController
            where TCellObjectLoader : CellObjectLoader
        {
            GameObject manager = new GameObject("Dynamic Loading Manager");
            manager.AddComponent<ComponentManager>();
            manager.AddComponent<World>();
            manager.AddComponent<ActiveGrid>();
            manager.AddComponent<PlayerPrefsPersistentDataController>();
            manager.AddComponent<BoundaryMonitor>();
            manager.AddComponent<TSubController>();
            manager.AddComponent<TCellObjectLoader>();

            ManagerDependencyConfigurer dependencyConfigurer = new ManagerDependencyConfigurer(manager);
            dependencyConfigurer.ConfigureDependencies();
        }
    }
}