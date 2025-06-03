using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private ChestDataSO[] chestTypes;
    [SerializeField] private Transform chestSlotParent;

    private List<ChestModel> chests = new();
    private ChestSubject subject;
    private int maxSlots = 4;
    private HashSet<string> spawnedChestNames = new();
    void Awake()
    {
        subject = new ChestSubject();
        ServiceLocator.Register(subject);
    }

    //public void SpawnChest()
    //{
    //    if (chests.Count >= maxSlots)
    //    {
    //        Debug.Log("All slots full!");
    //        return;
    //    }

    //    var emptySlotIndex = GetEmptySlotIndex();
    //    if (emptySlotIndex == -1)
    //    {
    //        Debug.Log("No empty slot available!");
    //        return;
    //    }

    //    var randomChest = chestTypes[UnityEngine.Random.Range(0, chestTypes.Length)];
    //    var chest = new ChestModel
    //    {
    //        chestData = randomChest,
    //        unlockDuration = TimeSpan.FromMinutes(randomChest.unlockTimeMinutes)
    //    };
    //    chest.GenerateRewards();

    //    chests.Add(chest);
    //    subject.Notify(chest, emptySlotIndex);
    //}

    //private int GetEmptySlotIndex()
    //{
    //    for (int i = 0; i < chestSlotParent.childCount; i++)
    //    {
    //        var slot = chestSlotParent.GetChild(i);

    //        // Check if slot has only 1 child (the placeholder Image component, or the slot itself)
    //        if (slot.childCount == 0)
    //            return i;

    //        // If child is NOT a chest prefab (optional)
    //        bool hasChest = false;
    //        foreach (Transform child in slot)
    //        {
    //            if (child.CompareTag("ChestUI"))
    //            {
    //                hasChest = true;
    //                break;
    //            }
    //        }

    //        if (!hasChest)
    //            return i;
    //    }
    //    return -1;
    //}

    public void SpawnChest()
    {
        if (chests.Count >= maxSlots)
        {
            Debug.Log("All slots full!");
            return;
        }

        var emptySlotIndex = GetEmptySlotIndex();
        if (emptySlotIndex == -1)
        {
            Debug.Log("No empty slot available!");
            return;
        }

        ChestDataSO selectedChest;

        // Phase 1: Spawn each unique chest until all are used
        if (spawnedChestNames.Count < chestTypes.Length)
        {
            // Get unspawned chest types
            List<ChestDataSO> unspawned = new();
            foreach (var chest in chestTypes)
            {
                if (!spawnedChestNames.Contains(chest.chestName))
                    unspawned.Add(chest);
            }

            selectedChest = unspawned[UnityEngine.Random.Range(0, unspawned.Count)];
            spawnedChestNames.Add(selectedChest.chestName);
        }
        else
        {
            // Phase 2: Use fully random chests
            selectedChest = chestTypes[UnityEngine.Random.Range(0, chestTypes.Length)];
        }

        var chestModel = new ChestModel
        {
            chestData = selectedChest,
            unlockDuration = TimeSpan.FromMinutes(selectedChest.unlockTimeMinutes)
        };
        chestModel.GenerateRewards();

        chests.Add(chestModel);
        subject.Notify(chestModel, emptySlotIndex);
    }

    private int GetEmptySlotIndex()
    {
        for (int i = 0; i < chestSlotParent.childCount; i++)
        {
            var slot = chestSlotParent.GetChild(i);

            // Check if slot has only 1 child (the placeholder Image component, or the slot itself)
            if (slot.childCount == 0)
                return i;

            // If child is NOT a chest prefab (optional)
            bool hasChest = false;
            foreach (Transform child in slot)
            {
                if (child.CompareTag("ChestUI"))
                {
                    hasChest = true;
                    break;
                }
            }

            if (!hasChest)
                return i;
        }
        return -1;
    }
}