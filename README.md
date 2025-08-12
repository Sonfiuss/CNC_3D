# 3-Axis CNC – Open-source Milling/Engraving Machine

An open-source 3-axis CNC (X/Y/Z) for woodworking, plastics, acrylic, light aluminum, and PCB engraving. The design focuses on affordability, easy assembly, safety, and extensibility. It uses common hardware (NEMA steppers, aluminum extrusion frame) and widely adopted firmware (GRBL/GRBL-ESP32/LinuxCNC) so you can get up and running quickly.

## Table of Contents
- Overview
- Key Features
- Reference Specifications
- System Architecture
- Hardware
- Software and Firmware
- Assembly and Wiring
- Setup – Calibration – Operation
- Safety
- Project Structure
- Roadmap
- Contributing
- License

---

## Overview
- Goal: Build a 3-axis CNC for 2D–2.5D cutting/engraving with stable accuracy for small shops and makers.
- Audience: Makers, students, labs, small sign shops, mechanical prototyping.
- Philosophy: Open, modular, and flexible — multiple configurations to match budget and needs.

## Key Features
- Independent X/Y/Z motion with homing and limit switches.
- Standard G-code support (RS-274), compatible with popular CAM tools (Fusion 360, VCarve, FreeCAD, KiCad PCB2GCode, etc.).
- GRBL/GRBL-ESP32/LinuxCNC controller options; USB/UART/Wi‑Fi connectivity (optional).
- Smooth motion with accurate curve interpolation; acceleration/jerk handling.
- Probing (Z-probe), E‑Stop, Feed Hold, Reset.
- Expandable: drag chains, dust collection, spindle sensing, optical/mechanical endstops, tool length sensor.

## Reference Specifications
- Work area (example 6040 class):
  - X: ~600 mm, Y: ~400 mm, Z: 80–120 mm (config-dependent)
- Drive:
  - Y/X: lead screw T8 or ballscrew SFU1204/1605; MGN12/15 linear rails or V-wheels
  - Z: T8/ballscrew; MGN12 rails
- Stepper motors:
  - NEMA 17 (light-duty) or NEMA 23 (medium-strong); 1.0–2.8 A
- Drivers:
  - TMC2209 (quiet, high microstepping) or DRV8825/TB6600 (higher torque)
- Spindle:
  - 500 W – 1.5 kW, ER11 collet, on/off and PWM control (VFD optional)
- Power:
  - 24 VDC 10–20 A (depends on motors and spindle), auxiliary 5 VDC if required
- Accuracy:
  - Repeatability ±0.05–0.1 mm (depends on mechanics and calibration)
- Speed:
  - Rapids 1000–3000 mm/min (safe baseline); cutting per material/tooling

Note: Values vary with frame, screw type, drivers, and your setup.

## System Architecture
- Mechanics: 2040/2060/2080 aluminum extrusions, T‑slot/MDF spoilboard, lead screw/timing belt, linear rails.
- Control and I/O:
  - MCU: Arduino Uno/Nano (GRBL) or ESP32 (grbl_ESP32) or PC + LinuxCNC
  - Stepper drivers: TMC2209/DRV8825/TB6600
  - Sensors: Limit/Homing (X/Y/Z), Z‑probe, E‑Stop
  - Spindle: Relay/PWM/VFD control
- Software:
  - G-code sender: Universal Gcode Sender (UGS), CNCjs, bCNC
  - CAM: Fusion 360, FreeCAD Path, Vectric, FlatCAM, Inkscape plugins

Block diagram (illustrative):
```
[PC/CAM] -> [G-code Sender] -> [Controller (GRBL/ESP32/LinuxCNC)]
                                    |-> Step/Dir -> [Drivers] -> [Steppers X/Y/Z]
                                    |-> In/Out   -> [Limit/Probe/E-Stop]
                                    |-> PWM/Relay-> [Spindle/VFD]
```

## Hardware
- Frame:
  - 2040/2060 aluminum extrusions, L brackets, motor plates, drag chains, vibration-damping feet
- Motion:
  - T8/SFU ballscrews + anti-backlash nuts, couplers, or HTD belts/pulleys (if belt-driven)
- Guides:
  - MGN12/15 rails + blocks, or V-wheels on V‑slot
- Motors and Drivers:
  - NEMA17/23; TMC2209 (quiet) or TB6600 for larger NEMA23 torque
- Electrical and Protection:
  - 24V/5V PSU, fuses, hard E‑Stop, Feed Hold/Resume buttons, chassis grounding
