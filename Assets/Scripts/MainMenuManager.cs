using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
