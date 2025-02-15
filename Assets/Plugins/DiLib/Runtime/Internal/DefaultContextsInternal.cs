using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feverfew.DiLib
{
    internal class DefaultContextsInternal
    {
        internal static DiContext ProjectDependencyContainer => _project;

        internal static DiContext GetSceneDependencyContainer(Scene scene)
        {
            if (_sceneContexts.TryGetValue(scene, out var context))
            {
                return context;
            }
            else
            {
                var newContext = _project.GetNewChild(scene.name + scene.GetHashCode());
                _sceneContexts.Add(scene, newContext);
                return newContext;
            }
        }

        internal static void ForceToDisposeProjectContext()
        {
            _project.DisposeInternal();
            _project = new("Project");
        }


        private static DiContext _project = new("Project");

        private static Dictionary<Scene, DiContext> _sceneContexts = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnFirstSceneAwake()
        {
            SceneManager.sceneUnloaded += RemoveSceneContext;
        }

        private static void RemoveSceneContext(Scene scene)
        {
            if (_sceneContexts.TryGetValue(scene, out var context))
            {
                context.DisposeInternal();
                _sceneContexts.Remove(scene);
            }
        }
    }
}

