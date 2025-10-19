using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public void StartButton() {
        Invoke("StartGame", 1f);
    }
    
    void StartGame()
    {
        sceneLoader.LoadScene(2);
    }
}
