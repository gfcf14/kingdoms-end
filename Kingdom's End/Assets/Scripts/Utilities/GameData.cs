// TODO: Use this class to save game progress. Ensure that constant variables (introText, highestSTR, etc.) are moved and referenced from elsewhere
public class GameData {
  public static string area = "meadows";
  public static string introText = "Legend tells that the blossoming of the Rosolis flower heralds a period of unparalleled victory, followed by everlasting peace...";
  public static string[] playerStats = { "HP", "MP", "STR", "STA", "CRI", "LCK" };

  public static int baseHP = 100;
  public static int highestHP = 5000;

  public static int baseSTR = 10;
  public static int highestSTR = 2000;

  public static int baseSTA = 25;
  public static int highestSTA = 2500;

  public static int initialGameTime = 720;

  public static int maxJumpLimit = 2;

  public static float baseCRI = 0.03f;
  public static float highestCRI = 0.3f;

  public static float baseLCK = 0.025f;
  public static float highestLCK = 0.25f;

  public static float mainCameraX = -66;
  public static float mainCameraY = 12.5f;

  public static float playerJumpHeight = 8f;

  public static float playerX = -124;
  public static float playerY = 0;

  public static int highestEXP = 999999999;

  public static float[] enemyEXPValues = {1, 2, 4, 5, 10, 20, 25, 30, 37.5f, 45, 50};

  public static bool blewBarricade = false;
}
