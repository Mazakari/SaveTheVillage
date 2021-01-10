using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   public void StartNewGame()
   {
        SceneManager.LoadScene(1);
   }

    public void QuitGame()
    {
        Application.Quit();
    }
}
