using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Feverfew.DiLib
{
    [DefaultExecutionOrder(-9999)]
    public class SceneInjectionManager : MonoBehaviour
    {
        private enum InjectableCollectMethod
        {
            GetComponentInRootObjects,
            GetComponentsInRootObjects,
            GetComponentsInChildrenOfRootObjects
        }

        [SerializeField] private UnityEvent<IDependencyContainer> _dependencyPrepareAction;

        [SerializeField] private InjectableCollectMethod _injectableCollectMethod;

        private void Awake()
        {
            var sceneContext = DiLib.Containers.SceneContext(gameObject.scene);

            _dependencyPrepareAction.Invoke(DiLib.Containers.SceneContext(gameObject.scene));

            var rootGameObjects = gameObject.scene.GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                if (_injectableCollectMethod == InjectableCollectMethod.GetComponentInRootObjects)
                {
                    if (rootGameObject.TryGetComponent<IInjectable>(out var injectable))
                        injectable.Inject(sceneContext);
                }
                else if (_injectableCollectMethod == InjectableCollectMethod.GetComponentsInRootObjects)
                {
                    var injectables = rootGameObject.GetComponents<IInjectable>();

                    foreach (var injectable in injectables)
                        injectable.Inject(sceneContext);
                }
                else if (_injectableCollectMethod == InjectableCollectMethod.GetComponentsInChildrenOfRootObjects)
                {
                    var injectables = rootGameObject.GetComponentsInChildren<IInjectable>();

                    foreach (var injectable in injectables)
                        injectable.Inject(sceneContext);
                }
                else
                {
                    UnityEngine.Assertions.Assert.IsTrue(false, "Not implemented injectable collection method.");
                }
            }
        }
    }
}
