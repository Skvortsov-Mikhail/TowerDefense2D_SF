using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class LevelDisplayController : MonoBehaviour
    {
        [SerializeField] private MapLevel[] levels;
        [SerializeField] private ExtraLevel[] extraLevels;

        private void Start()
        {
            var drawLevel = 0;
            var score = 1;

            while (score != 0 && drawLevel < levels.Length)
            {
                score = levels[drawLevel].Initialise();
                drawLevel++;
            }

            for (int i = drawLevel; i < levels.Length; i++)
            {
                levels[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < extraLevels.Length; i++)
            {
                extraLevels[i].TryActivate();
            }
        }
    }
}