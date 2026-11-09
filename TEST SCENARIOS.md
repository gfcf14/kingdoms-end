# Test Scenarios

## Purpose
Manually verify Hero movement and interactions with other objects.

**Build/commit:** __________  
**Platform:** __________

## Walking / Movement

### WM-01: Walk downhill on inclines and descents

**Steps**
1. Walk down an incline and then down a descent, testing both travel directions where possible.
2. Repeat at normal movement speed.

**Expected**
- Movement follows the terrain smoothly rather than appearing to fall in steps.
- No fall animation while the Hero remains supported by the slope.

### WM-02: Dropkick onto and along a slope

**Steps**
1. Jump, then press down and jump to dropkick toward an incline or descent.
2. After landing, walk downhill; repeat with the dropkick facing both uphill and downhill.

**Expected**
- The dropkick lands without repeated fall transitions.
- Walking downhill afterward follows the terrain smoothly.

### WM-03: Traverse low incline/descent hill

**Steps**
1. Starting on the left, walk up the incline to the upper flat section.
2. Continue down the descent to the lower flat section.
3. Turn around and walk back over the hill.

**Expected**
- No brief fall animation when reaching either flat section.
- Slope movement stops cleanly on flat ground and resumes on the next slope.

### WM-04: Slope movement with speed effects

**Steps**
1. Repeat WM-01 while affected by the Earth speed-reduction effect.
2. Repeat with a speed potion active, including the potion expiring while on a slope.

**Expected**
- The Hero continues to follow the slope at the modified speed.
- The rise/run remains consistent; speed changes should not cause false falling or alter the terrain angle.

## Landing / Grounding

### LG-01: Distinguish landing from side contact

**Steps**
1. Fall onto a Floor, a Breakable, and an Interactable such as ChestLarge.
2. Release movement input after landing on each object.
3. Repeat by moving into the side of each object without landing on top.

**Expected**
- Landing from above sets the Hero grounded and ends falling.
- The grounded animation can transition to idle when there is no movement input.
- Side contact alone does not count as a landing or prevent the Hero from moving away.

## Ceiling contact

### CC-01: Hit a ceiling while jumping

**Steps**
1. Jump straight up beneath a low ceiling.
2. Repeat while moving into the underside of a ledge.
3. Move away from the ceiling and jump again.
4. Jump beneath a low ceiling, wait until descending, then double jump and hit its underside while rising.
5. Walk or jump into the side of a wall without contacting its underside.

**Expected**
- Hitting the underside while rising triggers the ceiling-collision response.
- Hitting the underside during a double jump from descent triggers the ceiling-collision response before falling.
- Side contact alone does not trigger the ceiling response.
- The Hero can jump normally again after leaving the ceiling.

## Wall bumping

### WB-01: Jump into a tall wall from either facing direction

**Steps**
1. Jump into a tall wall while facing it.
2. Repeat while facing away from the wall and moving backward into it.
3. Hold movement toward the wall after contact, then try to jump before landing.

**Expected**
- The Hero bumps away from the wall and falls.
- Input toward the wall stays blocked until the Hero moves away.
- The Hero cannot jump again before landing, and holding into the wall does not cause repeated bumps.

### WB-02: Wall beneath a low ceiling

**Steps**
1. Jump into a wall with a lower ceiling extending over the space beyond it, forming an inverted-L shape.

**Expected**
- The Hero bumps away instead of stepping over or appearing on top of the wall beneath the ceiling.

### WB-03: Drive horizontal blocking from front/back contact

**Steps**
1. Walk into a Wall, a Breakable, and an Interactable from the front until the Hero makes contact.
2. Keep holding toward each object, then press away from it.
3. Repeat with the object on the Hero's opposite side.
4. Walk beside a Floor without contacting a Wall, Breakable, or Interactable.

**Expected**
- The Hero cannot move through a blocking Wall, Breakable, or Interactable.
- Movement away from a contacted object is not blocked.
- Contact with a Floor alone does not block horizontal movement.

### WB-04: Perform a long jump towards a far-away edge while speed enhanced

**Steps**
1. Consume a speed potion and stand near the edge of an elevated platform, leaving enough room for the jump to fall short of the distant platform.
2. Jump toward the distant platform while continuing to hold the direction of travel.
3. Repeat with the takeoff position adjusted slightly closer to and farther from the edge.

**Example setup**
- Use a takeoff point near world position (-50, 22) and a distant platform edge about 13 world units away.
- Start near, but not at, the elevated platform's edge so the Hero does not land safely on the distant platform.

**Expected**
- Hitting the cliff edge triggers a wall-bump recoil that clears the edge even while directional input is held.
- The bump moves the Hero farther while speed-enhanced than during a normal-speed wall bump.
- The Hero does not remain pinned against the edge; if the distant platform is missed, the Hero falls.

### WB-05: Step over low platforms and bump blocked edges

**Steps**
1. Jump toward a low platform with clear space on top.
2. Repeat against a taller or obstructed edge that cannot be stepped over.
3. Repeat with the front sensor reaching a solid Floor collider before its paired Wall collider, if the setup allows.

**Expected**
- A valid step-over moves the Hero onto the low platform without triggering a wall bump.
- An edge that is too high or has no clear landing space triggers a wall bump.
- A Floor contact processed before its paired Wall contact still takes the correct step-over or bump response.

## Results
Record the build, platform, status, and any reproduction notes when a test fails or behaves intermittently.