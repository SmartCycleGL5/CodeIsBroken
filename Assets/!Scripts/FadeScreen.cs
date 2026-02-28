using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken
{
    public class FadeScreen : MonoBehaviour
    {
        private VisualElement fade;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        async void Start()
        {
            fade = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("fade");
            
            int cycles = 70;
            
            for (int i = cycles; i >= 0; i--)
            {
                await Task.Delay(1);

                fade.style.opacity = (float)i / cycles;
                
                Debug.Log(fade.style.opacity);
            }

            fade.style.opacity = 0;
        }
    }
}
