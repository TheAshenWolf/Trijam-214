using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenuDoor : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI finalTimeText;
    
    
    public float finalTime;
    public virtual void Awake()
    {
        GameManager.Instance.timerRunning = false;
        finalTime = GameManager.Instance.timer;
        
        GameManager.Instance.timerText.gameObject.SetActive(false);
    }
    
    public virtual void Start()
    {
        finalTimeText.text = $"Thank you for playing.\n Your final time was {finalTime:F4}";
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Menu());
        }
    }
    
    public virtual IEnumerator Menu()
    {
        yield return StartCoroutine(GameManager.Instance.FadeBlack());
        SceneManager.UnloadSceneAsync(GameManager.Instance.currentLevel);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        
        GameManager.Instance.timer = 0;
        GameManager.Instance.timerText.text = "0.00";
        GameManager.Instance.player.playerSpriteRenderer.flipX = false;
        GameManager.Instance.inputAllowed = false;
        GameManager.Instance.mainCam.orthographicSize = 5;
        GameManager.Instance.mainCam.transform.position = new Vector3(-12.45f, -6.38f, -10);
        
        yield return StartCoroutine(GameManager.Instance.FadeClear());
        GameManager.Instance.player.isDead = false;
        yield return null;
    }
}
