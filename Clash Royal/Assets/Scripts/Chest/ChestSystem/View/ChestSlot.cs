using UnityEngine;


namespace ChestSystem.View
{
   

    public class ChestSlot : MonoBehaviour
    {
        public bool IsOccupied { get; private set; }

        public void Occupy() => IsOccupied = true;
        public void Vacate() => IsOccupied = false;
    }
}