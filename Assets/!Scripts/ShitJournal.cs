using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static WindowManager;

public class ShitJournal : MonoBehaviour, IWindow
{

    public static VisualTreeAsset journalAsset { get; private set; }
    public static VisualTreeAsset journalElementAsset { get; private set; }
    public Window window { get; set; }

    VisualElement windowElement;
    VisualElement journal;

    static List<BuildingSO> buildings;

    private async void Start()
    {
        if (journalAsset == null)
        {
            journalAsset = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.ShitJournal, AddressableToLoad.GameObject);
        }
        if (journalElementAsset == null)
        {
            journalElementAsset = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.ShitJournalElement, AddressableToLoad.GameObject);
        }
        buildings = await Addressable.LoadAssets<BuildingSO>("Machines");
        
        // Sort list from int
        buildings = buildings.OrderBy(so => so.sortingOrder).ToList();

        
        windowElement = journalAsset.Instantiate();
        journal = windowElement.Q<ScrollView>("Journal");

        foreach (var item in buildings)
        {
            Debug.Log(item.isUnlocked);

            if (!item.isUnlocked) continue;

            VisualElement element = journalElementAsset.Instantiate();
            journal.Add(element);

            element.Q<Label>("Name").text = item.buildingName;
            element.Q<Label>("Description").text = item.buildingDesctiption;
        }

        window = new Window("Journal", windowElement, this);
    }

    public void Close()
    {
        Destroy(this);
    }
}
