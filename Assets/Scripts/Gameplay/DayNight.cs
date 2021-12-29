using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CultGame.Saving;

namespace CultGame.Gameplay
{
    public class DayNight : MonoBehaviour, ISaveable
    {      
        public Light DirectionalLight;
        public float IntensityMultiplier;
        public Volume SkyFogVolume;
        public VolumeProfile MorningVolume;
        public VolumeProfile EveningVolume;
        public VolumeProfile NightVolume;
    
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(ChangeTimeOfDay());   
        }

        /// <summary>
        /// Decreases directional light intensity as time goes on
        /// </summary>
        /// <returns></returns>
        IEnumerator ChangeTimeOfDay()
        {
            while(DirectionalLight.intensity > 0.4f)
            {
                DirectionalLight.intensity -= IntensityMultiplier;
                UpdateSkybox();
                yield return new WaitForSeconds(80f);
            }
        }

        /// <summary>
        /// Updates the Sky and Fog Volume Profile that suits the lighting
        /// </summary>
        private void UpdateSkybox()
        {
            if (DirectionalLight.intensity > 2)
                SkyFogVolume.profile = MorningVolume;
            else if (DirectionalLight.intensity < 1.7 && DirectionalLight.intensity > 0.8)
                SkyFogVolume.profile = EveningVolume;
            else
                SkyFogVolume.profile = NightVolume;
        }

        public object CaptureState()
        {
            return DirectionalLight.intensity;
        }

        public void RestoreState(object state)
        {
            DirectionalLight.intensity = (float)state;
            UpdateSkybox();
        }
    }
}
