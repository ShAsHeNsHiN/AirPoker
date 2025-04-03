using UnityEngine.SceneManagement;

public static class Loader
{
    private static EScene targetScene;

    public static void Load(EScene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(EScene.LoadScene.ToString());
    }

    public static void LoaderCallBack()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
