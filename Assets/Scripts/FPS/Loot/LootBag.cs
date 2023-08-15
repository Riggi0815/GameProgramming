using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootBag : MonoBehaviour {

    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    private Loot GetDroppedItem() {

        int randNumber = Random.Range(1, 101); //Number Betwen 1 - 100 (0 excluded and 100 included)
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList) {
            if (randNumber <= item.dropChance) {
                possibleItems.Add(item);
            }
        }

        if (possibleItems.Count > 0) {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        Debug.Log("kein Drop");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPos) {
        Loot droppedItem = GetDroppedItem();
        if (droppedItem != null) {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPos, Quaternion.identity);
        }
    }
    
}
