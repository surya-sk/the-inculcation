using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CultGame.UI
{
    public class OpeningCredits : MonoBehaviour
    {
        public TextMeshProUGUI disciplineText;
        public TextMeshProUGUI personText;
        public string[] disciplines;
        public string[] artists;
        public TextMeshProUGUI title;
        public float timeBetweenCredits;

        private void Start()
        {
            title.enabled = false;
            StartCoroutine(Credits());
        }
        IEnumerator Credits()
        {
            int counter = 0;
            while(counter<disciplines.Length)
            {
                disciplineText.text = disciplines[counter];
                personText.text = artists[counter];
                yield return new WaitForSeconds(timeBetweenCredits);
                counter++;
            }
            disciplineText.enabled = false;
            personText.enabled = false;
            title.enabled = true;
            yield return new WaitForSeconds(timeBetweenCredits);
            // Load next scene here
        }
    }
}
