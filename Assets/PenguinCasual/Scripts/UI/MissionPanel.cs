using UnityEngine;
using UnityEngine.UI;

namespace Penguin.UI
{
    public class MissionPanel: MonoBehaviour
    {
        [SerializeField]
        private Text _missionText;

        public void SetMission(string mission)
        {
            _missionText.text = mission;
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void OnCancel()
        {
            this.gameObject.SetActive(false);
        }
    }
}