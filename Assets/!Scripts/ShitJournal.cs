using Coding;
using UnityEngine;
using UnityEngine.UIElements;
using static UIManager;

public class ShitJournal : MonoBehaviour, IWindow
{

    public VisualTreeAsset journalAsset { get; private set; }
    public VisualTreeAsset journalElementAsset { get; private set; }
    public UIManager.Window window { get; set; }

    VisualElement journal;

    private async void Start()
    {
        if (journalAsset == null)
        {
            journalAsset = await Utility.Addressable.ReturnAdressableAsset<VisualTreeAsset>("Window/ShitJournal");
        }
        if (journalElementAsset == null)
        {
            journalElementAsset = await Utility.Addressable.ReturnAdressableAsset<VisualTreeAsset>("Window/ShitJournalElement");
        }

        journal = journalAsset.Instantiate();

        foreach (var item in transform)
        {
            
        }

        window = new Window("Journal", journal, this);
    }

    public void Close()
    {
        Destroy(this);
    }
}
