using System;
using System.Collections;
using Abu.Tools.UI;
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
        
        [SerializeField] InputField inputField;
        [SerializeField] Text output;
        [SerializeField] GameObject console;
        [SerializeField] RawImage DebugImage;
        
        [SerializeField] GameObject DebugHapticMenu;

        Console Console;
        
        bool isConsoleActive = false;
        public bool IsConsoleActive
        {
            get => isConsoleActive;
            private set
            {
                isConsoleActive = value;
                console.SetActive(value);
                
                if (console.activeInHierarchy)
                    inputField.Select();
            }
        }

        IEnumerator Start()
        {
            instance = this;   
            IsConsoleActive = false;
            Console = new Console();

            yield return new WaitForEndOfFrame();

            while (true)
            {
                yield return null;
                ProcessInput();
            }
        }

        void ProcessInput()
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
                Process();
        }
        
        void Process()
        {
            output.text = output.text + inputField.text + Environment.NewLine;
            string result = Console.Process(inputField.text) + Environment.NewLine;
            output.text += result;
            inputField.text = string.Empty;
        }

        public static void ToggleDebugImage()
        {
            instance.DebugImage.gameObject.SetActive(!instance.DebugImage.gameObject.activeSelf);
        }

        public static void ToggleHapticMenu()
        {
            instance.DebugHapticMenu.gameObject.SetActive(!instance.DebugHapticMenu.gameObject.activeSelf);
        }
    }
}