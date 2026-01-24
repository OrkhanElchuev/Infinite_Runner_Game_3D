using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Controls camera zoom (Field of View) using a CinemachineCamera.
/// When speed increases, the camera zooms out a bit (higher FOV) for a sense of speed,
/// and optionally plays a particle effect.
/// </summary>

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Particles played only when speeding up (speedAmount > 0).")]
    [SerializeField] ParticleSystem speedUpParticleSystem;

    [Header("Camera Settings")]
    [Tooltip("Minimum allowed Field of View.")]
    [SerializeField] float minFOV = 30f;
    [Tooltip("Maximum allowed Field of View.")]
    [SerializeField] float maxFOV = 120f;
    [Tooltip("How long it takes to transition from current FOV to target FOV.")]
    [SerializeField] float zoomDuration = 1f;
    [Tooltip("Scales the incoming speedAmount to a larger/smaller FOV shift.")]
    [SerializeField] float zoomSpeedModifier = 5f;

    CinemachineCamera cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();    
    }

    // Requests a smooth FOV change based on speed delta.
    // Positive speedAmount increases FOV (zoom out), negative decreases it (zoom in).
    public void ChangeCameraFOV(float speedAmount)
    {
        if (cinemachineCamera == null) return;

        StopAllCoroutines();
        StartCoroutine(ChangeFOVRoutine(speedAmount));

        // Show Particles only when speeding up.
        if (speedAmount > 0f && speedUpParticleSystem != null)
        {
            if (!speedUpParticleSystem.isPlaying)
            {
                speedUpParticleSystem.Play();                
            }
        }
    }

    IEnumerator ChangeFOVRoutine(float speedAmount)
    {
        float startFOV = cinemachineCamera.Lens.FieldOfView;
        // Target FOV depends on the speed delta and is clamped to avoid extreme values.
        float targetFOV = Mathf.Clamp(startFOV + speedAmount * zoomSpeedModifier, minFOV, maxFOV);

        float elapsedTime = 0f;

        // Lerp from current to target over zoomDuration seconds.
        while (elapsedTime < zoomDuration)
        {
            // Normalized time in [0..1].
            float time = elapsedTime / zoomDuration;

            // Advance time before applying to avoid one-frame delay in some cases.
            elapsedTime += Time.deltaTime;

            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, time);
            yield return null;
        }
        
        cinemachineCamera.Lens.FieldOfView = targetFOV;
    }
}
