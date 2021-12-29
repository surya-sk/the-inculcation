using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CultGame.Pickups
{
    /// <summary>
    /// Handles text-related pickups. Mostly for exposition.
    /// </summary>
    public class NotePickup : MonoBehaviour
    {
        public Canvas promptCanvas;
        public TextMeshProUGUI promptText;
        public Canvas noteCanvas;
        public TextMeshProUGUI noteText;
        public string text;
        private bool isReadingText;

        private void Start()
        {
            promptCanvas.enabled = false;
            noteCanvas.enabled = false;
            noteText.text = text;
            isReadingText = false;
            promptText.text = "Press B to read";
        }

        void Update()
        {
            if(promptCanvas.enabled && !isReadingText)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.B) || UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    isReadingText = true;                
                }
            }

            // Pop up note on the screen
            else if (isReadingText)
            {
                promptText.text = "Press B to exit";
                noteCanvas.enabled = true;
                Time.timeScale = 0;
                if (UnityEngine.Input.GetKeyDown(KeyCode.B) || UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    // reset values
                    isReadingText = false;
                    noteCanvas.enabled = false;
                    Time.timeScale = 1;
                    promptText.text = "Press B to read";
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                promptCanvas.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                promptCanvas.enabled = false;
            }
        }
    }

}