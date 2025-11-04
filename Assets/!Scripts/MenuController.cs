using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void MainGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ArtScene()
    {
        SceneManager.LoadScene("ArtScene");
    }
}
