using System;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Console
{
    public class ConsoleView : MonoBehaviour
    {
        public static Texture DebugTexture
        {
            set
            {
                Print("DebugImage updated...");
                instance.DebugImage.texture = value;
            }
        }  
        
        static ConsoleView instance;
        public static void Print(string log)
        {
            instance.output.text += log + Environment.NewLine + "--------" + Environment.NewLine;
            Debug.Log("[Console] " + log);
        } 
        
        [SerializeField] private InputField inputField;
        [SerializeField] private Text output;
        [SerializeField] private GameObject console;
        [SerializeField] RawImage DebugImage;

        private Console Console;
        
        private bool _isConsoleActive = false;
        public bool IsConsoleActive
        {
            get => _isConsoleActive;
            private set
            {
                _isConsoleActive = value;
                console.SetActive(value);
                
                if (console.activeInHierarchy)
                    inputField.Select();
            }
        }

        private void Start()
        {
            instance = this;   
            IsConsoleActive = false;
            Console = new Console();
        }

        private void Update()
        {
            bool mobileInput = Input.touchCount > 2 && (Input.touches[0].phase == TouchPhase.Began ||
                Input.touches[1].phase == TouchPhase.Began);

            bool computerInput = Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F); 
            
            if (mobileInput || computerInput)
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
            output.text = output.text + inputField.text + Environment.NewLine;
            string result = Console.Process(inputField.text) + Environment.NewLine;
            output.text += result;
            inputField.text = "";
        }

        public static void ToggleDebugImage()
        {
            instance.DebugImage.gameObject.SetActive(!instance.DebugImage.gameObject.activeSelf);
        }
    }
}