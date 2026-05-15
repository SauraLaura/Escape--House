using UnityEngine;
using UnityEngine.SceneManagement;

public class EngGame : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("You have entered the engineering room. You see a lever and a hatch.");
            SceneManager.LoadScene("EndScreen"); // Load the engineering puzzle scene when the player enters the trigger area
        }
    }
}
