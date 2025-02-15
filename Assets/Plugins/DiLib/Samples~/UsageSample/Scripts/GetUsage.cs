using UnityEngine;

namespace Feverfew.DiLib.Samples.UsageSample
{
    public class GetUsage : MonoBehaviour
    {
        private void Awake()
        {
            var message = DiLib.DefaultContexts.ProjectContextDependencyContainer.Get<string>();
            Debug.Log(message);

            var sceneContext = DiLib.DefaultContexts.GetSceneDependencyContainer(gameObject.scene);

            var number = sceneContext.Get<int>();
            Debug.Log(number);

            var testParameter = sceneContext.Get<TestParameter>();
            Debug.Log(testParameter.Id);
        }
    }
}
