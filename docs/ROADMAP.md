# Development Roadmap

**CNC 3-Axis Wood Machine - Development Phases**

This roadmap outlines the development phases from initial prototype to production-ready machine.

---

## Overview

```
Phase 1 (MVP)    Phase 2 (Opt)    Phase 3 (4-axis)  Phase 4 (5-axis)  Phase 5 (Prod)
    3 months        2 months          3 months          6 months         Ongoing
       │                │                 │                 │                │
       ▼                ▼                 ▼                 ▼                ▼
   Basic CNC      Calibrated       Rotary axis      Tilt mechanism    Production
   XYZ working   High accuracy    Indexing mode     Simultaneous      features
```

---

## Phase 1: MVP (Minimum Viable Product) - 3 Axis
**Duration:** 2-3 months  
**Goal:** Get a functional 3-axis CNC cutting wood

### 1.1 Mechanical Assembly (4-6 weeks)
- [ ] **Week 1-2: Frame construction**
  - Cut aluminum extrusions to length
  - Assemble frame with L-brackets and T-nuts
  - Add cross-bracing for rigidity
  - Mount on anti-vibration feet
- [ ] **Week 3-4: Motion system**
  - Install linear rails (MGN12/15)
  - Mount ballscrews or belts
  - Align X and Y axes (check parallelism with dial indicator)
  - Assemble gantry and Z-axis
  - Install couplers and bearing blocks
- [ ] **Week 5: Squaring and alignment**
  - Check frame squareness (diagonals should be equal)
  - Verify rails are parallel (< 0.05mm over 500mm)
  - Adjust preload on linear rail blocks
  - Test smooth motion by hand (no binding)
- [ ] **Week 6: Spindle and workholding**
  - Mount spindle to Z-axis
  - Install spoilboard
  - Add T-track or clamps

### 1.2 Electrical Wiring (2-3 weeks)
- [ ] **Week 1: Power distribution**
  - Install power supplies (48V, 24V, 5V)
  - Wire main power switch and fuses
  - Connect E-stop in series with power (NC)
  - Test voltages with multimeter
- [ ] **Week 2: Motor and driver wiring**
  - Connect NEMA 23 motors to drivers (X, Y, Z)
  - Wire STEP/DIR/ENABLE signals from MCU to drivers
  - Configure microstepping (1/8 or 1/16)
  - Test each axis individually (jog 10mm, measure actual)
- [ ] **Week 3: Sensors and I/O**
  - Install limit switches (NC type)
  - Wire Z-probe input
  - Connect spindle control (PWM or relay)
  - Test all inputs with multimeter/firmware

### 1.3 Firmware Setup (1-2 weeks)
- [ ] **Option A: STM32 + grblHAL**
  - Clone grblHAL repository
  - Configure pin definitions in `my_machine.h`
  - Build and flash firmware
  - Test serial communication (115200 baud)
- [ ] **Option B: ESP32 + FluidNC**
  - Clone FluidNC repository
  - Create `config.yaml` for your machine
  - Build and flash firmware
  - Access WebUI to verify

### 1.4 Software Setup (1 week)
- [ ] **Raspberry Pi host**
  - Install Raspberry Pi OS
  - Install CNCjs: `sudo npm install -g cncjs`
  - Auto-start on boot (systemd service)
  - Access via browser: `http://raspberrypi.local:8000`
- [ ] **Connect Pi to MCU**
  - USB cable: `/dev/ttyUSB0` or `/dev/ttyACM0`
  - Test connection in CNCjs
  - Send `$$` to see GRBL settings

### 1.5 Initial Configuration (1 week)
- [ ] **Calculate steps/mm**
  ```
  For ballscrew (5mm pitch): (200 steps × 16 microsteps) / 5 = 640 steps/mm
  For belt (GT2): (200 × 16) / (20 teeth × 2mm) = 80 steps/mm
  ```
  - Set $100 (X), $101 (Y), $102 (Z)
