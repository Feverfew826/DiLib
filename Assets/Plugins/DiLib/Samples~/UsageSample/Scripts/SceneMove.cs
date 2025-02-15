using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feverfew.DiLib.Samples.UsageSample
{
    public class SceneMove : MonoBehaviour
    {
        public void MoveToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
