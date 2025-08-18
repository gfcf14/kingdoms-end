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

    // toggles the underground light based on area and room container
    Hero.instance.undergroundLight.SetActive(GameData.area == "underground" && !isIntersectionsContainer);

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
  }
}
