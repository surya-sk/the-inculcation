using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CultGame.Saving;

namespace CultGame.Gameplay
{
    public class DayNight : MonoBehaviour, ISaveable
    {      
        public float IntensityMultiplier;
        public Volume SkyFogVolume;
        public VolumeProfile MorningVolume;
        public VolumeProfile EveningVolume;
        public VolumeProfile NightVolume;
        
        private Light m_DirectionalLight;
    
        // Start is called before the first frame update
        void Start()
        {
            m_DirectionalLight = GetComponent<Light>();
            StartCoroutine(ChangeTimeOfDay());   
        }

        /// <summary>
        /// Decreases directional light intensity as time goes on
        /// </summary>
        /// <returns></returns>
        IEnumerator ChangeTimeOfDay()
        {
            while(m_DirectionalLight.intensity > 0.4f)
            {
                m_DirectionalLight.intensity -= IntensityMultiplier;
                Debug.Log(m_DirectionalLight.intensity);
                UpdateSkybox();
                yield return new WaitForSeconds(40f);
            }
        }

        /// <summary>
        /// Updates the Sky and Fog Volume Profile that suits the lighting
        /// </summary>
        private void UpdateSkybox()
        {
            if (m_DirectionalLight.intensity > 1.7)
                SkyFogVolume.profile = MorningVolume;
            else if (m_DirectionalLight.intensity < 1.7 && m_DirectionalLight.intensity > 0.8)
                SkyFogVolume.profile = EveningVolume;
            else
                SkyFogVolume.profile = NightVolume;
        }

        public object CaptureState()
        {
            return m_DirectionalLight.intensity;
        }

        public void RestoreState(object state)
        {
            if(m_DirectionalLight == null)
            {
                m_DirectionalLight = GetComponent<Light>();
            }
            m_DirectionalLight.intensity = (float)state;
            UpdateSkybox();
        }
    }
}
