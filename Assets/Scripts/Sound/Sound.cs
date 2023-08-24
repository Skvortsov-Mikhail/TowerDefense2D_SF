namespace TowerDefense
{
    public enum Sound
    {
        ArrowProj = 0,
        ArrowHit = 1,
        MageProj = 2,
        MageHit = 3,
        Trap = 4,
        TrapHit = 5,
        EnemyDie = 6,
        LoseHealth = 7,
        PlayerWin = 8,
        PlayerLose = 9,
        BGMusic = 10
    }

    public static class SoundExtensions
    {
        public static void Play(this Sound sound)
        {
            SoundPlayer.Instance.Play(sound);
        }
    }
}