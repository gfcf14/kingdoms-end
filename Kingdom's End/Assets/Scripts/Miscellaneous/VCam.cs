using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class VCam : MonoBehaviour
{
  void Start() {}
  void Update() {}

  public void DetermineRepaintOutsideNonGradientAreas() {
    string roomContainer = transform.parent.parent.gameObject.name;
    bool isInNonGradientArea = Helpers.IsValueInArray(Constants.nonGradientAreas, GameData.area);
    bool isIntersectionsContainer = roomContainer.Contains("Intersections");

    // modifies bool to ensure areas can receive gradient color changes despite being in an area that shouldn't
    // (e.g. if the player is in an intersection room in the underground area)
    InGame.instance.globalGradients.shouldPaintOutsideArea = !isInNonGradientArea || (isInNonGradientArea && isIntersectionsContainer);

    if (isIntersectionsContainer) {
      string currentRoom = transform.parent.gameObject.name;
      string firstAreaRegex = @"Room\s*-\s*([A-Za-z]+)"; // gets first mentioned area, e.g. from "Room - Desert to Seaside" it gets "desert"

      Match match = Regex.Match(currentRoom, firstAreaRegex);

      if (match.Success) {
        InGame.instance.globalGradients.area = match.Groups[1].Value.ToLower();
      }
    } else {
      if (isInNonGradientArea) {
        // resets the tilemaps when, while in the same area, entering back to a location where tilemaps should not paint
        // (e.g. when going from an intersection to the main underground portion of the underground area)
        InGame.instance.globalGradients.ResetTilemaps();
      }
    }

    // toggles the underground light based on if the user has a partial light relic, area and room container
    bool hasPartialLightRelic = Hero.instance.relicItems.Any(relicItem => Constants.partialLightRelics.Contains(relicItem.key));
    Hero.instance.undergroundLight.SetActive(hasPartialLightRelic && GameData.area == "underground" && !isIntersectionsContainer);

    // Deactivates any present darkness in an area if the user has the sundrop
      bool hasSunDrop = Hero.instance.relicItems.Any(relicItem => relicItem.key == "sundrop");

      if (hasSunDrop) {
        GameObject[] allDarknesses = GameObject.FindGameObjectsWithTag("Darkness");

        foreach (GameObject currDarkness in allDarknesses) {
          if (currDarkness.activeSelf) {
            currDarkness.SetActive(false);
          }
        }
      }
  }
  public void RemoveOwnedRelics() {
    // Convert to HashSet for fast lookups
    HashSet<string> ownedRelicKeys = new HashSet<string>();
    foreach (var item in Hero.instance.relicItems) {
        ownedRelicKeys.Add(item.key);
    }

    // Find all relics in the scene
    Relic[] relicObjects = FindObjectsOfType<Relic>();

    foreach (var relic in relicObjects) {
      // if any relics are found that exist in the HashSet, then the player already owns them and thus these should be destroyed
      if (ownedRelicKeys.Contains(relic.key)) {
        Destroy(relic.gameObject);
      }
    }
  }
}
