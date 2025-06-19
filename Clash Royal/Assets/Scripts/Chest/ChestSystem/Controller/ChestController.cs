using System;
using System.Collections.Generic;
using UnityEngine;
using ChestSystem.Model;
using ChestSystem.View;
using ChestSystem.Utility;


namespace ChestSystem.Controller
{
    public class ChestController : MonoBehaviour
    {
        //Initialize necessary scripts
        [SerializeField] private ChestDataSO[] chestTypes;
        [SerializeField] private Transform chestSlotParent;
        [SerializeField] private PlayerUI playerUI;
        [SerializeField] private UIPopupHandler popupHandler;

        private List<ChestModel> chests = new(); //Track spawned chest
        private ChestSubject subject;
        private int maxSlots = 4; //Spawn max 4 chests
        private HashSet<string> spawnedChestNames = new(); //Track which chest types have been spawned
        private Stack<ICommand> commandStack = new();

        public int playerGems = 100;

        void Awake()
        {
            subject = new ChestSubject(); //Initializes the ChestSubject which will notify observers
            ServiceLocator.Register(subject); //Registers it in a global ServiceLocator
        }

        //Spawn chest
        public void SpawnChest()
        {
            SoundManager.Instance.Play(Sounds.ButtonClick);

            if (chests.Count >= maxSlots)
            {
                SoundManager.Instance.Play(Sounds.Warning);
                popupHandler.ShowSlotsFullMessage();
                return;
            }

            var emptySlotIndex = GetEmptySlotIndex();
            if (emptySlotIndex == -1)
            {
                SoundManager.Instance.Play(Sounds.Warning);             
                popupHandler.ShowNoSlotMessage();
                return;
            }

            ChestDataSO selectedChest;

            // Phase 1: Spawn each unique chest until all are used
            if (spawnedChestNames.Count < chestTypes.Length)
            {
                List<ChestDataSO> unspawned = new(); // Get unspawned chest types
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
                // Phase 2: Use fully random chests once all unique chests have appeared
                selectedChest = chestTypes[UnityEngine.Random.Range(0, chestTypes.Length)];
            }

            //Creates a new ChestModel and generates coin/gem rewards randomly
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

                // If child is NOT a chest prefab 
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

        //Remove chest
        public void RemoveChest(ChestModel chest)
        {
            if (chests.Contains(chest))
            {
                chests.Remove(chest);
            }
        }

        //Check if any chest is unlocking
        public bool IsAnyChestUnlocking()
        {
            return chests.Exists(c => c.chestState == ChestState.Unlocking);
        }

        //Start the unlock timer
        public void StartUnlockTimer(ChestModel chest)
        {
            chest.chestState = ChestState.Unlocking;
            chest.unlockStartTime = System.DateTime.Now;
        }

        //Try to unlock with chest
        public void TryUnlockWithGems(ChestModel chest)
        {
            var cmd = new UnlockWithGemsCommand(chest, this, playerUI , popupHandler);
            cmd.Execute();
            commandStack.Push(cmd);
        }

        public void UndoLastCommand()
        {
            SoundManager.Instance.Play(Sounds.ButtonClick);

            //Undo the last gem unlock action
            if (commandStack.Count > 0)
            {
                ICommand lastCommand = commandStack.Pop();
                lastCommand.Undo();

                foreach (Transform slot in chestSlotParent)
                {
                    var chestUI = slot.GetComponentInChildren<ChestUIRuntime>();
                    if (chestUI != null && chestUI.Model != null)  // <-- check if model exists
                    {
                        popupHandler.ShowUndoMessage();
                        chestUI.RefreshtheUI();
                    }
                }
            }
            else
            {
                Debug.Log("No command to undo");
            }
        }

    }
}
   