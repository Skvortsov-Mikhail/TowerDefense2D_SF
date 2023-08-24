using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource
        {
            Gold,
            Mana,
            Life
        }

        public UpdateSource source = UpdateSource.Gold;

        private Text m_Text;

        private void Start()
        {
            m_Text = GetComponent<Text>();

            switch (source)
            {
                case UpdateSource.Gold:
                    TDPlayer.GoldUpdateSubscribe(UpdateText);
                    break;

                case UpdateSource.Mana:
                    TDPlayer.ManaUpdateSubscribe(UpdateText);
                    break;
                
                case UpdateSource.Life:
                    TDPlayer.LifeUpdateSubscribe(UpdateText);
                    break;
            }
        }

        private void UpdateText(int value)
        {
            m_Text.text = value.ToString();
        }

        private void OnDestroy()
        {
            TDPlayer.GoldUpdateUnSubscribe(UpdateText);
            TDPlayer.ManaUpdateUnSubscribe(UpdateText);
            TDPlayer.LifeUpdateUnSubscribe(UpdateText);
        }
    }
}
