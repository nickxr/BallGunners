using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LobbyUI : MonoBehaviour
    { 
        [SerializeField] private TMP_Dropdown colorDropdown;
        [SerializeField] private TextMeshProUGUI statusText;

        public event Action<Color> SetPlayerColorEvent;

        private void Awake()
        {
            colorDropdown.onValueChanged.AddListener(OnColorChanged);
            colorDropdown.value = 0;
        }

        private void OnColorChanged(int index)
        {
            Color selectedColor = Color.white;
            switch (index)
            {
                case 0:
                    selectedColor = Color.red;
                    break;
                case 1:
                    selectedColor = Color.green;
                    break;
                case 2:
                    selectedColor = Color.blue;
                    break;
                case 3:
                    selectedColor = Color.yellow;
                    break;
            }

            SetPlayerColorEvent?.Invoke(selectedColor);
        }

        public void UpdateStatus(string message)
        {
            statusText.text = message;
        }
    }
}
