using UnityEngine;

public class IceEffect : MonoBehaviour {
  public Hero hero;
  public string consumableKey;

  // refers to the number of "hits" needed to be broken
  [SerializeField] public int strength;
  void Start() {}

  void Update() {}

  public void Damage(int amount) {
    strength -= amount;

    // TODO: play crack sound
    // shake?
    // increase crack sprite?

    if (strength <= 0) {
      Break();
    }
  }

  private void Break() {
    hero.BreakOutOfIce();
    Destroy(gameObject);
  }
}
