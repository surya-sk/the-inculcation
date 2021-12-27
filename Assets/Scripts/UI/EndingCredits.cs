using CultGame.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CultGame.UI
{
    public class EndingCredits : MonoBehaviour
    {
        public TextMeshProUGUI DisciplineText;
        public TextMeshProUGUI PersonText;
        public string[] Disciplines;
        public string[] Artists;
        public float TimeBetweenCredits;
        public SceneLoader SceneLoader;

        IEnumerator Credits()
        {
            DisciplineText.enabled = true;
            PersonText.enabled = true;
            int counter = 0;
            while (counter < Disciplines.Length)
            {
                DisciplineText.text = Disciplines[counter];
                PersonText.text = Artists[counter];
                yield return new WaitForSeconds(TimeBetweenCredits);
                counter++;
            }
            DisciplineText.enabled = false;
            PersonText.enabled = false;
            yield return new WaitForSeconds(TimeBetweenCredits);
            SceneLoader.MainMenu();
        }

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(Credits());
        }
    }
}

