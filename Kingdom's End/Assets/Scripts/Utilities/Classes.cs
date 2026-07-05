using System;
using UnityEngine;

public class Classes {}

public class WeaponDamage {
  public int damage;
  // TODO: include property for when a weapon is allowed to damage so long as collision happens, not just on enter
}

public class Item {
  public string key { get; set; }
  public int amount { get; set; }

  public Item(string key, int amount) {
    this.key = key;
    this.amount = amount;
  }
}

public class ProbabilityItem {
  public string key;
  public float probability;
}

[Serializable]
public class Consumable {
  public string key;
  public float duration;
  public float useTime;
}

public class MagicResistance {
  public string name;
  public string type; // add or remove
}

public class Effects {
  public float? duration; // to be measured in seconds
  public int? hp;
  public float? hpPercentage;
  public int? mp;
  public float? mpPercentage;
  public string[] statusHeal;
  public int? atk;
  public int? def;
  public float? crit; // number between 0 and 1
  public float? luck;
  public int? speed; // multiplier for speed (movement and animation) increased/decreased
  public float? jumpHeight; // multiplier for jump height increased/decreased
  public int? stamina; //multiplier for stamina
  public string? status; // for consumables which display a status in the wheel
  public MagicResistance[] magicResistances;
}

public class EffectItem {
  public int spriteIndex;
  public string key;
  public object value; // since it can be int or float
}

public class RelicEffect {
  public string type;
  public string value;

  public RelicEffect(string type, string value) {
    this.type = type;
    this.value = value;
  }
}

public class PauseItem {
  public Sprite thumbnail;
  public Sprite image;
  public string name;
  public string description;

  public PauseItem(Sprite thumbnail, Sprite image, string name, string description) {
    this.thumbnail = thumbnail;
    this.image = image;

    if (name.Length > Constants.maxItemNameLength) {
      throw new Exception("An item name (\"" + name + "\") must not exceed " + Constants.maxItemNameLength + " characters");
    } else {
      this.name = name;
    }

    if (description.Length > Constants.maxItemDescriptionLength) {
      throw new Exception("An item description (\"" + description + "\") must not exceed " + Constants.maxItemDescriptionLength + " characters");
    } else {
      this.description = description;
    }
  }
}

public class RegularItem : PauseItem {
  public string type;
  public int price;
  public Effects effects;

  public RegularItem(Sprite thumbnail, Sprite image, string name, string description, string type, int price, Effects effects = null) : base(thumbnail, image, name, description) {
    this.type = type;
    this.price = price;

    if (effects != null) {
      this.effects = effects;
    }
  }

  public float? GetEffectValue(string key) {
    if (effects == null) return null;

    return key switch {
        "hp"            => effects.hp,
        "hpPercentage"  => effects.hpPercentage,
        "mp"            => effects.mp,
        "mpPercentage"  => effects.mpPercentage,
        "atk"           => effects.atk,
        "def"           => effects.def,
        "crit"          => effects.crit,
        "luck"          => effects.luck,
        _               => null
    };
  }

  // TODO: add values to increase player stats (atk, def, two-handed, etc.)
}

public class RelicItem : PauseItem {
  public RelicEffect effect;

  public RelicItem(Sprite thumbnail, Sprite image, string name, string description, RelicEffect effect) : base(thumbnail, image, name, description) {
    this.effect = effect;
  }
}

public class CompositePauseImage {
  public Sprite thumbnail;
  public string name;
}

public class HeroMagicResistance {
  public string name;
  public int frequency;
}

public class BreakableDimension {
  public Vector2 offset;
  public Vector2 size;
}

public class MoneyItem {
  public Sprite image;
  public int increment;
  public string text;
}

public class EnemyHealth {
  public int current;
  public int maximum;

  public EnemyHealth(int current, int maximum) {
    this.current = current;
    this.maximum = maximum;
  }
}

public class EnemyStats {
  public string name;
  public string form;
  public string baseMaterial;
  public string normalAttackType;
  public int hp;
  public int atk;
  public int def;
  public float crit;
  public int exp;
  public int speed;
  public float reach;
  public float longReach;
  public float edgeCastLength;
  public float arrowBurnPosition;
  public float mass;
}

public class Outcome {
  public string outcomeCase;
  public string outcomeValue;
}

public class ChatLine {
  public string character;
  public string emotion;
  public string line;
  public Outcome outcome;
  public Decision decision;

  public ChatLine(string character, string emotion, string line, Outcome outcome, Decision decision = null) {
    this.character = character;
    this.emotion = emotion;
    this.outcome = outcome;
    this.decision = decision;

    if (line.Length > Constants.maxChatLineLength) {
      throw new Exception($"A chat line for the character {character} (emotion: {emotion}) exceeds {Constants.maxChatLineLength} characters. Line: '{line}'");
    } else {
      this.line = line;
    }
  }
}

public class Decision {
  public string yes;
  public string no;

  public string Get(string key) {
    return key switch {
      "yes" => yes,
      "no" => no,
      _ => null,
    };
  }

  public string GetOther(string key) {
    return key switch {
      "yes" => no,
      "no" => yes,
      _ => null,
    };
  }
}

public class MessageLine {
  public string line;
  public Outcome outcome; // needed if any message (like finding a note on a desk) allows the player to get an item, e.g. "Under this paper you will find a key", thus the player gets a key
}

public class Condition {
  public string conditionCheck;
  public string conditionValue;
}

public class ChatNode {
  public Condition nodeCondition;
  public ChatLine[] nodeLines;
  public string nextNode;
  public string fallbackNode;
  public bool continueOnLeave;
}

public class FragmentOutcome {
  public string key;
  public int count;
}

public class ValuePair {
  public float x;
  public float y;

  public ValuePair(float x, float y) {
    this.x = x;
    this.y = y;
  }
}

public class ThrowableSpecs {
  public ValuePair colliderOffset;
  public ValuePair colliderSize;
  public bool freezeRotation;
  public float hDisplacement;
  public ValuePair initialRotationValues;
  public float maxHeight;
  public int? rotationFactor;
  public float speed;
  public float steepness;
}

public class RuneSpecs {
  public ValuePair position;
  public ValuePair scale;
}

public class ZoneSpecs {
  public float animSpeed; // how fast the player changes frames in an animation
  public string groundMaterial; // different material type for running sounds
  public float jumpHeight; // how high the player jumps
  public float moveFriction; // how fast the player stops moving
  public float moveSpeed; // how fast the player moves
}

public class AreaGradients {
  public Gradient sky;
  public Gradient ground;
  public Gradient buildings;
}

public class RemapInput {
  public string keyCode { get; set; }       // The control name (e.g., "buttonSouth", "a", "leftButton")
  public string deviceName { get; set; }    // The device name (e.g., "Xbox Controller", "Keyboard")
  public bool isGamepad { get; set; }
  public bool isForbidden { get; set; }
}
