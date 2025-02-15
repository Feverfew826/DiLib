using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feverfew.DiLib
{
    internal class ContainersInternal
    {
        internal static DiContext ProjectContainer => _projectContainer;

        internal static DiContext GetSceneContainer(Scene scene)
        {
            if (_sceneContainers.TryGetValue(scene, out var context))
            {
                return context;
            }
            else
            {
                var newContext = _projectContainer.Child(new SceneChild(scene));
                _sceneContainers.Add(scene, newContext);
                return newContext;
            }
        }

        internal static void ForceToDisposeProjectContainer()
        {
            _projectContainer.DisposeInternal();
            _projectContainer = new("Project");
        }


        private static DiContext _projectContainer = new("Project");

        private static Dictionary<Scene, DiContext> _sceneContainers = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnFirstSceneAwake()
        {
            SceneManager.sceneUnloaded += RemoveSceneContainer;
        }

        private static void RemoveSceneContainer(Scene scene)
        {
            if (_sceneContainers.TryGetValue(scene, out var context))
            {
                context.DisposeInternal();
                _sceneContainers.Remove(scene);
            }
        }

        private struct SceneChild
        {
            private string _sceneName;
            private int _sceneHashCode;

            public SceneChild(Scene scene)
            {
                _sceneName = scene.name;
                _sceneHashCode = scene.GetHashCode();
            }

            public override string ToString()
            {
                return _sceneName + _sceneHashCode;
            }
        }
    }
}