- [ ] **Set max rates**
  - Start conservative: 1000 mm/min
  - Set $110, $111, $112
- [ ] **Set acceleration**
  - Start: 300 mm/s²
  - Set $120, $121, $122
- [ ] **Set travel limits**
  - Measure actual travel
  - Set $130, $131, $132

### 1.6 First Tests (1 week)
- [ ] **Axis movement test**
  - Jog each axis (no spindle)
  - Verify direction (invert if wrong: $3)
  - Check for smooth motion, no missed steps
- [ ] **Homing test**
  - Configure homing ($22=1)
  - Set homing direction ($23)
  - Test $H command
  - Should repeat within 0.01mm
- [ ] **G-code test (air cut)**
  - Load simple G-code (square, circle)
  - Run with spindle OFF and Z raised
  - Verify toolpath is correct
- [ ] **First cut!**
  - Secure MDF piece
  - Install tool (6mm end mill)
  - Set Z zero with probe or paper method
  - Run simple pocket or profile
  - Celebrate! 🎉

### Phase 1 Deliverables
- ✅ Functional 3-axis CNC
- ✅ Basic homing and limit switches
- ✅ Web UI for control
- ✅ Able to cut simple 2D/2.5D shapes in MDF

---

## Phase 2: Optimization & Calibration
**Duration:** 1-2 months  
**Goal:** Improve accuracy and repeatability

### 2.1 Calibration (2-3 weeks)
- [ ] **Steps/mm accuracy**
  - Command 100mm move, measure actual with calipers
  - Adjust $100/$101/$102 if needed
  - Repeat until error < 0.1mm over 100mm
- [ ] **Backlash measurement**
  - Use dial indicator on each axis
  - Measure play when reversing direction
  - Adjust anti-backlash nut if > 0.05mm
- [ ] **Squareness**
  - Cut large square (400×400mm)
  - Measure diagonals (should be equal within 0.2mm)
  - Adjust frame if needed
- [ ] **Tram Z-axis**
  - Use dial indicator on spindle
  - Check perpendicularity to table (< 0.05mm over 200mm)
  - Shim if needed

### 2.2 Motion Tuning (1-2 weeks)
- [ ] **Acceleration tuning**
  - Increase $120/$121/$122 gradually
  - Test with rapid moves
  - Stop when you hear skipping or feel vibration
  - Set 10-20% below that limit
- [ ] **Junction deviation**
  - Affects cornering speed
  - Typical: 0.01-0.05mm
  - Test with rounded corners
- [ ] **Jerk (if supported)**
  - Controls abruptness of direction changes
  - Lower = smoother, but slower
  - Tune by feel and surface finish

### 2.3 Features & Automation (2-3 weeks)
- [ ] **Z-probe auto-zero**
  - Create macro in CNCjs
  - Use touchplate (known thickness)
  - Store as button: "Probe Z"
- [ ] **Work coordinate systems**
  - Learn G54-G59 (GRBL supports 6)
  - Use for multiple workpieces
- [ ] **Macro system**
  - Surfacing macro (flatten spoilboard)
  - Tool change prompt macro
  - Corner finding macro
- [ ] **Dust collection**
  - Mount shop vac hose to spindle
  - Add relay for auto on/off with spindle
- [ ] **Logging**
  - Raspberry Pi logs job start/end times
  - Track runtime, errors
  - SQLite database

### 2.4 Testing (1 week)
- [ ] **Accuracy test patterns**
  - Cut precision test piece (circles, pockets, text)
  - Measure with calipers
  - Document results
- [ ] **Repeatability test**
  - Run same job 5 times
  - Measure variation (should be < 0.05mm)
- [ ] **Material tests**
  - MDF, plywood, hardwood, softwood
  - Tune feeds and speeds for each
  - Document in tool library

### Phase 2 Deliverables
- ✅ Calibrated and accurate (±0.1mm)
- ✅ Auto Z-probing
- ✅ Macros for common tasks
- ✅ Dust collection integrated
- ✅ Job logging system

---

