using UnityEngine;

public class HealthPotion : Pickup, ICollectable
{
    public int healthRestoration;
    public void collect()
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        player.RestoreHealth(healthRestoration);
    }
   
}
