using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense
{
    public interface ILevelCondition
    {
        bool isCompleted { get; }
    }

    public class LevelController : SingletoneBase<LevelController>
    {
        [SerializeField] private float m_ReferenceTime;
        public float ReferenceTime => m_ReferenceTime;

        [SerializeField] private UnityEvent m_EventLevelComleted;

        private ILevelCondition[] m_Conditions;

        private bool m_IsLevelCompleted;

        private int m_LevelScore = 3;
        public int LevelScore => m_LevelScore;

        private float m_LevelTime;
        public float LevelTime => m_LevelTime;

        private void Start()
        {
            m_Conditions = GetComponentsInChildren<ILevelCondition>();

            Player.Instance.OnPlayerDeath += EndLevel;

            m_ReferenceTime += Time.time;

            m_EventLevelComleted.AddListener(() =>
            {
                StopLevelActivity();

                if (m_ReferenceTime <= Time.time)
                {
                    m_LevelScore -= 1;
                }

                MapCompletion.SaveEpisodeResult(m_LevelScore);
            });

            void LifeScoreChange(int _)
            {
                m_LevelScore -= 1;
                TDPlayer.OnLifeUpdate -= LifeScoreChange;
            }

            TDPlayer.OnLifeUpdate += LifeScoreChange;
        }

        private void Update()
        {
            if (m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;

                CheckLevelConditions();
            }
        }

        private void CheckLevelConditions()
        {
            if (m_Conditions == null || m_Conditions.Length == 0) return;

            int numCompleted = 0;

            foreach (var v in m_Conditions)
            {
                if (v.isCompleted == true)
                {
                    numCompleted++;
                }

                if (numCompleted == m_Conditions.Length)
                {
                    m_IsLevelCompleted = true;

                    m_EventLevelComleted?.Invoke();

                    LevelSequenceController.Instance?.FinishCurrentLevel(true);
                }
            }
        }

        private void StopLevelActivity()
        {
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.transform.root.GetComponent<SpaceShip>().enabled = false;
                enemy.transform.root.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            void DisableAll<T>() where T : MonoBehaviour
            {
                foreach (var obj in FindObjectsOfType<T>())
                {
                    obj.enabled = false;
                }
            }

            DisableAll<EnemyWave>();
            DisableAll<Projectile>();
            DisableAll<Tower>();
            DisableAll<NextWave>();
        }

        private void EndLevel()
        {
            StopLevelActivity();

            ResultPanelController.Instance.Show(false);
        }


        private void OnDestroy()
        {
            Player.Instance.OnPlayerDeath -= EndLevel;
        }
    }
}
