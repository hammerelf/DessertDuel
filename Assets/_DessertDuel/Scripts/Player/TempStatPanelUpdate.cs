//Created by: Ryan King

using UnityEngine;
using TMPro;

namespace HammerElf.Games.DessertDuel
{
    public class TempStatPanelUpdate : MonoBehaviour
    {
        public TextMeshProUGUI moneyText;
        public TextMeshProUGUI winsText;
        public TextMeshProUGUI livesText;
        public TextMeshProUGUI roundText;

        private void Update()
        {
            moneyText.text = GameManager.Instance.currentPlayerState.money.ToString();
            winsText.text = GameManager.Instance.currentPlayerState.wins.ToString();
            livesText.text = GameManager.Instance.currentPlayerState.lives.ToString();
            roundText.text = GameManager.Instance.currentPlayerState.currentRound.ToString();
        }
    }
}
