using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour 
{
    public TextMeshProUGUI objectiveText;

    public void UpdateObjective(string newGoal) 
    {
        objectiveText.text = "Objective: " + newGoal;
    }
}