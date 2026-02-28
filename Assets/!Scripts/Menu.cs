using System.Collections;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace CodeIsBroken
{
    public class Menu : MonoBehaviour
    {
        VisualElement root;

        [Scene, SerializeField] private string gameScene;

        private VisualElement menu;
        private VisualElement fade;
        
        void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            
            menu = root.Q<VisualElement>("Menu");
            fade = root.Q<VisualElement>("Fade");
            
            root.Q<Button>("TitleButton").clicked += StartGame;
            root.Q<Button>("Exit").clicked += StartGame;
        }

        async void StartGame()
        {
            int cycles = 70;
            
            for (int i = 0; i < cycles; i++)
            {
                await Task.Delay(1);

                fade.style.opacity = (float)i / cycles;
                menu.style.translate = new Translate(0, i * -i / 5);
            }
            
            fade.style.opacity = 1;
            
            SceneManager.LoadScene(gameScene);
        }
        
        void ExitGame()
        {
            Application.Quit();
        }
    }
}
