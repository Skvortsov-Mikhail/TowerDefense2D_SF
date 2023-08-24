using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class UpgradesShop : MonoBehaviour
    {
        /*
        [Serializable] private class UpgradeSlot
        {
            public GlobalUpgrade slot;
            public UpgradeAsset asset;

            public void AssignUpgrade()
            {
                slot.SetUpgrade(asset);
            }
        }
        */

        [SerializeField] private Text m_AmountStarsText;
        [SerializeField] private GlobalUpgrade[] m_Upgrades;

        private int m_StarsAmount;

        private void Start()
        {
            foreach (var slot in m_Upgrades)
            {
                slot.Initialize();

                slot.GetComponentInChildren<Button>().onClick.AddListener(UpdateStarsAmount);
            }

            UpdateStarsAmount();

            gameObject.SetActive(false);
        }

        public void UpdateStarsAmount()
        {
            MapCompletion.Instance.UpdateTotalScore();

            m_StarsAmount = MapCompletion.Instance.TotalScore;

            m_StarsAmount -= Upgrades.GetTotalCost();

            m_AmountStarsText.text = m_StarsAmount.ToString();

            foreach (var slot in m_Upgrades)
            {
                slot.CheckCost(m_StarsAmount);
            }
        }
    }
}