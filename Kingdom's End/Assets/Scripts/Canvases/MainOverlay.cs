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

    // sets the area image based on the gamedata area
    // TODO: ensure this can change as the game is loaded from a save state
    areaImage.sprite = Helpers.GetOrException(Sprites.areaImages, GameData.area);
    // areaImage.sprite = Helpers.GetOrException(Sprites.areaImages, "space");
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
    areaImage.sprite = Helpers.GetOrException(Sprites.areaImages, GameData.area);
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
