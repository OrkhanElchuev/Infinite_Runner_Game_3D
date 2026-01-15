using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] float startTime = 5f;

    float timeLeft;

    void Start()
    {
        timeLeft = startTime;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        // F1 for 1 decimal place formatting
        timeText.text = timeLeft.ToString("F1");
    }
}