## Phase 3: 4-Axis Expansion (Rotary A)
**Duration:** 2-3 months  
**Goal:** Add rotary axis for cylindrical work

### 3.1 Mechanical Design (2-3 weeks)
- [ ] **Rotary axis design**
  - Decide: table-mounted or replace Y-axis
  - Design: chuck + tailstock (for long pieces)
  - Size: 100-200mm diameter capacity
- [ ] **CAD model**
  - Design in Fusion 360 or FreeCAD
  - Check clearances with spindle
  - Export DXF/STEP for fabrication
- [ ] **Fabrication**
  - Cut aluminum plates (laser or CNC)
  - Buy rotary table or dividing head
  - Assemble and test rotation by hand

### 3.2 Electronics (1 week)
- [ ] **4th driver**
  - Add DM542 or TB6600 for A-axis
  - Wire to MCU (STEP/DIR/ENABLE)
  - Connect NEMA 23 motor
- [ ] **Gearing (if needed)**
  - Direct drive: 1:1 (motor → rotary)
  - Geared: 5:1 or 10:1 (more torque, finer resolution)
  - Calculate steps/degree

### 3.3 Firmware Configuration (1 week)
- [ ] **Enable 4-axis in grblHAL/FluidNC**
  - Edit config for 4 axes
  - Set $103 (A steps/degree)
  - Set $113 (A max rate)
  - Set $123 (A acceleration)
  - Set $133 (A max travel = 360° or 99999 for continuous)
- [ ] **Kinematics mode**
  - Indexing: Rotate A, then cut XYZ (simpler)
  - Wrapped: A moves while XYZ moves (for spiral carving)
- [ ] **Test rotation**
  - Command `G0 A90` (rotate 90°)
  - Verify direction and accuracy

### 3.4 CAM Workflow (2-3 weeks)
- [ ] **Fusion 360 4-axis**
  - Enable 4th axis in CAM workspace
  - Create toolpath for cylinder (e.g., engraved text around pipe)
  - Post-process with 4-axis post (GRBL 4-axis)
  - Export G-code
- [ ] **Test simple job**
  - Mount wooden dowel (50mm diameter)
  - Engrave spiral or text around circumference
  - Run job in indexing mode
- [ ] **Advanced jobs**
  - Fluted column
  - Spiral rifling
  - 3D relief on cylinder

### 3.5 Testing & Refinement (1-2 weeks)
- [ ] **Accuracy**
  - Engrave degree markings (every 10°)
  - Check with protractor
  - Should be within 0.5°
- [ ] **Backlash**
  - Test forward and reverse rotation
  - Adjust if > 1° of play
- [ ] **Vibration**
  - Balance workpiece (eccentric parts cause vibration)
  - Use tailstock for long parts

### Phase 3 Deliverables
- ✅ Functional 4-axis (X, Y, Z, A)
- ✅ Indexing mode working
- ✅ CAM workflow for rotary carving
- ✅ Able to engrave cylindrical parts

---

## Phase 4: 5-Axis Capability (Advanced)
**Duration:** 3-6 months  
**Goal:** Add tilt (B) or secondary rotary (C) for complex 3D work

### 4.1 Design & Analysis (4-6 weeks)
- [ ] **Choose configuration**
  - **Trunnion table:** A-axis (rotation) + B-axis (tilt)
  - **Swivel head:** A-axis (rotation) + C-axis (spindle tilt)
  - **Hybrid:** Various combinations
- [ ] **Kinematic analysis**
  - Forward kinematics: Joint angles → tool position
  - Inverse kinematics: Desired tool orientation → joint angles
  - Collision detection (tool vs. workpiece vs. machine)
- [ ] **CAD model**
  - Full 3D model including all axes
  - Simulate full range of motion
  - Check for singularities (positions where movement is impossible)

### 4.2 Mechanical Build (6-8 weeks)
- [ ] **Fabricate B or C axis**
  - Precision required (< 0.01° alignment error)
  - Use ball bearings for tilt axis
  - Add limit switches
