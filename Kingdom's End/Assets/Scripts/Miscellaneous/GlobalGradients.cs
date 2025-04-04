using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlobalGradients : MonoBehaviour {
  [SerializeField] public Tilemap skyTilemap;
  [SerializeField] public Tilemap cloudsTilemap;
  [SerializeField] public Tilemap backgroundFarTilemap;
  [SerializeField] public Tilemap backgroundMiddleTilemap;
  [SerializeField] public Tilemap backgroundCloseTilemap;
  [SerializeField] public Tilemap fortressBackTilemap;
  [SerializeField] public Tilemap foregroundTilemap;
  [SerializeField] public Tilemap fortressFrontTilemap;
  [SerializeField] public Tilemap wallsTilemap;
  [SerializeField] public Tilemap floorsTilemap;
  [SerializeField] public Tilemap blendsTilemap;
  [SerializeField] public Tilemap detailTilemap;
  [SerializeField] public Tilemap buildingsBackTilemap;
  [SerializeField] public Tilemap buildingsFrontTilemap;
  [SerializeField] public Tilemap overlaysTilemap;

  [System.NonSerialized] public Tilemap savePointTilemap;
  [System.NonSerialized] public Tilemap teleportPointTilemap;

  [SerializeField] public string area = "";

  [SerializeField] public bool isIndoors = false;

  // TODO: remove these test variables once no longer used
  [SerializeField] string currentTime;
  [SerializeField] float currentGradientPercentage;

  [Header("Common Gradients")]
  [SerializeField] Gradient cloudsGradient;
  [SerializeField] Gradient detailGradient;

  [Space(10)]

  [Header("Meadows Gradients")]
  [SerializeField] Gradient meadowsSkyGradient;
  [SerializeField] Gradient meadowsGroundGradient;
  [SerializeField] Gradient meadowsBuildingsGradient;
  [Space(10)]

  [Header("Forest Gradients")]
  [SerializeField] Gradient forestSkyGradient;
  [SerializeField] Gradient forestGroundGradient;
  [SerializeField] Gradient forestBuildingsGradient;
  [Space(10)]

  [Header("Swamps Gradients")]
  [SerializeField] Gradient swampsSkyGradient;
  [SerializeField] Gradient swampsGroundGradient;
  [SerializeField] Gradient swampsBuildingsGradient;
  [Space(10)]

  [Header("Mountains Gradients")]
  [SerializeField] Gradient mountainsSkyGradient;
  [SerializeField] Gradient mountainsGroundGradient;
  [SerializeField] Gradient mountainsBuildingsGradient;
  [Space(10)]

  [Header("Calderas Gradients")]
  [SerializeField] Gradient calderasSkyGradient;
  [SerializeField] Gradient calderasGroundGradient;
  [SerializeField] Gradient calderasBuildingsGradient;
  [Space(10)]

  [Header("Glaciers Gradients")]
  [SerializeField] Gradient glaciersSkyGradient;
  [SerializeField] Gradient glaciersGroundGradient;
  [SerializeField] Gradient glaciersBuildingsGradient;
  [Space(10)]

  [Header("Seaside Gradients")]
  [SerializeField] Gradient seasideSkyGradient;
  [SerializeField] Gradient seasideGroundGradient;
  [SerializeField] Gradient seasideBuildingsGradient;
  [Space(10)]

  [Header("Oceans Gradients")]
  [SerializeField] Gradient oceansSkyGradient;
  [SerializeField] Gradient oceansGroundGradient;
  [SerializeField] Gradient oceansBuildingsGradient;
  [Space(10)]

  [Header("Desert Gradients")]
  [SerializeField] Gradient desertSkyGradient;
  [SerializeField] Gradient desertGroundGradient;
  [SerializeField] Gradient desertBuildingsGradient;
  [Space(10)]

  [Header("Ruins Gradients")]
  [SerializeField] Gradient ruinsSkyGradient;
  [SerializeField] Gradient ruinsGroundGradient;
  [SerializeField] Gradient ruinsBuildingsGradient;
  [Space(10)]

  [Header("Wasteland Gradients")]
  [SerializeField] Gradient wastelandSkyGradient;
  [SerializeField] Gradient wastelandGroundGradient;
  [SerializeField] Gradient wastelandBuildingsGradient;
  [Space(10)]

  [System.NonSerialized] private Dictionary<string, Dictionary<string, Gradient>> areaGradients;
  [System.NonSerialized] private Color resetColor = Color.white;

  void Start() {
    areaGradients = new Dictionary<string, Dictionary<string, Gradient>> {
      {"calderas", new Dictionary<string, Gradient> {
          {"sky", calderasSkyGradient}, {"ground", calderasGroundGradient}, {"buildings", calderasBuildingsGradient}
        }
      },
      {"desert", new Dictionary<string, Gradient> {
          {"sky", desertSkyGradient}, {"ground", desertGroundGradient}, {"buildings", desertBuildingsGradient}
        }
      },
      {"forest", new Dictionary<string, Gradient> {
          {"sky", forestSkyGradient}, {"ground", forestGroundGradient}, {"buildings", forestBuildingsGradient}
        }
      },
      {"glaciers", new Dictionary<string, Gradient> {
          {"sky", glaciersSkyGradient}, {"ground", glaciersGroundGradient}, {"buildings", glaciersBuildingsGradient}
        }
      },
      {"meadows", new Dictionary<string, Gradient> {
          {"sky", meadowsSkyGradient}, {"ground", meadowsGroundGradient}, {"buildings", meadowsBuildingsGradient}
        }
      },
      {"mountains", new Dictionary<string, Gradient> {
          {"sky", mountainsSkyGradient}, {"ground", mountainsGroundGradient}, {"buildings", mountainsBuildingsGradient}
        }
      },
      {"oceans", new Dictionary<string, Gradient> {
          {"sky", oceansSkyGradient}, {"ground", oceansGroundGradient}, {"buildings", oceansBuildingsGradient}
        }
      },
      {"ruins", new Dictionary<string, Gradient> {
          {"sky", ruinsSkyGradient}, {"ground", ruinsGroundGradient}, {"buildings", ruinsBuildingsGradient}
        }
      },
      {"seaside", new Dictionary<string, Gradient> {
          {"sky", seasideSkyGradient}, {"ground", seasideGroundGradient}, {"buildings", seasideBuildingsGradient}
        }
      },
      {"swamps", new Dictionary<string, Gradient> {
          {"sky", swampsSkyGradient}, {"ground", swampsGroundGradient}, {"buildings", swampsBuildingsGradient}
        }
      },
      {"wasteland", new Dictionary<string, Gradient> {
          {"sky", wastelandSkyGradient}, {"ground", wastelandGroundGradient}, {"buildings", wastelandBuildingsGradient}
        }
      }
    };
  }

  void Update() {
    if (!Hero.instance.isPaused) {
      // TODO: add time from loaded time once implemented
      float elapsedTime = Time.time + GameData.initialGameTime;
      currentTime = Helpers.GetGameTime((int)(elapsedTime % Constants.maxDayLength));

      // gets the percentage based on the sine of the time elapsed
      float percentage = (elapsedTime % Constants.maxDayLength) / Constants.maxDayLength;

      // clamps the percentage to be between 0 and 1
      percentage = Mathf.Clamp01(percentage);
      currentGradientPercentage = percentage;

      if (!Helpers.IsValueInArray(Constants.nonGradientAreas, area)) {
        skyTilemap.color = Helpers.GetOrException(Helpers.GetOrException(areaGradients, area), "sky").Evaluate(percentage);
      }

      if (!isIndoors && !Helpers.IsValueInArray(Constants.nonGradientAreas, area)) {
        cloudsTilemap.color = cloudsGradient.Evaluate(percentage);

        Color currentGradientColor = Helpers.GetOrException(Helpers.GetOrException(areaGradients, area), "ground").Evaluate(percentage);
        Color buildingGradientColor = Helpers.GetOrException(Helpers.GetOrException(areaGradients, area), "buildings").Evaluate(percentage);

        backgroundFarTilemap.color = currentGradientColor;
        backgroundMiddleTilemap.color = currentGradientColor;
        backgroundCloseTilemap.color = currentGradientColor;
        foregroundTilemap.color = currentGradientColor;
        wallsTilemap.color = currentGradientColor;
        floorsTilemap.color = currentGradientColor;
        blendsTilemap.color = currentGradientColor;
        overlaysTilemap.color = currentGradientColor;

        if (savePointTilemap != null) {
          savePointTilemap.color = currentGradientColor;
        }

        if (teleportPointTilemap != null) {
          teleportPointTilemap.color = currentGradientColor;
        }

        // TODO: find a way for each ambush floor to put itself on Start inside a list here, and remove itself on destroy. That way there's less work to do for this object
        // checks all enemy spawners of the current room and if an ambush floor is found, paint it as per current gradient (otherwise it's glaringly visible)
        if (Hero.instance.currentRoom != null) {
          foreach (Transform child in Hero.instance.currentRoom.transform) {
            if (child.tag == "EnemySpawner") {
              foreach (Transform spawnerChild in child.transform) {
                if (spawnerChild.tag == "AmbushFloor") {
                  spawnerChild.Find("Tile").GetComponent<SpriteRenderer>().color = currentGradientColor;
                }
              }
            }
          }
        }

        detailTilemap.color = detailGradient.Evaluate(percentage);

        fortressBackTilemap.color = buildingGradientColor;
        fortressFrontTilemap.color = buildingGradientColor;
        buildingsBackTilemap.color = buildingGradientColor;
        buildingsFrontTilemap.color = buildingGradientColor;
      }
    }
  }

  public void ResetTilemaps() {
    skyTilemap.color = resetColor;
    cloudsTilemap.color = resetColor;
    backgroundFarTilemap.color = resetColor;
    backgroundMiddleTilemap.color = resetColor;
    backgroundCloseTilemap.color = resetColor;
    foregroundTilemap.color = resetColor;
    wallsTilemap.color = resetColor;
    floorsTilemap.color = resetColor;
    blendsTilemap.color = resetColor;
    overlaysTilemap.color = resetColor;
    detailTilemap.color = resetColor;
  }
}
