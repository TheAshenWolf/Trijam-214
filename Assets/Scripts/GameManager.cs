using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; set; }

  [Header("References")] 
  public PlayerController player;
  public Image fader;
  public TextMeshProUGUI timerText;
  public Camera mainCam;
  
  [Header("Prefabs")]
  public GameObject bulletPrefab;
  
  [Header("Data")]
  public int currentLevel = 1;
  public bool inputAllowed;
  public bool timerRunning;
  public float timer;
  public bool transitioning;
  
  [Header("Sounds")]
  public AudioClip[] footstepSounds;
  public AudioClip shotSound;
  public AudioClip deathSound;
  public AudioClip gemSound;
  
  public void Awake()
  {
    if (Instance is null) Instance = this;
    else Destroy(gameObject);
    
    DontDestroyOnLoad(gameObject);

    SceneManager.LoadScene(1, LoadSceneMode.Additive);
  }

  public void Update()
  {
    if (timerRunning)
    {
      timer += Time.deltaTime;
      timerText.text = timer.ToString("F2");
    }
  }

  public IEnumerator RestartLevel()
  {
    yield return StartCoroutine(LoadLevel(false));
  }
  
  public IEnumerator LoadLevel(bool increment)
  {
    yield return StartCoroutine(FadeBlack());
    SceneManager.UnloadSceneAsync(currentLevel);
    if (increment) currentLevel++;
    SceneManager.LoadScene(currentLevel, LoadSceneMode.Additive);
    yield return StartCoroutine(FadeClear());
    player.isDead = false;
    yield return null;
  }

  public virtual IEnumerator FadeClear()
  {
    fader.DOColor(new Color(0, 0, 0, 0), 1f);
    yield return new WaitForSecondsRealtime(.5f);
    if (!SceneManager.GetSceneByBuildIndex(1).isLoaded) Instance.timerRunning = true;
    Instance.transitioning = false;
    yield return new WaitForSecondsRealtime(.5f);
  }

  public virtual IEnumerator FadeBlack()
  {
    fader.DOColor(new Color(0, 0, 0, 1), 1f);
    yield return new WaitForSecondsRealtime(.5f);
    Instance.timerRunning = false;
    yield return new WaitForSecondsRealtime(.5f);
  }

  public void LoadNextLevel()
  {
    StartCoroutine(LoadLevel(true));
  }
}