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

## Quick Start

**New to CNC or want to get started quickly?**

👉 **[Quick Start Guide (Vietnamese)](docs/QUICKSTART_VN.md)** - Step-by-step guide from parts to first cut

**For detailed information:**
- Read the [Full Specification (Vietnamese)](docs/SPEC_VN.md) for comprehensive design details
- Follow the [Development Roadmap](docs/ROADMAP.md) for phased implementation
- Review [Safety Guidelines](docs/SAFETY_EMI.md) before starting

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

### Host Software (Raspberry Pi)
**Recommended: CNCjs**
- Web-based interface
- G-code streaming and visualization
- Job queue management
- Macros and widgets
- Installation: `sudo npm install -g cncjs`
- Access: `http://raspberrypi.local:8000`

**Alternative Options:**
- Custom Flask/FastAPI (Python) web app
- Custom Node.js/Express server
- bCNC (Python desktop app)
- Universal Gcode Sender (Java)

### Firmware (MCU)
**Option 1: grblHAL on STM32F407 (Recommended)**
- Low jitter, high precision
- Excellent real-time performance
- Well-documented, stable
- Repository: https://github.com/grblHAL/STM32F4xx
- Build: STM32CubeIDE or arm-none-eabi-gcc
- Flash: ST-Link programmer

**Option 2: FluidNC on ESP32**
- Built-in Wi-Fi and web interface
- YAML configuration
- Good for prototyping and home use
- Repository: https://github.com/bdring/FluidNC
- Build: PlatformIO or Arduino IDE
- Flash: USB cable

**Option 3: Custom Firmware**
- Full control over features
- Based on grblHAL or g2core
- Higher development effort

### CAM Software
- **Fusion 360** - Free for hobbyists, full CAD/CAM
- **FreeCAD Path** - Open-source, 2.5D-3D
- **VCarve** - Excellent for signage and engraving
- **Estlcam** - Affordable (~$60), easy to use

See [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) for detailed build instructions.

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

## Documentation

This project includes comprehensive documentation:

### Core Documentation
- **[docs/SPEC_VN.md](docs/SPEC_VN.md)** - Complete Vietnamese specification (15 sections)
  - Goals and scope
  - Mechanical structure
  - Electrical system
  - Software architecture
  - Operating parameters
  - Formulas and calculations
  - Safety and EMI management
  - Development roadmap
  - Bill of materials
  - STM32 vs ESP32 comparison
  - And more...

- **[docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)** - System architecture
  - Pi + MCU distributed control
  - Communication protocols
  - Firmware options
  - Build and flash instructions
  - Configuration examples

- **[ENV_SETUP.md](ENV_SETUP.md)** - Environment setup guide
  - Toolchain installation
  - Raspberry Pi setup
  - STM32 firmware flashing
  - ESP32 firmware flashing
  - Cross-compilation
  - Troubleshooting

### Hardware Documentation
- **[docs/hardware/BOM.md](docs/hardware/BOM.md)** - Complete Bill of Materials
  - All parts with specifications
  - Price estimates
  - Supplier recommendations
  - Tool requirements

### Safety & Operations
- **[docs/SAFETY_EMI.md](docs/SAFETY_EMI.md)** - Safety guidelines and EMI management
  - General safety rules
  - Electrical safety
  - Mechanical safety
  - EMI mitigation strategies
  - Emergency procedures
  - Maintenance safety

### Project Planning
- **[docs/ROADMAP.md](docs/ROADMAP.md)** - Development roadmap
  - Phase 1: MVP 3-axis (2-3 months)
  - Phase 2: Optimization (1-2 months)
  - Phase 3: 4-axis expansion (2-3 months)
  - Phase 4: 5-axis capability (3-6 months)
  - Phase 5: Production ready (ongoing)
  - Risk management
  - Success metrics

### Additional Resources
- **README.md** - This file (project overview)
- **src/** - Application source code
- **firmware/** - MCU firmware (planned)
- **raspberry_pi/** - Host software (planned)

## Roadmap
- V1: 2040 frame, GRBL + TMC2209, 500–800 W spindle, homing + hard limits
- V2: Ballscrews + MGN rails, tool length sensor, aluminum T‑slot bed
- V3: 1.5 kW VFD with PWM/Modbus RPM control, auto-squaring (dual Y)
- Software: ESP32 web UI, advanced probing macros, auto surfacing

## Contributing

We welcome contributions! Here's how you can help:

### Reporting Issues
- Use GitHub Issues for bug reports
- Include: Machine specs, firmware version, G-code that caused issue
- Attach photos/videos if relevant

### Code Contributions
- Fork the repository
- Create feature branch: `git checkout -b feature/amazing-feature`
- Follow existing code style
- Test your changes thoroughly
- Submit pull request with clear description

### Documentation
- Fix typos or clarify instructions
- Add translations (English, Vietnamese, etc.)
- Share your build photos and experiences

### Hardware Improvements
- Share CAD designs for upgrades
- Document modifications
- Post to Discussions for community feedback

### Community
- Help others in Issues and Discussions
- Share your projects and G-code
- Write tutorials or blog posts

**Please read [docs/ROADMAP.md](docs/ROADMAP.md) to see current development priorities.**

## License
- Source and documentation under MIT license (unless noted otherwise in specific folders).

---

Contact – Q&A
- Use Issues/Discussions
- Share build photos/videos for community feedback and optimization
