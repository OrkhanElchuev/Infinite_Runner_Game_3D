using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float minFOV = 30f;
    [SerializeField] float maxFOV = 120f;
    [SerializeField] float zoomDuration = 1f;

    CinemachineCamera cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();    
    }

    // Field of View 
    public void ChangeCameraFOV(float speedAmount)
    {
        StartCoroutine(ChangeFOVRoutine(speedAmount));
    }

    IEnumerator ChangeFOVRoutine(float speedAmount)
    {
        float startFOV = cinemachineCamera.Lens.FieldOfView;
        float targetFOV = Mathf.Clamp(startFOV + speedAmount, minFOV, maxFOV);

        float elapsedTime = 0f;
        // Lerp slowly between point A to point B
        while (elapsedTime < zoomDuration)
        {
            float time = elapsedTime / zoomDuration;
            elapsedTime += Time.deltaTime;

            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, time);
            yield return null;
        }
        cinemachineCamera.Lens.FieldOfView = targetFOV;
    }
}
