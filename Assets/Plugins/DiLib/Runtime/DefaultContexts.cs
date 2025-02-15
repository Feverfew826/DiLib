using UnityEngine.SceneManagement;

namespace Feverfew.DiLib
{
    public class DefaultContexts
    {
        public static IDedendencyContainer ProjectContextDependencyContainer => DefaultContextsInternal.ProjectDependencyContainer;

        public static IDedendencyContainer GetSceneDependencyContainer(Scene scene) => DefaultContextsInternal.GetSceneDependencyContainer(scene);

        public static void ForceToDisposeProjectContext() => DefaultContextsInternal.ForceToDisposeProjectContext();
    }
}
