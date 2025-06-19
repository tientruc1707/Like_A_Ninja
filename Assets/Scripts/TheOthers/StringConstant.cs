
//define a class to hold string constants for the game
public static class StringConstant
{
    public enum SIGN
    {
        RAT,
        TIGER,
        DRAGON,
        SNAKE,
        HORSE,
        SHEEP,
        MONKEY,
        ROOSTER,
        DOG,
        PIG,
    }
    public static class CHARACTER
    {
        public static string PLAYER = "Player";
        public static string ENEMY = "Enemy";
        public static string NPC = "NPC";
    }
    public static class EVENT
    {
        public static string ON_PLAYER_DEATH = "OnPlayerDeath";
        public static string ON_GAME_START = "OnGameStart";
        public static string ON_GAME_END = "OnGameEnd";
    }
}
