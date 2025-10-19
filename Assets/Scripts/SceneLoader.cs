using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] InputAction menuSceneInputAction;

    public static SceneLoader Instance;

    void Start()
    {
        menuSceneInputAction.Enable();
        menuSceneInputAction.performed += LoadMenuSceneFromInputAction;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void LoadMenuSceneFromInputAction(InputAction.CallbackContext context)
    {
        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadScene(int value)
    {
        SceneManager.LoadScene(value);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}