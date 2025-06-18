using System.Collections;
using UnityEngine;

namespace ChestSystem.View
{
    public class UIPopupHandler : MonoBehaviour
    {
        [SerializeField] private GameObject slotsFullMessage;
        [SerializeField] private GameObject noSlotMessage;
        [SerializeField] private GameObject notEnoughGemsPopup;
        [SerializeField] private GameObject UndoMessage;


        public void ShowSlotsFullMessage() => StartCoroutine(ShowMessage(slotsFullMessage));
        public void ShowNoSlotMessage() => StartCoroutine(ShowMessage(noSlotMessage));
        public void ShowNotEnoughGemsPopup() => StartCoroutine(ShowMessage(notEnoughGemsPopup));

        public void ShowUndoMessage() => StartCoroutine(ShowMessage(UndoMessage));

        private IEnumerator ShowMessage(GameObject message, float duration = 3f)
        {
            message.SetActive(true);
            yield return new WaitForSeconds(duration);
            message.SetActive(false);
        }
    }

}
