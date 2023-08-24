using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class LevelSequenceController : SingletoneBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickname = "Scene_World_Map";

        public Episode CurrentEpisode { get; private set; }

        public int CurrentLevel { get; private set; }

        public bool LastLevelResult { get; private set; }

        public void StartEpisode(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(MainMenuSceneNickname);
        }

        public void FinishCurrentLevel(bool success)
        {
            ResultPanelController.Instance.Show(success);
        }

        public void AdvanceLevel()
        {
            CurrentLevel++;

            if(CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
            }

            else 
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            }
        }
    }
}
