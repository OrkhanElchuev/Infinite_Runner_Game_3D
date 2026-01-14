using UnityEngine;

public class PickUp : MonoBehaviour
{
    const string playerString = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerString))
        {
            Debug.Log(other.gameObject.name);   
        }
    }
}
