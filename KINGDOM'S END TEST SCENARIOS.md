# Kingdom's End: Test Scenarios

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

## Results
Record the build, platform, status, and any reproduction notes when a test fails or behaves intermittently.