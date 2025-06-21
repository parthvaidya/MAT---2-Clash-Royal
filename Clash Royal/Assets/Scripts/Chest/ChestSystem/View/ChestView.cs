using ChestSystem.Model;
using ChestSystem.Utility;
using UnityEngine;

namespace ChestSystem.View
{
    public class ChestView : MonoBehaviour, IChestObserver
    {
        //Initialize add UI and slot required to spawn the Chest
        [SerializeField] private GameObject chestUIPrefab;
        [SerializeField] private Transform chestSlotParent;

        private void Start()
        {
            var subject = ServiceLocator.Get<ChestSubject>(); //Get chest subject
            subject.Register(this); //Register the service
        }

        //Once the chest is spawned function
        public void OnChestSpawned(ChestModel model, int slotIndex)
        {
            var slot = chestSlotParent.GetChild(slotIndex); //Get slot for spawning chest
            var chestUI = Instantiate(chestUIPrefab, slot); //Instantiate chest UI at that slot

            //Ensures the prefab fills the slot properly initiazed to zero
            RectTransform rt = chestUI.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            chestUI.GetComponent<ChestUIRuntime>().Init(model);
        }
    }
}
    