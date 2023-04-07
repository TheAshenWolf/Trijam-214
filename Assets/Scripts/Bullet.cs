using UnityEngine;

public class Bullet : MonoBehaviour
{
  public virtual void OnCollisionEnter2D(Collision2D col)
  {
    if (col.gameObject.CompareTag("Enemy"))
    {
      // We can't play the sound on the enemy nor the bullet because they will be destroyed
      GameManager.Instance.player.audioSource.PlayOneShot(GameManager.Instance.deathSound);
      Destroy(col.gameObject);
    }
    Destroy(gameObject);
  }

  public void Update()
  {
    if (Vector2.Distance(Vector2.zero, transform.position) > 50)
      Destroy(gameObject);
  }
}
