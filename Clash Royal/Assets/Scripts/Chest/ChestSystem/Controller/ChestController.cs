using System;
using System.Collections.Generic;
using UnityEngine;
using ChestSystem.Model;
using ChestSystem.View;
using ChestSystem.Utility;
using ChestSystem.State;


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

        private bool[] occupiedSlots;

        void Awake()
        {
            ServiceLocator.Register(this);
            subject = new ChestSubject(); //Initializes the ChestSubject which will notify observers
            ServiceLocator.Register(subject); //Registers it in a global ServiceLocator
            occupiedSlots = new bool[chestSlotParent.childCount];
        }
        public void SpawnChest()
        {
            SoundManager.Instance.Play(Sounds.ButtonClick);

            if (!CanSpawnChest())
                return;

            int emptySlotIndex = GetEmptySlotIndex();
            if (emptySlotIndex == -1)
            {
                HandleNoSlotAvailable();
                return;
            }

            ChestDataSO selectedChest = SelectChestToSpawn();
            ChestModel newChest = CreateChestModel(selectedChest);

            chests.Add(newChest);
            occupiedSlots[emptySlotIndex] = true;
            subject.Notify(newChest, emptySlotIndex);
            
        }

        private bool CanSpawnChest()
        {
            if (chests.Count >= maxSlots)
            {
                SoundManager.Instance.Play(Sounds.Warning);
                popupHandler.ShowSlotsFullMessage();
                return false;
            }
            return true;
        }

        private void HandleNoSlotAvailable()
        {
            SoundManager.Instance.Play(Sounds.Warning);
            popupHandler.ShowNoSlotMessage();
        }

        private ChestDataSO SelectChestToSpawn()
        {
            if (spawnedChestNames.Count < chestTypes.Length)
            {
                List<ChestDataSO> unspawned = new();
                foreach (var chest in chestTypes)
                {
                    if (!spawnedChestNames.Contains(chest.chestName))
                        unspawned.Add(chest);
                }

                var selected = unspawned[UnityEngine.Random.Range(0, unspawned.Count)];
                spawnedChestNames.Add(selected.chestName);
                return selected;
            }

            return chestTypes[UnityEngine.Random.Range(0, chestTypes.Length)];
        }

        private ChestModel CreateChestModel(ChestDataSO chestData)
        {
            var model = new ChestModel
            {
                chestData = chestData,
                unlockDuration = TimeSpan.FromMinutes(chestData.unlockTimeMinutes)
            };
            model.GenerateRewards();
            return model;
        }

        private int GetEmptySlotIndex()
        {
            for (int i = 0; i < occupiedSlots.Length; i++)
            {
                if (!occupiedSlots[i])
                    return i;
            }
            return -1;
        }

        public void RemoveChest(ChestModel chest)
        {
            if (chests.Contains(chest))
                chests.Remove(chest);

            if (chest.slotIndex >= 0 && chest.slotIndex < occupiedSlots.Length)
                occupiedSlots[chest.slotIndex] = false;
        }

        public bool IsAnyChestUnlocking()
        {
            return chests.Exists(c => c.chestState == ChestState.Unlocking);
        }

        public void StartUnlockTimer(ChestModel chest)
        {
            if (chest.chestState != ChestState.Locked) return;
            chest.StateMachine.SetState(new UnlockingState());   

            
        }

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
   