# Safety Guidelines and EMI Management

**CNC 3-Axis Machine - Safety & Electromagnetic Interference**

---

## Table of Contents
1. [Safety Guidelines](#safety-guidelines)
2. [Electrical Safety](#electrical-safety)
3. [Mechanical Safety](#mechanical-safety)
4. [EMI Management](#emi-management)
5. [Emergency Procedures](#emergency-procedures)
6. [Maintenance Safety](#maintenance-safety)

---

## 1. Safety Guidelines

### General Rules
- **Never operate the machine alone** - Always have someone nearby who can help in an emergency
- **Wear appropriate PPE:**
  - Safety glasses (ANSI Z87.1 rated)
  - Hearing protection when spindle is running
  - Dust mask (N95 or better) for wood dust
  - No loose clothing, jewelry, or long hair near moving parts
- **Keep work area clean** - Remove debris, tools, and materials before starting
- **Never reach into the cutting area** while the machine is powered on
- **Dry run first** - Test toolpaths with spindle off and Z raised

### Before Each Use Checklist
- [ ] Emergency stop button tested and functional
- [ ] All guards and enclosures in place
- [ ] Work area is clear of obstructions
- [ ] Workpiece is securely clamped
- [ ] Tool is properly installed and tightened
- [ ] Spindle direction verified (clockwise for most operations)
- [ ] G-code file reviewed and previewed
- [ ] All personnel are clear of the machine

### During Operation
- **Monitor continuously** - Watch for unusual sounds, vibrations, or tool deflection
- **Stop immediately if:**
  - Unusual noise or vibration occurs
  - Smoke or burning smell detected
  - Tool breaks or becomes dull
  - Workpiece comes loose
  - Any error messages appear
- **Do not leave running unattended** - Even for "simple" jobs
- **Keep hands away** from the cutting area at all times

### After Operation
- [ ] Allow spindle to stop completely before opening enclosure
- [ ] Turn off spindle and wait for cooldown
- [ ] Remove workpiece only after all motion has stopped
- [ ] Clean up dust and chips
- [ ] Inspect tool for wear or damage
- [ ] Log any issues or maintenance needs

---

## 2. Electrical Safety

### Power Supply
- **Proper grounding:**
  - All metal parts connected to earth ground
  - Use 3-prong grounded outlets
  - Check ground continuity with multimeter (< 1Ω)
- **Fuse protection:**
  - Main power: 20A fuse/breaker
  - 48V stepper supply: 15A fuse
  - 24V auxiliary: 5A fuse
  - 5V logic: 5A fuse
- **GFCI protection** - Use Ground Fault Circuit Interrupter if water cooling or in damp area

### Emergency Stop (E-Stop)
- **Type:** Normally Closed (NC) mushroom button, red, 40mm
- **Location:** Easily accessible, visible, within arm's reach
- **Function:**
  - Cuts power to stepper drivers (hardware cutoff)
  - Disables spindle immediately (via contactor)
  - MCU enters alarm state
- **Testing:** Press E-stop before each use to verify it works
- **Reset procedure:**
  - Rotate or pull E-stop to unlock
  - Power cycle or send reset command
  - Re-home machine before resuming

### Wiring Safety
- **Cable ratings:**
  - Stepper motors: 22-18 AWG, rated for motor current
  - Spindle: 18-16 AWG, 3-phase shielded
  - AC mains: 14-12 AWG, UL listed
- **Cable management:**
  - Use drag chains for moving cables
  - Secure fixed cables with cable ties (not too tight)
  - Label all cables (X motor, Y limit, spindle, etc.)
- **Avoid:**
  - Pinch points where cables can be crushed
  - Sharp edges that can cut insulation
  - Heat sources (spindle, VFD, heatsinks)

### VFD (Variable Frequency Drive)
- **Installation:**
  - Mount in well-ventilated location
  - Keep away from control electronics (>30cm separation)
  - Ensure heatsink fins are vertical (convection cooling)
- **EMI filter:**
  - Install on AC input side
  - Ground filter case to VFD ground
- **Output cable:**
  - Use shielded 3-phase cable (VFD to spindle)
  - Ground shield at VFD end only (avoid ground loops)
  - Keep as short as possible (< 3m ideal)

---

## 3. Mechanical Safety

### Guards and Enclosures
- **Purpose:**
  - Contain flying chips and dust
  - Prevent accidental contact with moving parts
  - Reduce noise
- **Requirements:**
  - Transparent (acrylic, polycarbonate) for visibility
  - Interlock switch on access doors (optional but recommended)
  - Emergency stop button accessible without opening

### Spindle Safety
- **Installation:**
  - Verify spindle is securely clamped (torque to spec)
  - Check runout (< 0.02mm TIR)
  - Ensure collet is clean and properly seated
- **Operation:**
  - Allow spindle to reach full speed before cutting
  - Avoid sudden starts/stops (use ramp up/down in VFD)
  - Monitor bearing noise (replace if grinding heard)
- **Tool change:**
  - **Power off spindle** and wait for complete stop
  - Use proper wrenches (not pliers)
  - Clean collet and tool shanks before installing
  - Tighten collet nut to spec (hand-tight + 1/4 turn)

### Moving Parts
- **Axes:**
  - Keep fingers away from lead screws and belts
  - Never touch moving rails or carriages
  - Ensure proper lubrication (grease on ballscrews, oil on rails)
- **Backlash:**
  - Check for play in couplers and nuts
  - Adjust anti-backlash nuts if excessive
  - Monitor for wear over time

### Dust Collection
- **Hazard:** Wood dust is combustible and can cause respiratory issues
- **Solution:**
  - Shop vacuum or dust collector (minimum 300 CFM)
  - Dust shoe attached near spindle
  - Run continuously during cutting
  - Empty collection bin regularly (avoid overfill)
- **Fire risk:**
  - Wood dust is flammable - keep away from hot surfaces
  - No smoking near machine
  - Have fire extinguisher nearby (Class A or ABC)

---

## 4. EMI Management

### Sources of EMI
1. **VFD / Spindle:** High-frequency switching (1-20 kHz)
2. **Stepper drivers:** PWM chopping (20-40 kHz)
3. **Switching power supplies:** High-frequency ripple
4. **Relays / Contactors:** Inductive kick when switching

### Symptoms of EMI Problems
- Limit switches triggering randomly
- Raspberry Pi or MCU resets
- Serial communication errors (missed characters)
- Stepper drivers faulting
- LCD screen flickering

### Mitigation Strategies

#### 1. Cable Routing (Most Important)
- **Separate power and signal cables:**
  - **High power (spindle, VFD, AC mains):** Route on one side of frame
  - **Signal (step/dir, limits, probe):** Route on opposite side
  - **Minimum separation:** 15-20cm (6-8 inches)
- **Avoid parallel runs:**
  - If cables must cross, do so at 90° angles
  - Use cable trays or conduit to keep separation
- **Shorten cables:**
  - Keep signal cables as short as possible
  - Coil excess cable (use ferrite on coil if needed)

#### 2. Shielding
- **Spindle cable:**
  - Use braided shield or foil + drain wire
  - Ground shield at VFD end **only** (star ground)
  - Do **not** ground both ends (causes ground loop)
- **Stepper cables:**
  - Twisted pair reduces EMI
  - Shield recommended but not always needed
  - If shielded, ground at driver end
- **Signal cables (limits, probe):**
  - Twisted pair (each signal + ground)
  - Shield preferred for long runs (> 1m)
  - Ground shield at MCU end

#### 3. Grounding
- **Star ground topology:**
  ```
  Earth Ground (single point)
       ├── Machine frame
       ├── VFD case
       ├── Power supply case
       ├── Control enclosure
       └── Spindle motor case
  ```
- **Avoid ground loops:**
  - Do not create multiple ground paths
  - Use single-point grounding for shields
  - Check with multimeter: voltage between grounds should be < 0.1V
- **Ground wire size:**
  - Main earth ground: 12 AWG or larger
  - Internal grounds: 18-16 AWG

#### 4. Filtering
- **VFD input:**
  - AC EMI filter (e.g., Schaffner FN2070)
  - Reduces noise back into mains
- **VFD output:**
  - Ferrite clamp on spindle cable (near VFD)
  - Snap-on ferrite cores (FT-240-43 or similar)
  - Wrap cable 3-5 turns through ferrite
- **Stepper drivers:**
  - Ceramic capacitors (0.1µF) across STEP/DIR pins
  - RC snubber (100Ω + 0.1µF) if still noisy
- **Power supplies:**
  - Add bulk capacitors (1000-4700µF) at output
  - Common-mode choke on DC output (optional)

#### 5. Software Debouncing
- **Limit switches:**
  - Firmware debounce: 10-50ms
  - Ignore transient spikes
- **Probe input:**
  - Debounce + averaging
  - Reject single-sample glitches

### Testing for EMI
1. **Baseline test:**
   - Run machine with spindle **off**
   - Execute complex G-code (circles, arcs)
   - Should complete without errors
2. **Spindle EMI test:**
   - Run spindle at various RPMs (no cutting)
   - Monitor for limit switch false triggers
   - Check serial communication (no errors)
3. **Cutting test:**
   - Cut actual material
   - Monitor for issues under load
   - VFD current draw may increase EMI

### EMI Troubleshooting Flowchart
```
┌─────────────────────────────────┐
│  EMI Issue Detected             │
└───────────┬─────────────────────┘
            │
    ┌───────▼──────────┐
    │ Which symptom?   │
    └───┬──────────┬───┘
        │          │
        │          └──> Random resets → Check power supply ripple, add bulk caps
        │
        └──> False triggers → Limit switches
                  │
                  ├─> Use shielded twisted pair
                  ├─> Increase debounce time
                  ├─> Move cable away from spindle
                  └─> Add RC filter at MCU input
```

---

## 5. Emergency Procedures

### Emergency Stop Activated
1. **Do not panic** - Machine is safe, all motion halted
2. Assess situation:
   - Is there fire or smoke? → Evacuate, call emergency services
   - Is someone injured? → Provide first aid, call for help
   - Is it a false trigger? → Investigate cause
3. Power cycle if needed:
   - Turn off main power switch
   - Wait 10 seconds
   - Turn back on
4. Reset E-stop (rotate/pull to unlock)
5. **Re-home machine** before resuming (positions may be lost)

### Tool Break
1. **Press E-stop immediately**
2. Turn off spindle
3. Wait for spindle to stop completely
4. Remove broken tool carefully (may be hot)
5. Inspect spindle collet for damage
6. Replace with new tool
7. Re-zero Z height
8. Resume job (may need to restart from beginning)

### Fire
1. **Press E-stop**
2. Turn off main power
3. Use fire extinguisher (Class A or ABC)
   - **P.A.S.S.:** Pull pin, Aim low, Squeeze, Sweep
4. If fire is large or spreading:
   - Evacuate immediately
   - Call emergency services (911, 112, etc.)
5. Do not re-enter until safe

### Electrical Shock
1. **Do not touch the victim** if still in contact with electricity
2. Turn off main power at breaker
3. If safe, remove victim from electrical source (use non-conductive object)
4. Call emergency services immediately
5. If victim is not breathing, begin CPR if trained

### Workpiece Came Loose
1. **Press E-stop immediately**
2. Turn off spindle
3. Wait for spindle to stop
4. Carefully remove loose workpiece (may be spinning or hot)
5. Inspect for damage to tool, spindle, or machine
6. Re-secure workpiece with proper clamping
7. Restart job from safe point

---

## 6. Maintenance Safety

### Lockout/Tagout (LOTO)
- **Before maintenance:**
  - Turn off main power switch
  - Unplug machine from wall outlet
  - Attach "DO NOT OPERATE" tag
  - Verify no voltage with multimeter
- **Who can remove lock?** Only the person who placed it

### Inspection Schedule

#### Daily (If Used)
- [ ] Check for loose fasteners (vibrations can loosen screws)
- [ ] Inspect cables for damage (cuts, abrasion)
- [ ] Clean chips and dust from machine
- [ ] Verify E-stop functions

#### Weekly
- [ ] Lubricate ballscrews (grease) or rails (oil)
- [ ] Check belt tension (if belt-driven)
- [ ] Inspect tool holder for runout
- [ ] Clean spindle collet and ER nut

#### Monthly
- [ ] Check all electrical connections for tightness
- [ ] Inspect limit switches for wear
- [ ] Test homing accuracy (should repeat within 0.01mm)
- [ ] Check stepper driver heatsinks (should not be too hot to touch)
- [ ] Inspect VFD fan (clean dust from intake)

#### Annually
- [ ] Deep clean entire machine (disassemble if needed)
- [ ] Re-grease ballscrew nuts
- [ ] Check linear rail preload (adjust if needed)
- [ ] Inspect motor couplers for wear
- [ ] Test ground continuity (< 1Ω to earth)
- [ ] Replace VFD fan if noisy

### Safe Maintenance Practices
- **Electrical work:**
  - Always disconnect power first
  - Discharge capacitors (wait 5 minutes after power off)
  - Use insulated tools
  - Work with one hand when possible (other hand in pocket)
- **Mechanical work:**
  - Support heavy parts (gantry, spindle) before removing fasteners
  - Use proper tools (not adjustable wrenches on precision nuts)
  - Clean parts before reassembly
  - Apply Loctite (blue 243) on critical fasteners

---

## Safety Resources

### Training
- **Required knowledge:**
  - Basic CNC operation
  - G-code fundamentals
  - Emergency procedures
- **Recommended training:**
  - First aid and CPR
  - Fire extinguisher use
  - Electrical safety basics

### Contact Information
- **Emergency services:** [Your local number]
- **Electrical contractor:** [Your contact]
- **Machine technician:** [Your contact]
- **Nearest hospital:** [Address and phone]

### Signs and Labels
- Place warning signs:
  - "DANGER: Moving parts"
  - "DANGER: High voltage"
  - "NOTICE: Eye protection required"
  - "NOTICE: Hearing protection required"
- Label all power switches and emergency stops
- Mark direction of spindle rotation

---

## References
- OSHA 1910 Subpart O (Machinery and Machine Guarding)
- NFPA 79 (Electrical Standard for Industrial Machinery)
- ANSI B11.0 (Safety of Machinery)
- IEC 60204-1 (Safety of machinery - Electrical equipment)

---

**Document Version:** 1.0  
**Last Updated:** 2024  
**Review Frequency:** Annually or after any incident

**REMEMBER: Safety is everyone's responsibility. If something doesn't feel safe, STOP and ask for help.**