- Spindle and Tooling:
  - 500 W – 1.5 kW spindle, ER11 collets, tool clamps, dust shoe, workholding

## Software and Firmware
- Option 1: GRBL (Arduino Uno)
  - Easy to deploy, large community, stable
- Option 2: grbl_ESP32
  - Wi‑Fi connectivity, higher performance, more I/O
- Option 3: LinuxCNC (PC/SBC)
  - Advanced: kinematics, HAL, hard real-time control

Recommended G‑code senders: UGS, CNCjs, bCNC. CAM: Fusion 360, FreeCAD Path, Vectric, FlatCAM (for PCBs).

## Assembly and Wiring
- Mechanics:
  - Assemble the frame, square axes, install rails and screws, adjust preload
  - Mount a rigid Z-axis; verify table–X/Y parallelism
- Electrical:
  - Route stepper wiring away from spindle power to reduce EMI
  - Use shielded cables where possible; ground the frame; manage cables with drag chains
  - Prefer Normally Closed (NC) limit switches for fail-safe behavior
- Checks:
  - Smooth motion without binding; rigid spindle mount; clean collets

## Setup – Calibration – Operation
1) Flash firmware:
   - GRBL: flash via Arduino IDE/PlatformIO
   - ESP32: build and flash grbl_ESP32 per project docs
2) Basic GRBL configuration (via G‑code sender):
   - $0=10 (step pulse, µs — per driver)
   - $1=255 (hold motor)
   - $3=0/7 (invert axis direction if needed)
   - $5/$6 (invert limit/probe as required)
   - $20=0 / $21=1 (soft/hard limits per use case)
   - $22=1 (enable homing)
   - $10=3 (status report)
   - $100/$101/$102 (steps/mm — see below)
   - $110/$111/$112 (max rate, mm/min)
   - $120/$121/$122 (acceleration, mm/s^2)
   - $130/$131/$132 (max travel, mm)
3) Steps/mm (leadscrew):
   - steps_per_mm = (steps_per_rev × microsteps) / lead_mm
   - Example: 200 steps/rev × 16 microsteps / 8 mm = 400 steps/mm
4) Homing:
   - Choose home at min or max, run $H; verify Z→X→Y sequence
5) Practical calibration:
   - Command 100 mm moves and measure actual travel; tune $100/$101/$102
   - Verify X–Y squareness and tram; surface the spoilboard if needed
6) Operation:
   - Set feed, RPM, and depth of cut per material and tooling
   - Use a Z‑probe to establish Z0 for engraving/milling
   - Secure workholding; enable dust collection if available

Test G‑code sample:
```
G21 G90
G0 X0 Y0 Z5
G1 Z-1 F100
G1 X50 Y0 F600
G1 X50 Y50
G1 X0  Y50
G1 X0  Y0
G0 Z10
```

## Safety
- Always have an accessible E‑Stop; wear eye protection; keep safe clearance.
- Never reach into the cutting area while operating; keep cable management tidy.
- Ground the frame; manage combustible dust; mitigate noise.
- Dry-run (air-cut) before real cuts to verify toolpaths.

## Project Structure (suggested)
```
.
├─ firmware/         # GRBL/grbl_ESP32 configs, flashing scripts
├─ hardware/
│  ├─ cad/           # 3D, DXF, STEP for frame/plates
│  ├─ bom/           # Bill of Materials
│  └─ wiring/        # Wiring diagrams, pinouts
├─ cnc/
│  ├─ post/          # Custom post-processors (if any)
│  └─ samples/       # Sample G-code, test patterns
├─ docs/
│  ├─ setup/         # Assembly and calibration guides
│  └─ safety/        # Safety procedures
└─ README.md
```

## Roadmap
- V1: 2040 frame, GRBL + TMC2209, 500–800 W spindle, homing + hard limits
- V2: Ballscrews + MGN rails, tool length sensor, aluminum T‑slot bed
- V3: 1.5 kW VFD with PWM/Modbus RPM control, auto-squaring (dual Y)
- Software: ESP32 web UI, advanced probing macros, auto surfacing

## Contributing
- Issues/PRs welcome: bugs, docs, tuning, mechanical/electrical design.
- Guidelines: describe changes clearly; include images/diagrams when helpful; follow style/format conventions.

## License
- Source and documentation under MIT license (unless noted otherwise in specific folders).

---

Contact – Q&A
- Use Issues/Discussions
- Share build photos/videos for community feedback and optimization
