using System;
using UnityEngine;

namespace TowerDefense
{
    public class MapCompletion : SingletoneBase<MapCompletion>
    {
        public const string m_EpisodesFilename = "completion.dat";

        [Serializable]
        private class EpisodeScore
        {
            public Episode m_Episode;
            public int m_Score;
        }

        [SerializeField] private EpisodeScore[] m_CompletionData;

        private int m_TotalScore;
        public int TotalScore => m_TotalScore;

        private new void Awake()
        {
            base.Awake();

            Saver<EpisodeScore[]>.TryLoad(m_EpisodesFilename, ref m_CompletionData);

            UpdateTotalScore();
        }

        public static void SaveEpisodeResult(int result)
        {
            if (Instance != null)
            {
                foreach (var item in Instance.m_CompletionData)
                {
                    if (item.m_Episode == LevelSequenceController.Instance.CurrentEpisode)
                    {
                        if (result > item.m_Score)
                        {
                            item.m_Score = result;

                            Saver<EpisodeScore[]>.Save(m_EpisodesFilename, Instance.m_CompletionData);
                        }
                    }
                }
            }

            else
            {
                Debug.Log("Episode complete with score " + result);
            }
        }

        public void UpdateTotalScore()
        {
            m_TotalScore = 0;

            foreach (var episodeScore in m_CompletionData)
            {
                m_TotalScore += episodeScore.m_Score;
            }
        }

        public int GetEpisodeScore(Episode episode)
        {
            foreach(var data in m_CompletionData)
            {
                if (data.m_Episode == episode)
                {
                    return data.m_Score;
                }
            }

            return 0;
        }
    }
}