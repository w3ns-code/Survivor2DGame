using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    MapController mc;
    public GameObject targetMap;

  [System.Obsolete]
  void Start()
    {
        mc = FindObjectOfType<MapController>();
    }


  private void OnTriggerStay2D(Collider2D col)
  {
        if (col.CompareTag("Player"))
        {
            mc.currentChunck = targetMap;
        }
  }

  public void OnTriggerExit2D(Collider2D col)
  {
        if (col.CompareTag("Player"))
        {
            if(mc.currentChunck == targetMap)
            {
                mc.currentChunck = null;
            }
        }
  }
}
