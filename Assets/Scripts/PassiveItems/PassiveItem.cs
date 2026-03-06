using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApplyModifier()
    {
        
    }
    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
        ApplyModifier();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
