
//define a class to hold string constants for the game
using UnityEngine;

public static class StringConstant
{

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

    public static class AnimationState
    {

        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int MiniHurt = Animator.StringToHash("MiniHurt");
        public static readonly int BigHurt = Animator.StringToHash("BigHurt");

    }

}
