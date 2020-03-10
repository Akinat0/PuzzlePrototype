using System;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Console
{
    public class ConsoleView : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Text output;

        private Console Console;
        
        private void Start()
        {
            Console = new Console();
        }

        private void Update()
        {
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