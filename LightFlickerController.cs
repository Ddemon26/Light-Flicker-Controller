using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Damon.Lighting
{
    /// <summary>
    /// Controls flickering of a light to simulate effects like a faulty light bulb.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class LightFlickerController : MonoBehaviour
    {
        [SerializeField] Light controlledLight;
        [SerializeField, Tooltip("Minimum duration the light stays on or off during a flicker.")]
        float minFlickerDuration = 0.5f;
        [SerializeField, Tooltip("Maximum duration the light stays on or off during a flicker.")]
        float maxFlickerDuration = 1.5f;
        [SerializeField, Tooltip("Minimum duration the light stays off between flickers.")]
        float minOffDuration = 0.1f;
        [SerializeField, Tooltip("Maximum duration the light stays off between flickers.")] 
        float maxOffDuration = 0.2f;
        [SerializeField, Tooltip("Number of times the light flickers before stopping.")] 
        int repeatCount = 10;
        [SerializeField, Tooltip("Whether the light remains on after completing the flickering sequence.")] 
        bool stayOnAfterComplete = false;
        [SerializeField, Tooltip("Whether flickering starts automatically when the object awakens.")] 
        bool startOnAwake = true;
        [System.Serializable]
        public class DirectEvents
        {
            public UnityEvent OnFlickerStart;
            public UnityEvent OnFlickerStop;
            public UnityEvent OnLightTurnOn;
            public UnityEvent OnLightTurnOff;
        }
        [Header("Events"),Tooltip("Direct events.")]
        public DirectEvents Events = new();

        int currentRepeat = 0;
        CountdownTimer flickerTimer;
        CountdownTimer offTimer;

        void Awake()
        {
            SetupTimers();
            if (startOnAwake) StartFlickering();
        }
        void Update()
        {
            if (flickerTimer.IsRunning)
            {
                flickerTimer.Tick(Time.deltaTime);
            }
            if (offTimer.IsRunning)
            {
                offTimer.Tick(Time.deltaTime);
            }
        }

        void SetupTimers()
        {
            flickerTimer = new CountdownTimer(GetRandomFlickerDuration());
            offTimer = new CountdownTimer(GetRandomOffDuration());

            flickerTimer.OnTimerStop += HandleFlickerTimerStop;
            offTimer.OnTimerStop += HandleOffTimerStop;
        }
        void HandleFlickerTimerStop()
        {
            ToggleLight(!controlledLight.enabled);
            offTimer.Reset(GetRandomOffDuration());
            offTimer.Start();
        }

        void HandleOffTimerStop()
        {
            ToggleLight(!controlledLight.enabled);
            if (++currentRepeat < repeatCount)
            {
                flickerTimer.Reset(GetRandomFlickerDuration());
                flickerTimer.Start();
            }
            else
            {
                controlledLight.enabled = stayOnAfterComplete;
            }
        }

        float GetRandomFlickerDuration() => Random.Range(minFlickerDuration, maxFlickerDuration);
        float GetRandomOffDuration() => Random.Range(minOffDuration, maxOffDuration);

        /// <summary>
        /// Toggles the light on or off.
        /// </summary>
        public void ToggleLight(bool value)
        {
            controlledLight.enabled = value;
            if (value) Events.OnLightTurnOn.Invoke();
            else Events.OnLightTurnOff.Invoke();
        }
        /// <summary>
        /// Starts the flickering effect.
        /// </summary>
        public void StartFlickering()
        {
            currentRepeat = 0;
            flickerTimer.Start();
            Events.OnFlickerStart.Invoke();
        }
        /// <summary>
        /// Stops the flickering effect.
        /// </summary>
        public void StopFlickering()
        {
            flickerTimer.Stop();
            offTimer.Stop();
            controlledLight.enabled = stayOnAfterComplete;
            Events.OnFlickerStop.Invoke();
        }
        /// <summary>
        /// Resets and restarts the flickering effect.
        /// </summary>
        public void ResetFlickering()
        {
            StopFlickering();
            StartFlickering();
        }
    }
}
