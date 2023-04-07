using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overrides
{
  public class Level1Override : SceneInitiator
  {
    [Header("References")]
    public EnemyAI enemy;
    public bool started;
    public GameObject diamond;
    public TextMeshProUGUI pressAnyKeyText;
    public TextMeshProUGUI yoinkText;
    public TextMeshProUGUI stopRightThereText;
    public TextMeshProUGUI dontShootText;
    public TextMeshProUGUI theyNeverListenText;
    public TextMeshProUGUI runText;
    public Image logo;
    public TextMeshProUGUI createdByText;
    public AudioSource audioSource;

    [Space] public Color textColor;

    public override void Init()
    {
      GameManager.Instance.inputAllowed = false;
      GameManager.Instance.timerRunning = false;
      GameManager.Instance.timer = 0;
      GameManager.Instance.timerText.text = "0.00";
    }

    public virtual void Update()
    {
      if (!Input.anyKeyDown || started) return;
      GameManager.Instance.inputAllowed = false;
      started = true;
      StartCoroutine(Sequence());
    }

    public virtual IEnumerator Sequence()
    {
      pressAnyKeyText.DOColor(Color.clear, 1f);
      logo.DOColor(Color.clear, 1f);
      createdByText.DOColor(Color.clear, 1f);
      yield return new WaitForSecondsRealtime(1f);

      diamond.SetActive(false);
      yoinkText.gameObject.SetActive(true);
      audioSource.PlayOneShot(GameManager.Instance.gemSound);
      yield return new WaitForSecondsRealtime(1f);

      stopRightThereText.DOColor(textColor, .5f);
      yield return new WaitForSecondsRealtime(1f);
      GameManager.Instance.player.playerSpriteRenderer.flipX = true;

      GameManager.Instance.mainCam.DOOrthoSize(12f, 1f);
      GameManager.Instance.mainCam.transform.DOMove(new Vector3(0, 1, -10), 1f);
      stopRightThereText.DOColor(Color.clear, .5f);
      yoinkText.DOColor(Color.clear, .5f);
      yield return new WaitForSecondsRealtime(1f);

      dontShootText.DOColor(textColor, .5f);
      yield return new WaitForSecondsRealtime(1f);
      GameManager.Instance.inputAllowed = true;
      GameManager.Instance.timerRunning = true;

      dontShootText.DOColor(Color.clear, 1f);
      yield return new WaitForSecondsRealtime(.5f);
      enemy.enabled = true;
      enemy.timer = 0;

      yield return new WaitForSecondsRealtime(.5f);

      theyNeverListenText.DOColor(textColor, .5f);
      yield return new WaitForSecondsRealtime(.25f);
      runText.DOColor(textColor, .5f);
      yield return new WaitForSecondsRealtime(.5f);
      theyNeverListenText.DOColor(Color.clear, 1f);
      yield return new WaitForSecondsRealtime(.5f);

      yield return null;
    }

    public virtual void OnDestroy()
    {
      if (!GameManager.Instance.player.isDead) return;
      
      GameManager.Instance.timer = 0;
      GameManager.Instance.timerText.text = "0.00";
      GameManager.Instance.player.playerSpriteRenderer.flipX = false;
      GameManager.Instance.inputAllowed = false;
      GameManager.Instance.mainCam.orthographicSize = 5;
      GameManager.Instance.mainCam.transform.position = new Vector3(-12.45f, -6.38f, -10);
    }
  }
}