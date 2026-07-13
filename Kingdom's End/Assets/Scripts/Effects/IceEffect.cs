using System.Collections;
using UnityEngine;

public class IceEffect : MonoBehaviour {
  public Hero hero;
  public string consumableKey;

  // refers to the number of "hits" needed to be broken
  [SerializeField] public int strength;
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

  public void Damage(int amount) {
    strength -= amount;

    // TODO: play crack sound

    if (shakeRoutine != null) StopCoroutine(shakeRoutine);
    shakeRoutine = StartCoroutine(Shake());

    // TODO: increase crack sprite

    if (strength <= 0) {
      Break();
    }
  }

  private void Break() {
    hero.BreakOutOfIce();
    Destroy(gameObject);
  }
}
