using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ExperienceGem : Pickup, ICollectable
{
    public int experienceGranted;

    public void collect()
    {
        Debug.Log("Called");
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }

 

}
