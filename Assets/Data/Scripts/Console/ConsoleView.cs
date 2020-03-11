using System;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Console
{
    public class ConsoleView : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Text output;
        [SerializeField] private GameObject console;

        private Console Console;
        
        private bool _isConsoleActive = false;
        public bool IsConsoleActive
        {
            get
            {
                return _isConsoleActive;
            }
            private set
            {
                _isConsoleActive = value;
                console.SetActive(value);
            }
        }

        private void Start()
        {
            IsConsoleActive = false;
            Console = new Console();
        }

        private void Update()
        {
            if (Input.touchCount > 2 || Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F))
                IsConsoleActive = !IsConsoleActive;

            if (!IsConsoleActive)
                return;
            
            if (!inputField.gameObject.activeInHierarchy || !output.gameObject.activeInHierarchy) return;
            
            if (Input.GetKey(KeyCode.Return) && !String.IsNullOrWhiteSpace(inputField.text))
            {
                Process();
            }
        }

        public void Process()
        {
            output.text = inputField.text + Environment.NewLine;
            output.text += Console.Process(inputField.text) + Environment.NewLine;
            inputField.text = "";
        }
    }
}