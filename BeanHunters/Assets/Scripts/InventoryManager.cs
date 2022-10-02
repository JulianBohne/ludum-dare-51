using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<string, int> counts = new Dictionary<string, int> {
        {"Components", 0 },
        {"Fuel", 0 },
        {"Ration", 0 },
        {"Scrap", 0 }
    };

    public void addItem(string name, int count)
    {
        if (counts.TryGetValue(name, out int currentValue))
        {
            counts[name] = currentValue + count;
            Debug.Log(counts[name]);
            refreshUI(name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        refreshUI();
    }

    void refreshUI()
    {
        foreach(string itemType in counts.Keys)
        {
            refreshUI(itemType);
        }
    }

    void refreshUI(string itemType)
    {
        Transform child = transform.Find(itemType);
        if (child is null)
        {
            Debug.LogWarning("Could not find item type: " + itemType);
            return;
        }
        TextMeshProUGUI textMesh = child.Find("Count").GetComponent<TextMeshProUGUI>();
        if(counts.TryGetValue(itemType, out int count))
        {
            textMesh.SetText(count.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
