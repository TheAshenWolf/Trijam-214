using UnityEngine;

public class SceneInitiator : MonoBehaviour
{
    public Transform playerSpawnPoint;

    public virtual void Start()
    {
        GameManager.Instance.player.transform.position = playerSpawnPoint.position;
        GameManager.Instance.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Init();
    }

    public virtual void Init() {}
    
    #if UNITY_EDITOR
    
    public virtual void OnDrawGizmos()
    {
        if (playerSpawnPoint is null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerSpawnPoint.position,new Vector3(1, 1.75f, 0));
    }
    
    #endif
}