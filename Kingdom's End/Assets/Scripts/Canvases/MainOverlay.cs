using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MainOverlay : MonoBehaviour {
  private Animator anim;
  public Image areaImage;
  public float timeOnFade = 0;

  public void AssignTilemaps() {
    GameObject grid = GameObject.Find("Grid");

    Debug.Log(InGame.instance.globalGradients);
    InGame.instance.globalGradients.skyTilemap = grid.transform.Find("Sky").GetComponent<Tilemap>();
    InGame.instance.globalGradients.cloudsTilemap = grid.transform.Find("Clouds").GetComponent<Tilemap>();
    InGame.instance.globalGradients.backgroundFarTilemap = grid.transform.Find("Background Far").GetComponent<Tilemap>();
    InGame.instance.globalGradients.backgroundMiddleTilemap = grid.transform.Find("Background Middle").GetComponent<Tilemap>();
    InGame.instance.globalGradients.backgroundCloseTilemap = grid.transform.Find("Background Close").GetComponent<Tilemap>();
    InGame.instance.globalGradients.fortressBackTilemap = grid.transform.Find("Fortress - Back").GetComponent<Tilemap>();
    InGame.instance.globalGradients.foregroundTilemap = grid.transform.Find("Foreground").GetComponent<Tilemap>();
    InGame.instance.globalGradients.fortressFrontTilemap = grid.transform.Find("Fortress - Front").GetComponent<Tilemap>();
    InGame.instance.globalGradients.wallsTilemap = grid.transform.Find("Walls").GetComponent<Tilemap>();
    InGame.instance.globalGradients.floorsTilemap = grid.transform.Find("Floors").GetComponent<Tilemap>();
    InGame.instance.globalGradients.blendsTilemap = grid.transform.Find("Blends").GetComponent<Tilemap>();
    InGame.instance.globalGradients.detailTilemap = grid.transform.Find("Detail").GetComponent<Tilemap>();
    InGame.instance.globalGradients.buildingsBackTilemap = grid.transform.Find("Buildings - Back").GetComponent<Tilemap>();
    InGame.instance.globalGradients.buildingsFrontTilemap = grid.transform.Find("Buildings - Front").GetComponent<Tilemap>();
    InGame.instance.globalGradients.overlaysTilemap = grid.transform.Find("Overlays").GetComponent<Tilemap>();
  }

  void Start() {
    if (FindObjectsOfType<InGame>().Length == 0) {
      Instantiate(Helpers.GetOrException(Objects.prefabs, "in-game"), Vector2.zero, Quaternion.identity);
    } else {
      AssignTilemaps();
    }

    anim = GetComponent<Animator>();
    areaImage = gameObject.transform.Find("AreaImage").GetComponent<Image>();
  }

  void Update() {}

  public void PauseUntilFading() {
    anim.speed = 1;
    InGame.instance.SetPauseCase("fading-in");
  }

  public void PrepareAreaTransition() {
    InGame.instance.StopSoundtrack();
    InGame.instance.ClearPauseTimes();
    anim.speed = 1;
    anim.enabled = true;
    anim.Play("area-transition", -1, normalizedTime: 0);
  }

  public void ChangeArea() {
    PauseUntilFading();
  }

  public void ProceedWhenFading() {
    InGame.instance.ClearPauseCase();
    timeOnFade = Time.realtimeSinceStartup;
  }

  public void OverlayStandBy() {
    anim.speed = 0;
    anim.enabled = false;
  }

  public void NotifyFlashFadeOutFinish() {
    InGame.instance.FlashFinish();
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
    InGame.instance.PlaySoundtrack(GameData.area);
  }
}
