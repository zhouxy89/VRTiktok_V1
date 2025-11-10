using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadBaselineScene()
    {
        SceneManager.LoadScene("BaselineScene");
    }

    public void LoadConversationScene()
    {
        SceneManager.LoadScene("ConversationScene");
    }

    public void LoadExpressionOnlyScene()
    {
        SceneManager.LoadScene("ExpressionOnlyScene");
    }

    public void LoadSingleDialogScene()
    {
        SceneManager.LoadScene("SingleDialogScene");
    }
}
