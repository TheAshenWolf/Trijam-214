using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  public SceneInitiator sceneInitiator;
  public float timer = 1f;
  public bool isStunned;
  public float stunTimer;
  public SpriteRenderer spriteRenderer;
  public AudioSource audioSource;
  
  public void Update()
  {
    if (isStunned)
    {
      stunTimer -= Time.deltaTime;
      if (stunTimer <= 0)
      {
        isStunned = false;
      }
    }
    else
    {
      timer -= Time.deltaTime;
      if (timer <= 0)
      {
        if (CheckPlayerRaycast())
        {
          timer = Config.enemyShooDelay;
          Shoot();
        }
      }
    }
  }

  public virtual void Shoot()
  {
    Vector3 position = transform.position;
    Vector3 direction = (GameManager.Instance.player.transform.position - position).normalized;
    GameObject bullet = GameObject.Instantiate(GameManager.Instance.bulletPrefab, position + new Vector3(direction.x, direction.y * 1.5f, 0),
      Quaternion.identity);
    audioSource.PlayOneShot(GameManager.Instance.shotSound);
    bullet.transform.SetParent(sceneInitiator.transform);
    bullet.GetComponent<Rigidbody2D>().velocity = direction * Config.bulletSpeed;
    
    spriteRenderer.flipX = direction.x > 0;
  }

  public virtual bool CheckPlayerRaycast()
  {
    Vector3 position = transform.position;
    
    Vector3 direction = (GameManager.Instance.player.transform.position - position).normalized;
    // 100f is more than enough for this game 
    RaycastHit2D hit = Physics2D.Raycast(position + direction, direction, 100f);
    if (hit.collider is not null)
    {
      if (hit.collider.CompareTag("Player"))
      {
        return true;
      }
    }

    return false;
  }

  public void Stun()
  {
    isStunned = true;
    stunTimer = Config.stunTime;
  }
}