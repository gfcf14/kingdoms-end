using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites {
  public static Sprite[] elements = Resources.LoadAll<Sprite>("Pause/elements");
  public static Sprite[] keyset = Resources.LoadAll<Sprite>("Spritesheets/keyset");
  public static Sprite[] itemThumbnails = Resources.LoadAll<Sprite>("Pause/Items/thumbnails");
  public static Sprite[] itemCombinedThumbnails = Resources.LoadAll<Sprite>("Pause/Items/combined-thumbnails");
  public static Sprite[] itemImages = Resources.LoadAll<Sprite>("Pause/Items/images");
  public static Sprite[] equipmentButtonIcons = Resources.LoadAll<Sprite>("Pause/equipment-icons");
  public static Sprite[] statsIcons = Resources.LoadAll<Sprite>("Pause/stats-icons");
  public static Sprite[] droppableImages = Resources.LoadAll<Sprite>("Spritesheets/droppables");
  public static Sprite[] relicImages = Resources.LoadAll<Sprite>("Spritesheets/relics");
  public static Sprite[] relicItemThumbnails = Resources.LoadAll<Sprite>("Pause/Relics/thumbnails");
  public static Sprite[] relicItemImages = Resources.LoadAll<Sprite>("Pause/Relics/images");
  public static Sprite[] moneyImages = Resources.LoadAll<Sprite>("Spritesheets/money");

  public static Sprite[] breakableBoxes = Resources.LoadAll<Sprite>("Spritesheets/breakables/box");
  public static Sprite[] breakableBarrels = Resources.LoadAll<Sprite>("Spritesheets/breakables/barrel");
  public static Sprite[] breakableJars = Resources.LoadAll<Sprite>("Spritesheets/breakables/jar");
  public static Sprite[] breakableVases = Resources.LoadAll<Sprite>("Spritesheets/breakables/vase");
  public static Sprite[] dwarfSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/dwarf");
  public static Sprite[] goblinSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/goblin");
  public static Sprite[] nymphSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/nymph");
  public static Sprite[] pixieSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/pixie");
  public static Sprite[] skeletonSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/skeleton");
  public static Sprite[] skeletonKingSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/skeleton-king");
  public static Sprite[] trollSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/troll");
  public static Sprite[] unicornSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/unicorn");
  public static Sprite[] centaurSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/centaur");
  public static Sprite[] nereidSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/nereid");
  public static Sprite[] fairySprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/fairy");
  public static Sprite[] phoenixSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/phoenix");
  public static Sprite[] frostbirdSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/frostbird");
  public static Sprite[] thunderbirdSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/thunderbird");
  public static Sprite[] neretSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/neret");
  public static Sprite[] skelewingSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/skelewing");
  public static Sprite[] mummySprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/mummy");
  public static Sprite[] samodivaSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/samodiva");
  public static Sprite[] yanmabelSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/yanmabel");
  public static Sprite[] hippocampusSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/hippocampus");
  public static Sprite[] mermaidSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/mermaid");
  public static Sprite[] mermanSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/merman");
  public static Sprite[] kelpieSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/kelpie");
  public static Sprite[] ogreSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/ogre");
  public static Sprite[] cyclopsSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/cyclops");
  public static Sprite[] faunSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/faun");
  public static Sprite[] dryadSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/dryad");
  public static Sprite[] werewolfSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/werewolf");
  public static Sprite[] leprechaunSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/leprechaun");
  public static Sprite[] gnomeSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/gnome");
  public static Sprite[] nixieSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/nixie");
  public static Sprite[] pishtacoSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/pishtaco");
  public static Sprite[] bunyipSprites = Resources.LoadAll<Sprite>("Spritesheets/enemies/bunyip");
  public static Sprite[] peasantGirlSprites = Resources.LoadAll<Sprite>("Spritesheets/npcs/peasant-girl");

  public static Sprite[] bombSprites = Resources.LoadAll<Sprite>("Spritesheets/bomb");

  public static Sprite[] areaSprites = Resources.LoadAll<Sprite>("Spritesheets/areas");

  public static Sprite[] areaAmbushSprites = Resources.LoadAll<Sprite>("Tilemaps/miscellaneous/ambush");

  public static Dictionary<string, Sprite> ambushInitialSprites = new() {
    {"calderas", areaAmbushSprites[4]},
    {"desert", areaAmbushSprites[8]},
    {"forest", areaAmbushSprites[1]},
    {"glaciers", areaAmbushSprites[5]},
    {"hellscape", areaAmbushSprites[12]},
    {"meadows", areaAmbushSprites[0]},
    {"mountains", areaAmbushSprites[3]},
    {"oceans", areaAmbushSprites[7]},
    {"ruins", areaAmbushSprites[9]},
    {"seaside", areaAmbushSprites[6]},
    {"skyway", areaAmbushSprites[13]},
    {"swamps", areaAmbushSprites[2]},
    {"underground", areaAmbushSprites[11]},
    {"wasteland", areaAmbushSprites[10]},
  };

  public static Dictionary<string, Sprite> equipmentIcons = new() {
    {"body", equipmentButtonIcons[0]},
    {"arm1", equipmentButtonIcons[1]},
    {"arm2", equipmentButtonIcons[5]},
    {"neck", equipmentButtonIcons[4]},
    {"armwear1", equipmentButtonIcons[2]},
    {"armwear2", equipmentButtonIcons[6]},
    {"ring1", equipmentButtonIcons[3]},
    {"ring2", equipmentButtonIcons[7]},
  };

  public static Dictionary<string, Sprite> magicResistances = new() {
    {"air", elements[0]},
    {"dark", elements[1]},
    {"earth", elements[2]},
    {"fire", elements[3]},
    {"ice", elements[4]},
    {"light", elements[5]},
    {"lightning", elements[6]},
    {"water", elements[7]}
  };

  public static Dictionary<string, Sprite> keycodeSprites = new() {
    {"none", keyset[0]},
    {"backspace", keyset[123]},
    {"delete", keyset[1]},
    {"tab", keyset[124]},
    {"clear", keyset[2]},
    {"return", keyset[125]},
    {"pause", keyset[3]},
    {"escape", keyset[4]},
    {"space", keyset[121]},
    {"numpad0", keyset[5]},
    {"numpad1", keyset[6]},
    {"numpad2", keyset[7]},
    {"numpad3", keyset[8]},
    {"numpad4", keyset[9]},
    {"numpad5", keyset[10]},
    {"numpad6", keyset[11]},
    {"numpad7", keyset[12]},
    {"numpad8", keyset[13]},
    {"numpad9", keyset[14]},
    {"numpadPeriod", keyset[15]},
    {"numpadDivide", keyset[16]},
    {"numpadMultiply", keyset[17]},
    {"numpadMinus", keyset[18]},
    {"numpadPlus", keyset[19]},
    {"numpadEnter", keyset[20]},
    {"numpadEquals", keyset[21]},
    {"UpArrow", keyset[22]},
    {"upArrow", keyset[22]},
    {"DownArrow", keyset[23]},
    {"downArrow", keyset[23]},
    {"RightArrow", keyset[24]},
    {"rightArrow", keyset[24]},
    {"LeftArrow", keyset[25]},
    {"leftArrow", keyset[25]},
    {"insert", keyset[26]},
    {"home", keyset[27]},
    {"end", keyset[28]},
    {"pageUp", keyset[29]},
    {"pageDown", keyset[30]},
    {"f1", keyset[31]},
    {"f2", keyset[32]},
    {"f3", keyset[33]},
    {"f4", keyset[34]},
    {"f5", keyset[35]},
    {"f6", keyset[36]},
    {"f7", keyset[37]},
    {"f8", keyset[38]},
    {"f9", keyset[39]},
    {"f10", keyset[40]},
    {"f11", keyset[41]},
    {"f12", keyset[42]},
    {"f13", keyset[43]},
    {"f14", keyset[44]},
    {"f15", keyset[45]},
    {"0", keyset[46]},
    {"1", keyset[47]},
    {"2", keyset[48]},
    {"3", keyset[49]},
    {"4", keyset[50]},
    {"5", keyset[51]},
    {"6", keyset[52]},
    {"7", keyset[53]},
    {"8", keyset[54]},
    {"9", keyset[55]},
    {"exclaim", keyset[56]},
    {"doubleQuote", keyset[57]},
    {"hash", keyset[58]},
    {"dollar", keyset[59]},
    {"percent", keyset[60]},
    {"ampersand", keyset[61]},
    {"quote", keyset[62]},
    {"leftParen", keyset[63]},
    {"rightParen", keyset[64]},
    {"asterisk", keyset[65]},
    {"plus", keyset[66]},
    {"comma", keyset[67]},
    {"minus", keyset[68]},
    {"period", keyset[69]},
    {"slash", keyset[70]},
    {"colon", keyset[71]},
    {"semicolon", keyset[72]},
    {"less", keyset[73]},
    {"equals", keyset[74]},
    {"greater", keyset[75]},
    {"question", keyset[76]},
    {"at", keyset[77]},
    {"leftBracket", keyset[78]},
    {"backslash", keyset[79]},
    {"rightBracket", keyset[80]},
    {"caret", keyset[81]},
    {"underscore", keyset[82]},
    {"backQuote", keyset[83]},
    {"a", keyset[84]},
    {"b", keyset[85]},
    {"c", keyset[86]},
    {"d", keyset[87]},
    {"e", keyset[88]},
    {"f", keyset[89]},
    {"g", keyset[90]},
    {"h", keyset[91]},
    {"i", keyset[92]},
    {"j", keyset[93]},
    {"k", keyset[94]},
    {"l", keyset[95]},
    {"m", keyset[96]},
    {"n", keyset[97]},
    {"o", keyset[98]},
    {"p", keyset[99]},
    {"q", keyset[100]},
    {"r", keyset[101]},
    {"s", keyset[102]},
    {"t", keyset[103]},
    {"u", keyset[104]},
    {"v", keyset[105]},
    {"w", keyset[106]},
    {"x", keyset[107]},
    {"y", keyset[108]},
    {"z", keyset[109]},
    {"leftCurlyBracket", keyset[110]},
    {"pipe", keyset[111]},
    {"rightCurlyBracket", keyset[112]},
    {"tilde", keyset[113]},
    {"numlock", keyset[114]},
    {"capsLock", keyset[126]},
    {"scrollLock", keyset[115]},
    {"rightShift", keyset[122]},
    {"leftShift", keyset[131]},
    {"rightCtrl", keyset[127]},
    {"leftCtrl", keyset[128]},
    {"rightAlt", keyset[129]},
    {"leftAlt", keyset[130]},
    {"leftMeta", keyset[132]},
    {"leftCommand", keyset[133]},
    {"leftApple", keyset[132]},
    {"leftWindows", keyset[134]},
    {"rightMeta", keyset[135]},
    {"rightCommand", keyset[136]},
    {"rightApple", keyset[135]},
    {"rightWindows", keyset[137]},
    {"altGr", keyset[138]},
    {"help", keyset[116]},
    {"printScreen", keyset[117]},
    {"sysReq", keyset[118]},
    {"break", keyset[119]},
    {"menu", keyset[120]},
  };

  public static Dictionary<string, Dictionary<string, Sprite>> gamepadSprites = new() {
    {"playstation", new() {
      {"buttonSouth", keyset[200]},
      {"buttonEast", keyset[199]},
      {"buttonWest", keyset[197]},
      {"buttonNorth", keyset[198]},
      {"leftShoulder", keyset[139]},
      {"rightShoulder", keyset[156]},
      {"leftTrigger", keyset[153]},
      {"rightTrigger", keyset[168]},
      {"startButton", keyset[141]},
      {"selectButton", keyset[140]},
      {"leftStick", keyset[167]},
      {"rightStick", keyset[152]}
    }},
    {"usb gamepad", new() {
      {"button2", keyset[169]},
      {"button3", keyset[170]},
      {"button4", keyset[154]},
      {"button5", keyset[153]},
      {"button6", keyset[168]},
      {"button7", keyset[139]},
      {"button8", keyset[156]},
      {"trigger", keyset[155]},
      {"select", keyset[140]},
      {"start", keyset[141]}
    }},
    {"xbox", new() {
      {"buttonSouth", keyset[212]},
      {"buttonEast", keyset[211]},
      {"buttonWest", keyset[209]},
      {"buttonNorth", keyset[210]},
      {"leftShoulder", keyset[139]},
      {"rightShoulder", keyset[156]},
      {"leftTrigger", keyset[153]},
      {"rightTrigger", keyset[168]},
      {"startButton", keyset[141]},
      {"selectButton", keyset[140]},
      {"leftStick", keyset[167]},
      {"rightStick", keyset[152]}
    }}
  };

  // TODO: update these with the actual image directories once they're done
  public static Dictionary<string, Sprite> areaImages = new() {
    {"calderas", areaSprites[4]},
    {"desert", areaSprites[8]},
    {"forest", areaSprites[1]},
    {"glaciers", areaSprites[5]},
    {"hellscape", areaSprites[12]},
    {"meadows", areaSprites[0]},
    {"mountains", areaSprites[3]},
    {"oceans", areaSprites[7]},
    {"ruins", areaSprites[9]},
    {"seaside", areaSprites[6]},
    {"skyway", areaSprites[13]},
    {"swamps", areaSprites[2]},
    {"underground", areaSprites[11]},
    {"wasteland", areaSprites[10]},
  };

  public static Dictionary<string, Sprite> pauseAvatars = new() {
    {"body-1", Resources.Load<Sprite>("Pause/PlayerAvatars/body-1")}
  };

  // refers to the sprites for throwables - weapons BOTH the player and enemies can throw
  public static Dictionary<string, Sprite> throwableSprites = new() {
    {"axe", Resources.Load<Sprite>("Sprites/axe")},
    {"bomb", bombSprites[0]},
    {"hatchet", Resources.Load<Sprite>("Sprites/hatchet")},
    {"lance", Resources.Load<Sprite>("Sprites/lance")},
    {"skeleton-king-giant-bone", Resources.Load<Sprite>("Sprites/projectiles/skeleton-king-giant-bone")},
    {"knife", Resources.Load<Sprite>("Sprites/knife")},
    {"kunai", Resources.Load<Sprite>("Sprites/kunai")},
    {"shuriken-4", Resources.Load<Sprite>("Sprites/shuriken-4")},
    {"shuriken-6", Resources.Load<Sprite>("Sprites/shuriken-6")},
    {"watermelon", droppableImages[69]},
  };

  // refers to the sprites for projectiles that only enemies (like the shooter) can throw
  public static Dictionary<string, Sprite> projectileSprites = new() {
    {"bunyip-tooth", Resources.Load<Sprite>("Sprites/projectiles/bunyip-tooth")},
    {"centaur-spear", Resources.Load<Sprite>("Sprites/projectiles/centaur-spear")},
    {"cyclops-hillstone", Resources.Load<Sprite>("Sprites/projectiles/cyclops-hillstone")},
    {"dryad-twig", Resources.Load<Sprite>("Sprites/projectiles/dryad-twig")},
    {"dwarf-cobble", Resources.Load<Sprite>("Sprites/projectiles/dwarf-cobble")},
    {"fairy-blast", Resources.Load<Sprite>("Sprites/projectiles/fairy-blast")},
    {"faun-horn", Resources.Load<Sprite>("Sprites/projectiles/faun-horn")},
    {"frostbird-orb", Resources.Load<Sprite>("Sprites/projectiles/frostbird-orb")},
    {"gnome-truffle", Resources.Load<Sprite>("Sprites/projectiles/gnome-truffle")},
    {"goblin-knife", Resources.Load<Sprite>("Sprites/projectiles/goblin-knife")},
    {"hippocampus-scale", Resources.Load<Sprite>("Sprites/projectiles/hippocampus-scale")},
    {"kelpie-fin", Resources.Load<Sprite>("Sprites/projectiles/kelpie-fin")},
    {"leprechaun-mushroom", Resources.Load<Sprite>("Sprites/projectiles/leprechaun-mushroom")},
    {"mermaid-scale", Resources.Load<Sprite>("Sprites/projectiles/mermaid-scale")},
    {"merman-scale", Resources.Load<Sprite>("Sprites/projectiles/merman-scale")},
    {"mummy-rib", Resources.Load<Sprite>("Sprites/projectiles/mummy-rib")},
    {"nereid-seashell", Resources.Load<Sprite>("Sprites/projectiles/nereid-seashell")},
    {"neret-orb", Resources.Load<Sprite>("Sprites/projectiles/neret-orb")},
    {"nixie-cattail", Resources.Load<Sprite>("Sprites/projectiles/nixie-cattail")},
    {"nymph-acorn", Resources.Load<Sprite>("Sprites/projectiles/nymph-acorn")},
    {"ogre-stump", Resources.Load<Sprite>("Sprites/projectiles/ogre-stump")},
    {"phoenix-orb", Resources.Load<Sprite>("Sprites/projectiles/phoenix-orb")},
    {"pishtaco-vertebra", Resources.Load<Sprite>("Sprites/projectiles/pishtaco-vertebra")},
    {"pixie-fireball", Resources.Load<Sprite>("Sprites/projectiles/pixie-fireball")},
    {"samodiva-stalagtip", Resources.Load<Sprite>("Sprites/projectiles/samodiva-stalagtip")},
    {"skeleton-bone", Resources.Load<Sprite>("Sprites/projectiles/skeleton-bone")},
    {"skelewing-orb", Resources.Load<Sprite>("Sprites/projectiles/skelewing-orb")},
    {"thunderbird-orb", Resources.Load<Sprite>("Sprites/projectiles/thunderbird-orb")},
    {"troll-boulder", Resources.Load<Sprite>("Sprites/projectiles/troll-boulder")},
    {"unicorn-shard", Resources.Load<Sprite>("Sprites/projectiles/unicorn-shard")},
    {"werewolf-fang", Resources.Load<Sprite>("Sprites/projectiles/werewolf-fang")},
    {"yanmabel-stinger", Resources.Load<Sprite>("Sprites/projectiles/yanmabel-stinger")}
  };

  public static Dictionary<string, Sprite> droppableSprites = new() {
    {"basic-longsword", droppableImages[0]},
    {"basic-sword", droppableImages[1]},
    {"basic-shield", droppableImages[2]},
    {"chicken-drumstick", droppableImages[3]},
    {"love-necklace", droppableImages[4]},
    {"solomon-ring", droppableImages[5]},
    {"ra-ring", droppableImages[6]},
    {"skull-ring", droppableImages[7]},
    {"gold-bracelet", droppableImages[8]},
    {"silver-bracelet", droppableImages[9]},
    {"rabbit-paw", droppableImages[10]},
    {"moonlight-pendant", droppableImages[11]},
    {"rainbow-bracer", droppableImages[12]},
    {"body-1", droppableImages[13]},
    {"lance", droppableImages[14]},
    {"axe", droppableImages[15]},
    {"hatchet", droppableImages[16]},
    {"shuriken-6", droppableImages[17]},
    {"shuriken-4", droppableImages[18]},
    {"knife", droppableImages[19]},
    {"kunai", droppableImages[20]},
    {"basic-bow", droppableImages[21]},
    {"arrow-standard", droppableImages[22]},
    {"arrow-poison", droppableImages[23]},
    {"arrow-fire", droppableImages[24]},
    {"bomb", droppableImages[25]},
    {"diamond", droppableImages[26]},
    {"emerald", droppableImages[27]},
    {"sapphire", droppableImages[28]},
    {"citrine", droppableImages[29]},
    {"ruby", droppableImages[30]},
    {"opal", droppableImages[31]},
    {"amethyst", droppableImages[32]},
    {"gold-bar", droppableImages[33]},
    {"gold-ingot", droppableImages[34]},
    {"pearl", droppableImages[35]},
    {"potion", droppableImages[36]},
    {"mid-potion", droppableImages[37]},
    {"high-potion", droppableImages[38]},
    {"magic-ampoule", droppableImages[39]},
    {"magic-vial", droppableImages[40]},
    {"magic-bottle", droppableImages[41]},
    {"strength-flask", droppableImages[42]},
    {"stamina-flask", droppableImages[43]},
    {"critical-flask", droppableImages[44]},
    {"luck-flask", droppableImages[45]},
    {"random-flask", droppableImages[46]},
    {"theriac", droppableImages[47]},
    {"hashish", droppableImages[48]},
    {"earth-med", droppableImages[49]},
    {"air-med", droppableImages[50]},
    {"water-med", droppableImages[51]},
    {"fire-med", droppableImages[52]},
    {"lightning-med", droppableImages[53]},
    {"ice-med", droppableImages[54]},
    {"light-med", droppableImages[55]},
    {"dark-med", droppableImages[56]},
    {"elixir", droppableImages[57]},
    {"apple", droppableImages[58]},
    {"banana", droppableImages[59]},
    {"orange", droppableImages[60]},
    {"pear", droppableImages[61]},
    {"strawberry", droppableImages[62]},
    {"cherry", droppableImages[63]},
    {"grapes", droppableImages[64]},
    {"pineapple", droppableImages[65]},
    {"mango", droppableImages[66]},
    {"coconut", droppableImages[67]},
    {"honeydew", droppableImages[68]},
    {"watermelon", droppableImages[69]},
    {"watermelon-slice", droppableImages[70]},
    {"silver-bar", droppableImages[71]},
    {"silver-ingot", droppableImages[72]},
    {"bronze-bar", droppableImages[73]},
    {"bronze-ingot", droppableImages[74]},
    {"skull", droppableImages[75]},
    {"polished-skull", droppableImages[76]},
    {"silver-skull", droppableImages[77]},
    {"calcite", droppableImages[78]},
    {"refined-calcite", droppableImages[79]},
    {"precious-calcite", droppableImages[80]},
    {"darkling-sword", droppableImages[81]},
    {"darklord-sword", droppableImages[82]},
    {"spices", droppableImages[83]},
    {"textiles", droppableImages[84]},
    {"bandit-knife", droppableImages[85]},
    {"bandit-rapier", droppableImages[86]},
    {"laurel-crown", droppableImages[87]},
    {"flower-crown", droppableImages[88]},
    {"flower-wreath", droppableImages[89]},
    {"vine-staff", droppableImages[90]},
    {"vine-scepter", droppableImages[91]},
    {"wine", droppableImages[92]},
    {"reinforced-axe", droppableImages[93]},
    {"dwarf-hammer", droppableImages[95]},
    {"war-maul", droppableImages[95]},
    {"bronze-pixie-belt", droppableImages[96]},
    {"silver-pixie-belt", droppableImages[97]},
    {"gold-pixie-belt", droppableImages[98]},
    {"draco-shield", droppableImages[99]},
    {"king-skull", droppableImages[100]},
    {"polished-king-skull", droppableImages[101]},
    {"silver-king-skull", droppableImages[102]},
    {"royal-pelt", droppableImages[103]},
    {"skeleton-king-giant-bone", droppableImages[104]}
  };

  public static Dictionary<string, Sprite> relicSprites = new() {
    {"dawn-gem", relicImages[3]},
    {"magic-talisman", relicImages[1]},
    {"royal-lamp", relicImages[2]},
    {"sundrop", relicImages[4]},
    {"swift-boots", relicImages[0]},
  };

  public static Dictionary<string, Sprite> breakableSprites = new() {
    {"barrel", breakableBarrels[0]},
    {"box", breakableBoxes[0]},
    {"jar", breakableJars[0]},
    {"vase", breakableVases[0]}
  };

  public static Dictionary<string, Sprite> uiElements = new() {
    {"checkbox-checked", Resources.Load<Sprite>("Sprites/UI/Pause/checkbox-checked")},
    {"checkbox-unchecked", Resources.Load<Sprite>("Sprites/UI/Pause/checkbox-unchecked")},
    {"radio-checked", Resources.Load<Sprite>("Sprites/UI/Pause/radio-checked")},
    {"radio-unchecked", Resources.Load<Sprite>("Sprites/UI/Pause/radio-unchecked")}
  };

  public static Dictionary<string, Sprite> firstBossSprites = new() {
    {"bunyip", bunyipSprites[0]},
    {"centaur", centaurSprites[0]},
    {"cyclops", cyclopsSprites[0]},
    {"dryad", dryadSprites[0]},
    {"dwarf", dwarfSprites[0]},
    {"fairy", fairySprites[0]},
    {"faun", faunSprites[0]},
    {"frostbird", frostbirdSprites[0]},
    {"gnome", gnomeSprites[0]},
    {"goblin", goblinSprites[0]},
    {"hippocampus", hippocampusSprites[0]},
    {"kelpie", kelpieSprites[0]},
    {"leprechaun", leprechaunSprites[0]},
    {"mermaid", mermaidSprites[0]},
    {"merman", mermanSprites[0]},
    {"mummy", mummySprites[0]},
    {"nereid", nereidSprites[0]},
    {"neret", neretSprites[0]},
    {"nixie", nixieSprites[0]},
    {"nymph", nymphSprites[0]},
    {"ogre", ogreSprites[0]},
    {"phoenix", phoenixSprites[0]},
    {"pishtaco", pishtacoSprites[0]},
    {"pixie", pixieSprites[0]},
    {"samodiva", samodivaSprites[0]},
    {"skeleton", skeletonSprites[35]},
    {"skeleton-king", skeletonKingSprites[0]},
    {"skelewing", skelewingSprites[0]},
    {"thunderbird", thunderbirdSprites[0]},
    {"troll", trollSprites[0]},
    {"unicorn", unicornSprites[0]},
    {"werewolf", werewolfSprites[0]},
    {"yanmabel", yanmabelSprites[0]}
  };

  public static Dictionary<string, Dictionary<string, Sprite>> emotions = new() {
    {"peasant-girl", new() {
      {"affective", peasantGirlSprites[15]},
      {"afraid", peasantGirlSprites[7]},
      {"angry", peasantGirlSprites[19]},
      {"ashamed", peasantGirlSprites[21]},
      {"confused", peasantGirlSprites[23]},
      {"default", peasantGirlSprites[0]},
      {"happy", peasantGirlSprites[16]},
      {"jealous", peasantGirlSprites[22]},
      {"lonely", peasantGirlSprites[18]},
      {"sad", peasantGirlSprites[17]},
      {"surprised", peasantGirlSprites[20]}
    }}
  };

  public static Dictionary<string, Sprite> arrows = new() {
    {"arrow-fire", Resources.Load<Sprite>("Sprites/arrow-fire")},
    {"arrow-poison", Resources.Load<Sprite>("Sprites/arrow-poison")},
    {"arrow-standard", Resources.Load<Sprite>("Sprites/arrow-standard")}
  };
}
