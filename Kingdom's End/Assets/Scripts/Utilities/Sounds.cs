using System.Collections.Generic;
using UnityEngine;

public class Sounds {
  public static Dictionary<string, Dictionary<string, AudioClip[]>> runningSounds = new() {
    {"bedrock", new() {
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/bedrock/boots-1"),
        Resources.Load<AudioClip>("SFX/running/bedrock/boots-2"),
        Resources.Load<AudioClip>("SFX/running/bedrock/boots-3"),
        Resources.Load<AudioClip>("SFX/running/bedrock/boots-4"),
        Resources.Load<AudioClip>("SFX/running/bedrock/boots-5"),
      }}
    }},
    {"bentgrass", new() {
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/bentgrass/boots-1"),
        Resources.Load<AudioClip>("SFX/running/bentgrass/boots-2"),
        Resources.Load<AudioClip>("SFX/running/bentgrass/boots-3"),
        Resources.Load<AudioClip>("SFX/running/bentgrass/boots-4"),
        Resources.Load<AudioClip>("SFX/running/bentgrass/boots-5"),
      }}
    }},
    {"dirt", new() {
      {"barefoot", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/dirt/barefoot-1"),
        Resources.Load<AudioClip>("SFX/running/dirt/barefoot-2"),
        Resources.Load<AudioClip>("SFX/running/dirt/barefoot-3"),
        Resources.Load<AudioClip>("SFX/running/dirt/barefoot-4"),
        Resources.Load<AudioClip>("SFX/running/dirt/barefoot-5"),
      }},
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/dirt/boots-1"),
        Resources.Load<AudioClip>("SFX/running/dirt/boots-2"),
        Resources.Load<AudioClip>("SFX/running/dirt/boots-3"),
        Resources.Load<AudioClip>("SFX/running/dirt/boots-4"),
        Resources.Load<AudioClip>("SFX/running/dirt/boots-5"),
      }}
    }},
    {"grass", new() {
      {"barefoot", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/grass/barefoot-1"),
        Resources.Load<AudioClip>("SFX/running/grass/barefoot-2"),
        Resources.Load<AudioClip>("SFX/running/grass/barefoot-3"),
        Resources.Load<AudioClip>("SFX/running/grass/barefoot-4"),
        Resources.Load<AudioClip>("SFX/running/grass/barefoot-5"),
      }},
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/grass/boots-1"),
        Resources.Load<AudioClip>("SFX/running/grass/boots-2"),
        Resources.Load<AudioClip>("SFX/running/grass/boots-3"),
        Resources.Load<AudioClip>("SFX/running/grass/boots-4"),
        Resources.Load<AudioClip>("SFX/running/grass/boots-5"),
      }}
    }},
    {"gravel", new() {
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/gravel/boots-1"),
        Resources.Load<AudioClip>("SFX/running/gravel/boots-2"),
        Resources.Load<AudioClip>("SFX/running/gravel/boots-3"),
        Resources.Load<AudioClip>("SFX/running/gravel/boots-4"),
        Resources.Load<AudioClip>("SFX/running/gravel/boots-5"),
      }}
    }},
    {"ice", new() {
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/ice/boots-1"),
        Resources.Load<AudioClip>("SFX/running/ice/boots-2"),
        Resources.Load<AudioClip>("SFX/running/ice/boots-3"),
        Resources.Load<AudioClip>("SFX/running/ice/boots-4"),
        Resources.Load<AudioClip>("SFX/running/ice/boots-5"),
      }}
    }},
    {"sand", new() {
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/sand/boots-1"),
        Resources.Load<AudioClip>("SFX/running/sand/boots-2"),
        Resources.Load<AudioClip>("SFX/running/sand/boots-3"),
        Resources.Load<AudioClip>("SFX/running/sand/boots-4"),
        Resources.Load<AudioClip>("SFX/running/sand/boots-5"),
      }}
    }},
    {"snow", new() {
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/snow/boots-1"),
        Resources.Load<AudioClip>("SFX/running/snow/boots-2"),
        Resources.Load<AudioClip>("SFX/running/snow/boots-3"),
        Resources.Load<AudioClip>("SFX/running/snow/boots-4"),
        Resources.Load<AudioClip>("SFX/running/snow/boots-5"),
      }}
    }},
    {"tile", new() {
      // TODO: consider if at any time barefoot tile sound effects will be needed
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/tile/boots-1"),
        Resources.Load<AudioClip>("SFX/running/tile/boots-2"),
        Resources.Load<AudioClip>("SFX/running/tile/boots-3"),
        Resources.Load<AudioClip>("SFX/running/tile/boots-4"),
        Resources.Load<AudioClip>("SFX/running/tile/boots-5"),
      }}
    }},
    {"wetland", new() {
      {"boots", new AudioClip[] {
        Resources.Load<AudioClip>("SFX/running/wetland/boots-1"),
        Resources.Load<AudioClip>("SFX/running/wetland/boots-2"),
        Resources.Load<AudioClip>("SFX/running/wetland/boots-3"),
        Resources.Load<AudioClip>("SFX/running/wetland/boots-4"),
        Resources.Load<AudioClip>("SFX/running/wetland/boots-5"),
      }}
    }},
  };

  public static AudioClip[] woodBreakingSounds = new AudioClip[] {
    Resources.Load<AudioClip>("SFX/breakables/box/breaking/breaking-1"),
    Resources.Load<AudioClip>("SFX/breakables/box/breaking/breaking-2"),
    Resources.Load<AudioClip>("SFX/breakables/box/breaking/breaking-3"),
  };

  // TODO: given their rarity, consider if these need any more clips to randomize
  public static AudioClip[] jarBreakingSounds = new AudioClip[] {
    Resources.Load<AudioClip>("SFX/breakables/jar/breaking-1")
  };
  public static AudioClip[] vaseBreakingSounds = new AudioClip[] {
    Resources.Load<AudioClip>("SFX/breakables/vase/breaking-1")
  };

  public static Dictionary<string, AudioClip[]> breakableSounds = new() {
    {"barrel", woodBreakingSounds},
    {"box", woodBreakingSounds},
    {"jar", jarBreakingSounds},
    {"vase", vaseBreakingSounds}
  };

  // TODO: add other rock explosion sounds (e.g. for meadows) when rune pillars are implemented
  public static Dictionary<string, AudioClip> rockExplosionSounds = new() {
    {"ice", Resources.Load<AudioClip>("SFX/breakables/vase/breaking-1")}
  };

  public static Dictionary<string, AudioClip> woodFallingSounds = new() {
    {"barrel", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-box")},
    {"bedrock", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-bedrock")},
    {"bentgrass", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-bentgrass")},
    {"box", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-box")},
    {"dirt", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-dirt")},
    {"grass", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-grass")},
    {"gravel", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-gravel")},
    {"ice", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-ice")},
    {"item", Resources.Load<AudioClip>("SFX/items/falling/on-box")},
    {"sand", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-sand")},
    {"snow", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-snow")},
    {"wetland", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-wetland")}
  };

  // falling sounds for anything which base material is not changeable
  public static Dictionary<string, Dictionary<string, AudioClip>> fallingSounds = new() {
    {"barrel", woodFallingSounds},
    {"box", woodFallingSounds},
    {"character", new() {
      {"bedrock", Resources.Load<AudioClip>("SFX/falling/bedrock/boots")},
      {"bentgrass", Resources.Load<AudioClip>("SFX/falling/bentgrass/boots")},
      {"box", Resources.Load<AudioClip>("SFX/falling/breakables/box/boots")},
      {"dirt", Resources.Load<AudioClip>("SFX/falling/dirt/boots")},
      {"grass", Resources.Load<AudioClip>("SFX/falling/grass/boots")},
      {"gravel", Resources.Load<AudioClip>("SFX/falling/gravel/boots")},
      {"ice", Resources.Load<AudioClip>("SFX/falling/ice/boots")},
      {"sand", Resources.Load<AudioClip>("SFX/falling/sand/boots")},
      {"snow", Resources.Load<AudioClip>("SFX/falling/snow/boots")},
      {"wetland", Resources.Load<AudioClip>("SFX/falling/wetland/boots")}
    }}
  };

  public static Dictionary<string, Dictionary<string, AudioClip>> characterFallingSounds = new() {
    {"barrel", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/breakables/box/boots")}
    }},
    {"bedrock", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/bedrock/boots")}
    }},
    {"bentgrass", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/bentgrass/boots")}
    }},
    {"box", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/breakables/box/boots")}
    }},
    {"dirt", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/dirt/boots")}
    }},
    {"grass", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/grass/boots")}
    }},
    {"gravel", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/gravel/boots")}
    }},
    {"ice", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/ice/boots")}
    }},
    {"jar", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/breakables/box/boots")}
    }},
    {"sand", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/sand/boots")}
    }},
    {"snow", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/snow/boots")}
    }},
    {"tile", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/tile/boots")}
    }},
    {"vase", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/breakables/box/boots")}
    }},
    {"wetland", new() {
      {"boots", Resources.Load<AudioClip>("SFX/falling/wetland/boots")}
    }},
  };

  public static Dictionary<string, AudioClip> droppableFallingSounds = new() {
    {"bedrock", Resources.Load<AudioClip>("SFX/items/falling/on-bedrock")},
    {"bentgrass", Resources.Load<AudioClip>("SFX/items/falling/on-bentgrass")},
    {"dirt", Resources.Load<AudioClip>("SFX/items/falling/on-dirt")},
    {"grass", Resources.Load<AudioClip>("SFX/items/falling/on-grass")},
    {"gravel", Resources.Load<AudioClip>("SFX/items/falling/on-gravel")},
    {"ice", Resources.Load<AudioClip>("SFX/items/falling/on-ice")},
    {"sand", Resources.Load<AudioClip>("SFX/items/falling/on-sand")},
    {"snow", Resources.Load<AudioClip>("SFX/items/falling/on-snow")},
    {"tile", Resources.Load<AudioClip>("SFX/items/falling/on-tile")},
    {"wetland", Resources.Load<AudioClip>("SFX/items/falling/on-wetland")},
    {"interactable", Resources.Load<AudioClip>("SFX/breakables/box/falling/on-box")}
  };

  // TODO: since not all sounds of a type will involve weapons of the same material (e.g. not all singles will be swords),
  // at some point some differentiation will be needed
  public static Dictionary<string, AudioClip> attackSounds = new() {
    {"bow", Resources.Load<AudioClip>("SFX/weapons/bow/basic")},
    {"double", Resources.Load<AudioClip>("SFX/weapons/double/sword")},
    {"single", Resources.Load<AudioClip>("SFX/weapons/single/sword")},
    {"throwable-small", Resources.Load<AudioClip>("SFX/weapons/throwable/small")},
    {"throwable-food-middle", Resources.Load<AudioClip>("SFX/weapons/throwable/middle")},
    {"throwable-middle", Resources.Load<AudioClip>("SFX/weapons/throwable/middle")},
    {"throwable-double-large", Resources.Load<AudioClip>("SFX/weapons/throwable-double/large")},
    {"punch", Resources.Load<AudioClip>("SFX/unequipped/punch")},
    {"kick", Resources.Load<AudioClip>("SFX/unequipped/kick")},
  };

  public static Dictionary<string, Dictionary<string, AudioClip>> impactSounds = new() {
    {"punch", new() {
      {"normal", Resources.Load<AudioClip>("SFX/hitting/normal/punch")},
      {"critical", Resources.Load<AudioClip>("SFX/hitting/critical/punch")}
    }},
    {"kick", new() {
      {"normal", Resources.Load<AudioClip>("SFX/hitting/normal/kick")},
      {"critical", Resources.Load<AudioClip>("SFX/hitting/critical/kick")}
    }},
    {"sword", new() {
      {"normal", Resources.Load<AudioClip>("SFX/hitting/normal/sword")},
      {"critical", Resources.Load<AudioClip>("SFX/hitting/critical/sword")}
    }},
    {"throwable-small", new() {
      {"normal", Resources.Load<AudioClip>("SFX/hitting/normal/throwable-small")},
      {"critical", Resources.Load<AudioClip>("SFX/hitting/critical/throwable-small")}
    }},
    {"throwable-middle", new() {
      {"normal", Resources.Load<AudioClip>("SFX/hitting/normal/throwable-middle")},
      {"critical", Resources.Load<AudioClip>("SFX/hitting/critical/throwable-middle")}
    }},
    {"throwable-double-large", new() {
      {"normal", Resources.Load<AudioClip>("SFX/hitting/normal/throwable-double-large")},
      {"critical", Resources.Load<AudioClip>("SFX/hitting/critical/throwable-double-large")}
    }},
    {"throwable-fruit", new() {
      {"normal", Resources.Load<AudioClip>("SFX/hitting/normal/throwable-fruit")},
      {"critical", Resources.Load<AudioClip>("SFX/hitting/critical/throwable-fruit")}
    }},
    {"arrow", new() {
      {"normal", Resources.Load<AudioClip>("SFX/hitting/normal/arrow")},
      {"critical", Resources.Load<AudioClip>("SFX/hitting/critical/arrow")}
    }},
    {"blunt", new() {
      {"normal", Resources.Load<AudioClip>("SFX/hitting/normal/blunt")},
      {"critical", Resources.Load<AudioClip>("SFX/hitting/critical/blunt")}
    }},
    {"explosion", new() {
      {"normal", Resources.Load<AudioClip>("SFX/explosions/basic")},
      {"critical", Resources.Load<AudioClip>("SFX/explosions/basic")}
    }},
  };

  // TODO: modify these as soon as more sounds are added
  public static Dictionary<string, AudioClip> ambushFloorSounds = new() {
    {"calderas", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"desert", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"forest", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"glaciers", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"hellscape", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"meadows", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"mountains", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"oceans", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"ruins", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"seaside", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"skyway", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"swamps", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"underground", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")},
    {"wasteland", Resources.Load<AudioClip>("SFX/falling/grass/barefoot")}
  };

  // TODO: modify bewitch sound when more sounds are added
  public static AudioClip bewitchSound = Resources.Load<AudioClip>("SFX/hitting/critical/arrow") as AudioClip;
  public static AudioClip iceblockSound = Resources.Load<AudioClip>("SFX/weapons/bow/ice") as AudioClip;

  public static Dictionary<string, AudioClip> blockSounds = new() {
    {"basic", Resources.Load<AudioClip>("SFX/hitting/block/basic")}
  };

  public static Dictionary<string, AudioClip> explosionSounds = new() {
    {"arrow", Resources.Load<AudioClip>("SFX/explosions/arrow")},
    {"bomb", Resources.Load<AudioClip>("SFX/explosions/basic")},
    {"damage", Resources.Load<AudioClip>("SFX/explosions/basic")},
    {"enemy", Resources.Load<AudioClip>("SFX/explosions/enemy")}
  };

  public static Dictionary<string, AudioClip> menuSounds = new() {
    {"attention", Resources.Load<AudioClip>("SFX/menu/attention")},
    {"back", Resources.Load<AudioClip>("SFX/menu/back")},
    {"move", Resources.Load<AudioClip>("SFX/menu/move")},
    {"select", Resources.Load<AudioClip>("SFX/menu/select")},
    {"use", Resources.Load<AudioClip>("SFX/menu/use")}
  };

  public static Dictionary<string, AudioClip> itemPickSounds = new() {
    {"money", Resources.Load<AudioClip>("SFX/items/pick/money")},
    {"normal", Resources.Load<AudioClip>("SFX/items/pick/normal")},
    {"rare", Resources.Load<AudioClip>("SFX/items/pick/rare")}
  };

  public static Dictionary<string, AudioClip> chestSounds = new() {
    {"chest-small", Resources.Load<AudioClip>("SFX/chests/small")},
    {"chest-large", Resources.Load<AudioClip>("SFX/chests/large")},
  };

  public static Dictionary<string, AudioClip> notificationSounds = new() {
    {"levelup", Resources.Load<AudioClip>("SFX/notifications/levelup")},
  };

  public static Dictionary<string, AudioClip> poisonSounds = new() {
    {"basic", Resources.Load<AudioClip>("SFX/poison/basic")},
  };

  public static Dictionary<string, AudioClip> meleeSounds = new() {
    {"smash", Resources.Load<AudioClip>("SFX/melee/smash")},
  };

  public static Dictionary<string, AudioClip> loops = new() {
    {"arrow-burn", Resources.Load<AudioClip>("SFX/loops/arrow-burn")},
  };

  public static Dictionary<string, AudioClip> introSounds = new() {
    {"last", Resources.Load<AudioClip>("SFX/intro/last")},
    {"suspense", Resources.Load<AudioClip>("SFX/intro/suspense")}
  };

  // TODO: modify soundtrack directories once songs are ready
  public static Dictionary<string, AudioClip> soundtracks = new() {
    {"calderas", Resources.Load<AudioClip>("OST/meadows-1")},
    {"desert", Resources.Load<AudioClip>("OST/meadows-1")},
    {"forest", Resources.Load<AudioClip>("OST/meadows-1")},
    {"glaciers", Resources.Load<AudioClip>("OST/meadows-1")},
    {"hellscape", Resources.Load<AudioClip>("OST/meadows-1")},
    {"meadows", Resources.Load<AudioClip>("OST/meadows-1")},
    {"mountains", Resources.Load<AudioClip>("OST/meadows-1")},
    {"oceans", Resources.Load<AudioClip>("OST/meadows-1")},
    {"ruins", Resources.Load<AudioClip>("OST/meadows-1")},
    {"seaside", Resources.Load<AudioClip>("OST/meadows-1")},
    {"skyway", Resources.Load<AudioClip>("OST/meadows-1")},
    {"swamps", Resources.Load<AudioClip>("OST/meadows-1")},
    {"underground", Resources.Load<AudioClip>("OST/meadows-1")},
    {"wasteland", Resources.Load<AudioClip>("OST/meadows-1")},
    {"miniboss", Resources.Load<AudioClip>("OST/miniboss-1")}
  };

  public static AudioClip gameOverSound = Resources.Load<AudioClip>("SFX/gameover/game-over");
}
