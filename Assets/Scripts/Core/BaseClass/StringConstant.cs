
//define a class to hold string constants for the game
using UnityEngine;

public static class StringConstant
{
    public static readonly string PREFAB_EMPTY_CELL_PATH = "Prefabs/Items/EmptyCell";
    public static readonly string GAME_SETTING_PATH = "GameSetting";
    public static class CHARACTER
    {
        public static readonly string PLAYER = "Player";
        public static readonly string ENEMY = "Enemy";
        public static readonly string NPC = "NPC";
    }
    public static class EVENT
    {
        public static readonly string LOAD_SCENE = "LoadScene";
        public static readonly string END_GAME = "EndGame";
        public static readonly string PAUSE_TIMER = "PauseTimer";
        public static readonly string UNPAUSE_TIMER = "UnpauseTimer";
        public static readonly string CHANG_SIDE = "ChangeSide";
    }

    public static class SCENE
    {
        public static readonly string MAIN_MENU = "MainMenu";
        public static readonly string PVE_MODE = "PvE Mode";
        public static readonly string PVP_MODE = "PvP Mode";
    }

    public static class ITEM_PREFAB_PATH
    {
        public static readonly string HEALTH_POTION = "Prefabs/Items/HealthPotion";
        public static readonly string MANA_POTION = "Prefabs/Items/ManaPotion";
        public static readonly string SHURIKEN = "Prefabs/Items/Shuriken";
        public static readonly string SHIELD = "Prefabs/Items/Shield";
        public static readonly string ROW_CLEAR = "Prefabs/Items/RowClearItem";
        public static readonly string COLUMN_CLEAR = "Prefabs/Items/ColumnClearItem";
        public static readonly string ALL_CLEAR = "Prefabs/Items/AllClearItem";
    }
}