- [ ] **Assemble and align**
  - Mount to A-axis or spindle
  - Use laser alignment tools
  - Check runout (< 0.02mm TIR)

### 4.3 Firmware & Software (4-6 weeks)
- [ ] **Kinematics engine**
  - Implement inverse kinematics in firmware or post-processor
  - Handle tool orientation (i, j, k vectors in G-code)
  - Collision avoidance (software checks)
- [ ] **Firmware config**
  - Enable 5-axis mode
  - Set steps/degree for B/C
  - Set limits and max rates
- [ ] **Testing**
  - Simple test: Rotate tool around fixed point
  - Should maintain tool tip position

### 4.4 CAM Integration (4-6 weeks)
- [ ] **Fusion 360 5-axis**
  - Learn 5-axis CAM strategies (3+2, simultaneous)
  - Create custom post-processor for your kinematics
  - Test with simple part (e.g., impeller, turbine blade)
- [ ] **Simulation**
  - Verify toolpaths in CAM simulator
  - Check for collisions
  - Export G-code

### 4.5 Testing (2-4 weeks)
- [ ] **Accuracy tests**
  - Use ball bar or laser interferometer (if available)
  - Measure circularity, positioning error
- [ ] **Complex parts**
  - Machine 3D surface (sphere, saddle)
  - Measure with CMM or calipers
- [ ] **Production run**
  - Choose real project (e.g., mold, sculpture)
  - Run full job
  - Document lessons learned

### Phase 4 Deliverables
- ✅ Functional 5-axis CNC
- ✅ Inverse kinematics working
- ✅ Collision detection
- ✅ Able to machine complex 3D parts

---

## Phase 5: Production Ready (Ongoing)
**Duration:** Ongoing  
**Goal:** Make machine reliable, repeatable, and user-friendly

### 5.1 Reliability Improvements
- [ ] **Automatic tool changer (ATC)**
  - Design tool carousel (4-6 tools)
  - Add tool length sensor
  - Implement M6 tool change macro
- [ ] **Tool length sensor**
  - Touch-off probe on fixed location
  - Measure tool length automatically
  - Store in tool table
- [ ] **Watchdog & error recovery**
  - Firmware watchdog timer (reset if hung)
  - Power loss recovery (save position to EEPROM)
  - Resume job from middle (if interrupted)

### 5.2 User Experience
- [ ] **Improved UI**
  - Custom web dashboard (replace CNCjs)
  - Real-time 3D preview
  - Touch screen on machine
- [ ] **Job queue**
  - Upload multiple jobs
  - Run sequentially (lights-out operation)
- [ ] **Remote monitoring**
  - Camera stream
  - Email/SMS alerts on completion or error
  - Cloud logging

### 5.3 Advanced Features
- [ ] **Adaptive machining**
  - Adjust feed rate based on spindle load
  - Use current sensor on VFD
- [ ] **Vision system**
  - Camera for part alignment
  - Edge detection
  - OCR for serial numbers
- [ ] **Measurement probe**
  - 3D touch probe (Renishaw style)
  - Automatic part alignment
  - In-process inspection

### 5.4 Documentation & Training
- [ ] **User manual**
  - Complete setup guide
  - Troubleshooting section
  - CAM workflow tutorials
- [ ] **Video tutorials**
  - YouTube series
  - Assembly, calibration, first cut
- [ ] **Community**
  - Forum or Discord
  - Share G-code and projects
  - Collaborate on improvements

### Phase 5 Deliverables
- ✅ Production-ready CNC
- ✅ Automatic tool changing
- ✅ Remote monitoring
- ✅ Comprehensive documentation

---

## Risk Management

### Technical Risks
| Risk | Probability | Impact | Mitigation |
|------|-------------|---------|------------|
| Mechanical binding | Medium | High | Careful alignment, test motion by hand |
| EMI causing false triggers | High | Medium | Shielded cables, proper grounding |
| Firmware bugs | Medium | High | Use proven firmware (grblHAL), test thoroughly |
| Missed steps | Medium | High | Proper acceleration tuning, adequate voltage |
| CAM toolpath errors | Medium | High | Simulate before cutting, dry run |

