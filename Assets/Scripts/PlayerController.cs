using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public Rigidbody2D playerRigidBody;
  public SpriteRenderer playerSpriteRenderer;
  public AudioSource audioSource;

  [Header("Data")] public int remainingDoubleJumps;
  public bool goombaPower;
  public bool isDead;
  public float stepTimer = Config.stepDelay;

  public void Start()
  {
    remainingDoubleJumps = Config.doubleJumpCount;
    Application.targetFrameRate = 30;
    QualitySettings.vSyncCount = 0;
  }

  // Update is called once per frame
  public virtual void FixedUpdate()
  {
    if (!GameManager.Instance.inputAllowed) return;
    // jump
    if (Input.GetKeyDown(KeyCode.W))
    {
      if (IsGrounded())
      {
        playerRigidBody.velocity += Vector2.up * Config.jumpForce;
      }
      else
      {
        if (remainingDoubleJumps > 0)
        {
          playerRigidBody.velocity += Vector2.up * Config.jumpForce;
          remainingDoubleJumps--;
        }
      }
    }


    if (Input.GetKey(KeyCode.A))
    {
      playerRigidBody.velocity = new Vector2(-Config.movementSpeed, playerRigidBody.velocity.y);
      playerSpriteRenderer.flipX = false;
      Step();
    }
    else if (Input.GetKey(KeyCode.D))
    {
      playerRigidBody.velocity = new Vector2(Config.movementSpeed, playerRigidBody.velocity.y);
      playerSpriteRenderer.flipX = true;
      Step();
    }
  }

  public virtual void Step()
  {
    
    stepTimer -= Time.deltaTime;
    if (stepTimer <= 0)
    {
      if (!IsGrounded() || isDead) return;
      audioSource.volume = Random.Range(0.1f, 0.3f);
      audioSource.PlayOneShot(
        GameManager.Instance.footstepSounds[Random.Range(0, GameManager.Instance.footstepSounds.Length)]);
      audioSource.volume = 1f;
      stepTimer = Config.stepDelay;
    }
  }

  public virtual void OnCollisionEnter2D(Collision2D collision)
  {
    // Player became grounded
    if (collision.gameObject.CompareTag("Ground"))
    {
      remainingDoubleJumps = Config.doubleJumpCount;
    }

    // Player hit an enemy or a trap
    if (collision.gameObject.CompareTag("Danger") || collision.gameObject.CompareTag("Enemy"))
    {
      if (goombaPower)
      {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy is not null)
        {
          enemy.Stun();
        }
      }
      else
      {
        if (isDead) return;
        audioSource.PlayOneShot(GameManager.Instance.deathSound);
        isDead = true;
        StartCoroutine(GameManager.Instance.RestartLevel());
      }
    }
  }

  public virtual void OnTriggerEnter2D(Collider2D other)
  {
    if (isDead) return;

    if (other.CompareTag("Goomba"))
    {
      goombaPower = true;
    }

    if (other.CompareTag("Finish"))
    {
      if (GameManager.Instance.transitioning) return;
      GameManager.Instance.transitioning = true;
      GameManager.Instance.LoadNextLevel();
    }
  }

  public virtual void OnTriggerExit2D(Collider2D other)
  {
    if (other.CompareTag("Goomba"))
    {
      goombaPower = false;
    }
  }

  public virtual bool IsGrounded()
  {
    // If the player is at most 0.05 units away from the ground, then they are considered grounded
    // Player is 2 units tall, so + Vector3.down is at players feet
    return Physics2D.Raycast(transform.position + Vector3.down, Vector2.down, 0.05f);
  }
}