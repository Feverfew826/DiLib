using UnityEngine.SceneManagement;

namespace Feverfew.DiLib
{
    public class Containers
    {
        public static IDependencyContainer ProjectContext => ContainersInternal.ProjectContainer;

        public static void ForceToDisposeProjectContextContainer() => ContainersInternal.ForceToDisposeProjectContainer();

        public static IDependencyContainer SceneContext(Scene scene) => ContainersInternal.GetSceneContainer(scene);
    }
}
