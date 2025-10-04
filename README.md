# 3-Axis CNC Machine – Open-source Wood Milling/Engraving System

**Máy CNC 3 trục gia công gỗ mở rộng 4-5 trục**

An open-source 3-axis CNC (X/Y/Z, expandable to 4-5 axes) for woodworking applications. The design features a **distributed control architecture** with Raspberry Pi (high-level control, UI) + MCU (STM32/ESP32 for real-time step generation). Working area: 500mm × 500mm, using NEMA 23 steppers and 1-1.5kW spindle.

**Key Features:**
- **500×500mm working area** (wood cutting, engraving, signage)
- **Raspberry Pi + MCU architecture** (Pi for UI/G-code streaming, MCU for real-time control)
- **Expandable:** 3-axis (XYZ) → 4-axis (+ rotary A) → 5-axis (+ tilt B/C)
- **Industrial-grade firmware:** grblHAL (STM32) or FluidNC (ESP32)
- **Professional features:** Auto-homing, Z-probe, EMI management, safety interlocks

## Table of Contents
- [Overview](#overview)
- [Key Features](#key-features)
- [System Architecture](#system-architecture)
- [Reference Specifications](#reference-specifications)
- [Hardware](#hardware)
- [Software and Firmware](#software-and-firmware)
- [Assembly and Wiring](#assembly-and-wiring)
- [Setup – Calibration – Operation](#setup--calibration--operation)
- [Safety](#safety)
- [Documentation](#documentation)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)

---

## Overview

### Project Goal
Build a professional-grade 3-axis CNC machine for wood machining (cutting, 2.5D carving, signage, small parts) with the capability to expand to 4-5 axes for rotary and indexed/simultaneous 5-axis operations.

### Target Applications
- **Woodworking:** MDF, plywood, hardwood, softwood
- **Operations:** Cutting, engraving, pocketing, profiling, V-carving
- **Precision:** ±0.05-0.1mm repeatability

### Architecture Philosophy
**Distributed Control System:**
- **Raspberry Pi 4 (Host):** High-level control, Web UI, G-code preview, job management, network access
- **STM32/ESP32 (Real-time):** G-code parser, motion planner, step pulse generation (low jitter)
- **Communication:** USB Serial / UART / SPI / CAN Bus

This architecture provides:
- **Flexibility:** Easy UI development on Linux (Pi)
- **Real-time performance:** Dedicated MCU for time-critical tasks
- **Scalability:** Add more MCUs for additional axes or subsystems

## Key Features

### Mechanical
- **Working area:** ~500mm × 500mm × 80-120mm (X × Y × Z)
- **Drive system:** Ballscrew (SFU1605) or belt (GT2/GT3)
- **Linear guides:** MGN12/MGN15 rails with preload adjustment
- **Frame:** 2040/2060 aluminum extrusions with cross-bracing

### Motion Control
- **Motors:** NEMA 23 steppers (2.2-3.0 N·m holding torque)
- **Drivers:** TB6600/DM542 (basic) or TMC5160 (advanced, quiet)
- **Resolution:** 1/8 to 1/16 microstepping (optimal for wood)
- **Speeds:** 1,000-3,000 mm/min (feed), 3,000-5,000 mm/min (rapids)

### Spindle
- **Power:** 1-1.5 kW (air or water cooled)
- **Speed:** 8,000-24,000 RPM (VFD controlled)
- **Collet:** ER11 (up to 7mm) or ER16 (up to 10mm)
- **Control:** PWM (0-10V) or Modbus RS485

### Control System
- **Host:** Raspberry Pi 4 (4GB RAM)
  - Web UI (CNCjs or custom)
  - G-code preview and streaming
  - Job queue and history
- **Real-time MCU:** STM32F407 (recommended) or ESP32
  - grblHAL or FluidNC firmware
  - Step pulse generation (up to 100kHz per axis)
  - Safety monitoring (limits, E-stop, watchdog)

### Expansion Capabilities
- **4-axis:** Add rotary A-axis for turning/indexing
- **5-axis:** Add tilt B or rotary C for complex geometries
- **ATC:** Automatic tool changer (future)
- **Probing:** 3D touch probe for part alignment

## Reference Specifications

### Working Envelope
- **X-axis travel:** 500mm (effective)
- **Y-axis travel:** 500mm (effective)
- **Z-axis travel:** 80-120mm (configurable)
- **A-axis (rotary, optional):** 360° continuous

### Performance
- **Feed rate (wood):** 1,000-3,000 mm/min
- **Rapids:** 3,000-5,000 mm/min
- **Acceleration:** 500-800 mm/s² (tunable)
- **Repeatability:** ±0.05-0.1 mm
- **Step rate:** Up to 100 kHz per axis (MCU dependent)

### Power Requirements
- **Stepper motors:** 48VDC @ 10-15A (Mean Well or similar)
- **Auxiliary:** 24VDC @ 5A (relays, fans, LED)
- **Logic:** 5VDC @ 5A (Raspberry Pi, MCU)
- **Spindle:** 220VAC @ 10A (VFD powered)

## System Architecture

### Hardware Block Diagram
```
┌──────────────────┐
│  Raspberry Pi 4  │  (Host: UI, G-code streaming, job management)
│   + CNCjs/Web    │
└────────┬─────────┘
         │ USB Serial / UART / SPI
┌────────▼─────────┐
│  STM32 / ESP32   │  (Real-time: parser, planner, step generation)
│  + grblHAL       │
└────────┬─────────┘
         │ STEP/DIR signals
┌────────▼─────────┐
│ Stepper Drivers  │  (TB6600, DM542, or TMC5160)
└────────┬─────────┘
         │
┌────────▼─────────┐
│  NEMA 23 Motors  │  (X, Y, Z, optional A)
└──────────────────┘

Additional I/O:
- VFD/Spindle (PWM or Modbus)
- Limit switches (NC, debounced)
- Z-probe (touchplate)
- E-Stop (hard cutoff)
```

### Software Architecture
- **Application Layer (Pi):** Node.js/Python web app
- **Firmware Layer (MCU):** grblHAL (C) or FluidNC (C++)
- **Communication:** ASCII G-code (GRBL protocol) or binary (custom)

For detailed architecture, see [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md).

For Vietnamese specification (full 15 sections), see [docs/SPEC_VN.md](docs/SPEC_VN.md).

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
