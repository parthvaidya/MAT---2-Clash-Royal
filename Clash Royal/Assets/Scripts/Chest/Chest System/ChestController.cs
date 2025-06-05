using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private ChestDataSO[] chestTypes;
    [SerializeField] private Transform chestSlotParent;
    [SerializeField] private PlayerUI playerUI;

    [SerializeField] private GameObject slotsFullMessage;
    [SerializeField] private GameObject noSlotMessage;

    private List<ChestModel> chests = new();
    private ChestSubject subject;
    private int maxSlots = 4;
    private HashSet<string> spawnedChestNames = new();

    public int playerGems = 100;
    private Stack<ICommand> commandStack = new();
    void Awake()
    {
        subject = new ChestSubject();
        ServiceLocator.Register(subject);
    }

    
    public void SpawnChest()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        if (chests.Count >= maxSlots)
        {
            SoundManager.Instance.Play(Sounds.Warning);
            StartCoroutine(ShowMessageTemporarily(slotsFullMessage));
            
            Debug.Log("All slots full!");
            return;
        }

        var emptySlotIndex = GetEmptySlotIndex();
        if (emptySlotIndex == -1)
        {
            SoundManager.Instance.Play(Sounds.Warning);
            StartCoroutine(ShowMessageTemporarily(noSlotMessage));
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

    private IEnumerator ShowMessageTemporarily(GameObject messageGO, float duration = 3f)
    {
        messageGO.SetActive(true);
        yield return new WaitForSeconds(duration);
        messageGO.SetActive(false);
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

            Debug.Log("Destroyed chest, checking for slot availability...");
            Debug.Log($"Slot {i} has {slot.childCount} children.");
        }
        return -1;
    }

    public void SpawnChestAgain(ChestModel chest)
    {
        if (chests.Count >= maxSlots)
        {
            Debug.Log("No empty slots available to re-spawn chest.");
            return;
        }

        var emptySlotIndex = GetEmptySlotIndex();
        if (emptySlotIndex == -1)
        {
            Debug.Log("No empty slot found.");
            return;
        }

        chests.Add(chest);
        subject.Notify(chest, emptySlotIndex);
    }

    public void PushCommand(ICommand cmd)
    {
        commandStack.Push(cmd);
    }

    public void RemoveChest(ChestModel chest)
    {
        if (chests.Contains(chest))
        {
            chests.Remove(chest);
        }
    }
    public bool IsAnyChestUnlocking()
    {
        return chests.Exists(c => c.chestState == ChestState.Unlocking);
    }

    public void StartUnlockTimer(ChestModel chest)
    {
        if (IsAnyChestUnlocking())
        {
            Debug.Log("Only one chest can be unlocking at a time!");
            return;
        }

        chest.chestState = ChestState.Unlocking;
        chest.unlockStartTime = System.DateTime.Now;
    }

    public void TryUnlockWithGems(ChestModel chest)
    {
        var cmd = new UnlockWithGemsCommand(chest, this , playerUI);
        cmd.Execute();
        commandStack.Push(cmd);
    }

    public void UndoLastCommand()
    {
        if (commandStack.Count > 0)
            commandStack.Pop().Undo();
    }
}