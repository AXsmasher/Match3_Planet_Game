using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void Switch(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void UpdateLevel(Level level)
    {
        GlobalSceneVariables.level = level;
    }
}
