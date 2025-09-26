namespace Whatwapp.MergeSolitaire.Game
{
    public static class Consts
    {
        public const int FOUNDATION_POINTS = 10;
        public const int VICTORY_POINTS = 1000;

        public const string SFX_ExtractBlock = "ExtractBlock";
        public const string SFX_MergeBlocks = "MergeBlocks";
        public const string SFX_PlayBlock = "PlayBlock";
        public const string SFX_Lost = "Lost";
        public const string SFX_Victory = "Victory";
        public const string SFX_Explosion = "Explosion";

        public const string PREFS_HIGHSCORE = "HighScore";
        public const string PREFS_LAST_SCORE = "LastScore";
        public const string PREFS_LAST_WON = "LastWon";

        public const string SCENE_MAIN_MENU = "MainMenu";
        public const string SCENE_GAME = "Game";
        public const string SCENE_END_GAME = "EndGame";
        public const string SCENE_PAUSE_MENU = "PauseMenu";

        public static readonly string[] SFX_Foundations =
            { "Foundation1", "Foundation2", "Foundation3", "Foundation4" };

        public static string GetFoundationSFX(BlockSeed seed)
        {
            var index = (int)seed;
            return SFX_Foundations[index % SFX_Foundations.Length];
        }
    }
}