### Schedule Risks
| Risk | Probability | Impact | Mitigation |
|------|-------------|---------|------------|
| Parts delivery delay | High | Low | Order parts early, have backup suppliers |
| Alignment issues | Medium | Medium | Budget extra time for mechanical setup |
| Learning curve (CAM) | High | Medium | Take online courses, practice on simple parts |
| Scope creep | High | High | Stick to phase goals, defer extras to next phase |

### Budget Risks
| Risk | Probability | Impact | Mitigation |
|------|-------------|---------|------------|
| Cost overruns | Medium | Medium | Add 20% contingency, buy quality parts first time |
| Hidden costs (tools, shipping) | High | Low | Budget for misc expenses ($200-300) |
| Part failures | Low | High | Buy spares of critical parts (drivers, MCU) |

---

## Success Metrics

### Phase 1 (MVP)
- [ ] Cut a 200×200mm pocket in MDF (±0.2mm accuracy)
- [ ] Complete 1-hour job without errors
- [ ] All safety features working (E-stop, limits)

### Phase 2 (Optimization)
- [ ] Accuracy: ±0.1mm over 400mm
- [ ] Repeatability: < 0.05mm over 10 runs
- [ ] Surface finish: No visible chatter marks

### Phase 3 (4-axis)
- [ ] Engrave text around 50mm dowel (readable, uniform)
- [ ] Complete helical cut with no missed steps

### Phase 4 (5-axis)
- [ ] Machine 3D surface (sphere, 100mm diameter)
- [ ] Measured deviation < 0.2mm from CAD model

### Phase 5 (Production)
- [ ] Run 10 jobs unattended (lights-out)
- [ ] Tool change time < 30 seconds
- [ ] Uptime > 95% over 1 month

---

## Timeline Summary

```
Month 1-3:   Phase 1 (MVP)               ████████████░░░░░░░░░░░░░░░░░░░░░░░░
Month 4-5:   Phase 2 (Optimization)      ░░░░░░░░░░░░██████░░░░░░░░░░░░░░░░░░
Month 6-8:   Phase 3 (4-axis)            ░░░░░░░░░░░░░░░░░░████████░░░░░░░░░░
Month 9-14:  Phase 4 (5-axis)            ░░░░░░░░░░░░░░░░░░░░░░░░░░██████████
Month 15+:   Phase 5 (Production)        ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██ (Ongoing)

Total to 4-axis: ~8 months
Total to 5-axis: ~14 months
Total to production: ~18+ months
```

---

## Resources Needed

### People
- **Mechanical assembly:** 1 person, basic DIY skills
- **Electrical wiring:** 1 person, basic electronics knowledge
- **Firmware/software:** 1 person, C/C++ and Python/Node.js
- **CAM:** 1 person, Fusion 360 or similar

### Budget Summary
- Phase 1 (MVP): $1,200-1,800
- Phase 2 (Opt): $200-400 (probes, tools)
- Phase 3 (4-axis): $400-600 (rotary axis)
- Phase 4 (5-axis): $800-1,500 (B/C axis)
- Phase 5 (Prod): $500-1,000 (ATC, sensors)
- **Total:** $3,100-5,300

### Tools Required
- See docs/hardware/BOM.md for full list
- Key tools: drill, taps, Allen keys, multimeter, calipers, dial indicator

---

## Next Steps

**Ready to start?**
1. Review docs/SPEC_VN.md (full specification)
2. Review docs/hardware/BOM.md (parts list)
3. Order parts for Phase 1
4. Join community (Discord, forum)
5. Begin mechanical assembly!

**Questions?**
- Post in Issues on GitHub
- Tag maintainers for help
- Share your progress!

---

**Document Version:** 1.0  
**Last Updated:** 2024  
**Review:** Update after each phase completion
