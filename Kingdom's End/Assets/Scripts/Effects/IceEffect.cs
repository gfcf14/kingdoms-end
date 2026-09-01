using System.Collections;
using UnityEngine;

public class IceEffect : MonoBehaviour {
  public Hero hero;
  public GameObject iceCrack;
  public string consumableKey;

  // refers to the number of "hits" needed to be broken
  [SerializeField] public int strength;
  [SerializeField] public int totalStrength;
  [SerializeField] private float shakeDistance = 0.08f;
  [SerializeField] private float shakeTime = 0.03f;

  private Vector2 originalPosition;
  private Coroutine shakeRoutine;

  void Awake() {
    originalPosition = transform.localPosition;
  }

  void Start() {}

  void Update() {}

  private IEnumerator Shake() {
    int direction = Random.value < 0.5f ? -1 : 1;

    transform.localPosition = new Vector2(originalPosition.x + Vector2.right.x * direction * shakeDistance, originalPosition.y);
    yield return new WaitForSeconds(shakeTime);

    transform.localPosition = new Vector2(originalPosition.x + Vector2.right.x * -direction * shakeDistance * 0.6f, originalPosition.y);
    yield return new WaitForSeconds(shakeTime);

    transform.localPosition = originalPosition;

    shakeRoutine = null;
  }

  public void DestroyIce() {
    InGame.instance.PrepareFullRockExplosion(gameObject, "ice", "ice");
    Break();
  }

  public void Damage(int amount) {
    strength -= amount;

    InGame.instance.PlaySound(Sounds.iceCrackSound, transform.position);

    if (shakeRoutine != null) StopCoroutine(shakeRoutine);
    shakeRoutine = StartCoroutine(Shake());

    float totalToDamageRatio = (float)strength / totalStrength;
    int expectedCrackIndex = 0;

    if (totalToDamageRatio < 0.75f) {
      if (totalToDamageRatio <= 0.25) expectedCrackIndex = 25;
      else if (totalToDamageRatio <= 0.50) expectedCrackIndex = 50;
      else if (totalToDamageRatio <= 0.75) expectedCrackIndex = 75;

      iceCrack.GetComponent<SpriteRenderer>().sprite = Helpers.GetOrException(Sprites.iceCrackSpritesByPercentage, expectedCrackIndex);
    }

    if (strength <= 0) {
      DestroyIce();
    }
  }

  private void Break() {
    hero.BreakOutOfIce();
    Destroy(iceCrack);
    Destroy(gameObject);
  }
}
