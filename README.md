# Clash Royale-Style Chest System
-This project replicates a Clash Royale-style Chest System using Unity. 
-It implements key design patterns such as MVC, Command, Observer, State Machine, and Service Locator. 
-It simulates spawning, unlocking, and collecting chests using Gems or wait timers.

# Table of Contents
1. Gameplay
2. Features
3. Installation
4. Credits

# Gameplay:
Here, click the Start button in the Lobby, and then click the Generate Chest button once chests are spawned open them.
Based on your choice either with coins and gems or wait for the timer to open them.

**Gameplay Video:**

**Playable Link:** 

# Features
1. **Chest Spawning System**: Adds chests into available slots with type-based logic (first unique, then random).
2. **Unlock System**: Unlock chests either with time or instantly using Gems.
3. **Command Pattern**: Allows actions like "Unlock with Gems" to be undone.
4. **Observer Pattern**: UI updates automatically when a new chest is spawned.
5. **MVC Architecture**: Separates data (Model), logic (Controller), and visuals (View) for better structure and testing.
6. **Service Locator**: Provides global access to services like ChestSubject.
7. **Scriptable Objects**: Stores chest data such as rewards, unlock time, and sprite dynamically via the Unity Editor.
8. **State Machine**: Manages chest states: Locked, Unlocked, Unlocked, and Collected.
9. **Undo System**: Undo the last Gems-spending action using a stack of commands.
10. **Slot Management**: Supports 4 chest slots with logic to detect occupied or empty slots.
11. **Error Messaging**: Shows popups for full slots, no available slots, or not enough Gems.
12. **Currency System**: The player can earn and spend Coins and Gems.
13. **Random Rewards**: Chest rewards are randomized within min-max ranges.
14. **Animations & Sounds**: Uses sound feedback (clicks, warnings) for a better user experience.
15. **UI Integration**: Uses TextMeshPro for real-time coin/gem updates and chest display.

# Installation
1. Clone the Repository 
2. Open the project in Unity's Latest Version 
3. Load the Main Menu 
4. Play!!

# Credits
The credits goes to Outscal for the Project and my Mentor Shrish for his guidance


