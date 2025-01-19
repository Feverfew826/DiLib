using UnityEngine;

namespace Feverfew.DiLib.Samples.UsageSample
{
    public class GetUsage : MonoBehaviour
    {
        private void Awake()
        {
            var message = DiLib.DefaultContexts.Project.Get<string>();
            Debug.Log(message);

            var sceneContext = DiLib.DefaultContexts.GetSceneContext(gameObject.scene);

            var number = sceneContext.Get<int>();
            Debug.Log(number);

            var testParameter = sceneContext.Get<TestParameter>();
            Debug.Log(testParameter.Id);
        }
    }
}
