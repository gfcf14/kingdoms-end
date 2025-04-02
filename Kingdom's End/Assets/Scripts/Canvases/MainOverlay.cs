using UnityEngine;
using UnityEngine.UI;

public class MainOverlay : MonoBehaviour {
  private Animator anim;
  private InGame inGame;

  public Image areaImage;

  public float timeOnFade = 0;

  void Start() {
    anim = GetComponent<Animator>();
    inGame = GameObject.Find("InGame").gameObject.GetComponent<InGame>();
    areaImage = gameObject.transform.Find("AreaImage").GetComponent<Image>();
  }

  void Update() {}

  public void PauseUntilFading() {
    anim.speed = 1;
    inGame.SetPauseCase("fading-in");
  }

  public void PrepareAreaTransition() {
    inGame.StopSoundtrack();
    inGame.ClearPauseTimes();
    anim.speed = 1;
    anim.enabled = true;
    anim.Play("area-transition", -1, normalizedTime: 0);
  }

  public void ChangeArea() {
    PauseUntilFading();
  }

  public void ProceedWhenFading() {
    inGame.ClearPauseCase();
    timeOnFade = Time.realtimeSinceStartup;
  }

  public void OverlayStandBy() {
    anim.speed = 0;
    anim.enabled = false;
  }

  public void NotifyFlashFadeOutFinish() {
    inGame.FlashFinish();
  }

  public void FlashFadeOut() {
    anim.speed = 1;
    anim.enabled = true;
    anim.Play("flash-fade-out");
  }

  public void FlashFadeIn() {
    anim.Play("flash-fade-in");
  }

  public void FinishFlashFadeIn() {
    anim.speed = 0;
    anim.enabled = false;
  }

  public void PlaySoundtrack() {
    inGame.PlaySoundtrack(GameData.area);
  }
}
