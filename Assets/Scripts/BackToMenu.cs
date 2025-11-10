using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    // Call this from the Button's OnClick
    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
