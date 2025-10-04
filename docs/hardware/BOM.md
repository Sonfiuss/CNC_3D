# Hardware Bill of Materials (BOM)

**CNC 3-Axis Wood Machine - 500×500mm**

This document lists all mechanical, electrical, and electronic components needed to build the complete CNC machine.

---

## Summary

| Category | Estimated Cost (USD) |
|----------|---------------------|
| Mechanical (frame, motion) | $400-600 |
| Motors & Drivers | $250-400 |
| Spindle & VFD | $200-350 |
| Control Electronics | $100-150 |
| Sensors & I/O | $50-80 |
| Wiring & Accessories | $80-120 |
| Electrical Enclosure | $50-80 |
| **Total** | **$1,130-1,780** |

---

## 1. Mechanical Components

### Frame & Structure

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| Aluminum extrusion 2060 | 10m | 6-slot, anodized | Main frame | $150-200 |
| Aluminum extrusion 2040 | 5m | 4-slot, anodized | Gantry supports | $60-80 |
| Aluminum plate 8mm | 1 pc | 200×150mm | Z-axis mount | $20-30 |
| L-brackets | 20 | 2060/2040 compatible | Corner joints | $20-30 |
| T-nuts M5 | 100 | Drop-in or hammer | Fasteners | $10-15 |
| M5 screws (assorted) | 200 | 10mm, 16mm, 20mm | Various lengths | $15-20 |
| Cross-bracing plates | 4 | Custom cut aluminum | Stiffening | $20-30 |

### Motion System (Option A: Ballscrew)

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| Ballscrew SFU1605 | 3 | 600mm (X,Y), 200mm (Z) | C7 precision | $120-180 |
| Ballscrew nut | 3 | Flange type, anti-backlash | | $30-45 |
| BK12 bearing block | 6 | Fixed end | Support ballscrew | $30-40 |
| BF12 bearing block | 6 | Floating end | Support ballscrew | $30-40 |
| Flexible coupler | 3 | 6.35mm-8mm (motor-screw) | Aluminum | $15-20 |

### Motion System (Option B: Belt Drive)

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| GT2 timing belt | 10m | 6mm width | | $20-30 |
| GT2 pulley 20T | 6 | 8mm bore (motor shaft) | | $20-30 |
| GT2 idler pulley | 6 | Smooth or toothed | Tension | $20-30 |
| Belt tensioner | 3 | Adjustable | | $15-20 |

### Linear Guides

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| MGN15 rail | 2 | 600mm | X, Y axes | $60-80 |
| MGN15H block | 4 | High load capacity | | $40-60 |
| MGN12 rail | 1 | 200mm | Z axis | $20-30 |
| MGN12H block | 2 | Standard | | $15-25 |

### Spoilboard & Workholding

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| MDF board | 1 | 500×500×18mm | Replaceable | $10-15 |
| T-track extrusions | 4 | 500mm length | Clamping | $40-60 |
| Hold-down clamps | 6 | T-slot compatible | Workpiece clamping | $30-40 |
| Threaded inserts | 50 | M5 or M6 | MDF mounting | $10-15 |

---

## 2. Motors & Drivers

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| NEMA 23 stepper motor | 3-4 | 2.8A, 2.2-3.0 N·m | X, Y, Z (+ A) | $90-150 |
| TB6600 driver | 3-4 | 4.2A max, 18-50VDC | Basic option | $40-60 |
| DM542 driver | 3-4 | 4.2A max, 20-50VDC | Better quality | $60-100 |
| TMC5160 driver | 3-4 | SPI, silent, stallguard | Premium option | $100-180 |
| 48VDC power supply | 1 | 10-15A (500-720W) | Mean Well LRS-600-48 | $60-80 |
| 24VDC power supply | 1 | 5A (120W) | Auxiliary power | $20-30 |
| 5VDC power supply | 1 | 5A (25W) | Logic, Raspberry Pi | $10-15 |

---

## 3. Spindle & Tooling

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| Spindle motor | 1 | 1.5kW, ER11, air-cooled | 24,000 RPM max | $80-120 |
| VFD (inverter) | 1 | 1.5-2.2kW, single phase | Modbus or PWM | $80-120 |
| Spindle mount | 1 | 65mm clamp diameter | Aluminum | $20-30 |
| ER11 collet set | 1 | 1-7mm (10 pieces) | Precision ground | $20-30 |
| End mills (2-flute) | 5 | 3.175mm, 6mm carbide | Wood cutting | $20-30 |
| V-bit set | 1 | 60°, 90° angles | Engraving | $15-25 |
| Dust shoe | 1 | Custom or commercial | Chip collection | $20-40 |

---

## 4. Control Electronics

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| Raspberry Pi 4 | 1 | 4GB RAM + heatsink | Host controller | $55-75 |
| MicroSD card | 1 | 32GB Class 10 | OS storage | $10-15 |
| STM32F407 dev board | 1 | 168MHz, breakout pins | Or Nucleo-F407 | $15-25 |
| ESP32 dev board (alt) | 1 | 240MHz, Wi-Fi/BT | Alternative MCU | $10-15 |
| USB cable A-B | 1 | 1.5m | Pi to MCU | $5-10 |
| Breakout board (CNC shield) | 1 | Step/Dir outputs | Optional | $10-20 |
| Logic level shifter | 1 | 3.3V ↔ 5V (if needed) | Bi-directional | $3-5 |

---

## 5. Sensors & I/O

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| Mechanical limit switch | 6-9 | NC micro switch | V-15-1C25 or similar | $10-15 |
| Inductive proximity sensor (alt) | 3-6 | NPN NO, 5-24VDC | More reliable | $20-40 |
| Z-probe touch plate | 1 | Aluminum 100×100×10mm | DIY or buy | $10-20 |
| E-Stop button | 1 | NC mushroom head (red) | 40mm diameter | $10-15 |
| Feed hold button | 1 | NO momentary (green) | Pause function | $5-10 |
| Relay module (2ch) | 1 | 24VDC coil, 10A contacts | Spindle, vac | $8-12 |
| Optical RPM sensor (opt) | 1 | Reflective or slotted | Spindle feedback | $10-20 |

---

## 6. Wiring & Accessories

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| Stepper motor cable | 10m | 4-wire shielded, 22AWG | 2.5mm² flexible | $15-25 |
| Spindle cable | 3m | 4-core shielded, 18AWG | VFD to spindle | $10-15 |
| Signal cable (twisted pair) | 10m | 2-4 pairs, 22-24AWG | Limits, probe | $10-15 |
| Drag chain (cable carrier) | 3m | 15×15mm or 15×20mm | X, Y, Z axes | $20-30 |
| Spiral wrap / loom | 5m | 10mm diameter | Cable management | $5-10 |
| Terminal blocks | 10 | 10A screw type | Wiring distribution | $10-15 |
| Ferrules & crimps | Assorted | 22-18 AWG | Professional finish | $10-15 |
| Fuse holders | 5 | Panel mount, 5-20A | Safety | $10-15 |
| Heat shrink tubing | 1 set | Various sizes | Insulation | $5-10 |
| Cable ties | 100 | 150-200mm | Cable management | $5-10 |
| Spade/ring terminals | 50 | M3, M4, M5 | Ground connections | $5-10 |

---

## 7. Electrical Enclosure

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| Plastic/metal enclosure | 1 | 400×300×200mm (IP54) | Control box | $30-50 |
| DIN rail | 1m | 35mm standard | Mount drivers | $5-10 |
| Cooling fan 120mm | 2 | 24VDC, ball bearing | Enclosure ventilation | $15-25 |
| Fan grill & filter | 2 | 120mm | Dust protection | $5-10 |
| LED indicators | 3-5 | Red, green, blue | Status lights | $5-10 |
| Rocker switch | 1 | 20A, illuminated | Main power | $5-10 |
| Contactor (relay) | 1 | 25A, 24VDC coil | E-stop cutoff | $15-25 |
| Cable glands | 10 | PG9, PG11, PG13.5 | Cable entry | $10-15 |

---

## 8. Miscellaneous

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| Grease (NLGI-2) | 1 tube | Lithium or PTFE | Ballscrew/rails | $5-10 |
| Loctite threadlocker | 1 bottle | Blue 243 | Screw retention | $5-10 |
| Anti-vibration pads | 4 | Rubber 50×50mm | Machine feet | $10-15 |
| Touch-up paint | 1 can | Black or gray spray | Frame finish | $5-10 |
| Shop vacuum adapter | 1 | 40-50mm hose | Dust collection | $10-20 |
| Calibration tools | 1 set | Dial indicator, square | Alignment | $30-50 |

---

## 9. Optional/Future Upgrades

| Item | Qty | Specification | Notes | Est. Price |
|------|-----|---------------|-------|------------|
| Rotary axis (A) | 1 | Chuck + tailstock | 4-axis capability | $150-300 |
| Tool length sensor | 1 | Touch-off probe | Auto tool change | $50-100 |
| 3D touch probe | 1 | XYZ probe (Renishaw style) | Part alignment | $100-200 |
| Camera + LED ring | 1 | USB, 1080p | Vision system | $50-80 |
| Automatic tool changer | 1 | 4-6 tools | Advanced | $500-1000 |
| Water cooling system | 1 | Chiller for spindle | High duty cycle | $150-300 |
| Enclosure (full) | 1 | Acrylic/polycarbonate | Noise/dust | $200-400 |

---

## 10. Tools Needed for Assembly

| Tool | Notes |
|------|-------|
| Allen key set (metric) | M3-M10 |
| Wrench set (metric) | 8mm-19mm |
| Screwdriver set | Phillips + flathead |
| Drill + bits | For mounting holes |
| Tap set | M5, M6 threads |
| Caliper (digital) | Measure precision |
| Square (machinist) | Check perpendicularity |
| Level | Frame alignment |
| Multimeter | Electrical testing |
| Wire stripper/crimper | Cable prep |
| Soldering iron | Optional (for terminals) |
| Heat gun | Shrink tubing |

---

## Notes

1. **Prices are estimates** (2024, USD). Actual costs vary by supplier (AliExpress, Amazon, local).
2. **Option A vs B:** Choose ballscrew (precision) or belt (speed/cost).
3. **Driver choice:** TB6600 (budget), DM542 (balanced), TMC5160 (premium/silent).
4. **MCU choice:** STM32 (best jitter) or ESP32 (Wi-Fi, easier dev).
5. **Country-specific:** Add VAT/import tax, shipping.

---

## Purchasing Tips

- **Bulk orders:** Buy from one supplier to save shipping.
- **Quality over price:** Don't skimp on ballscrews, linear rails, or spindle.
- **Verify specs:** Check motor holding torque, driver current rating.
- **Lead time:** Some parts (e.g., ballscrews) may take 2-4 weeks shipping.
- **Return policy:** Buy from reputable sellers with good reviews.

---

## Suppliers (Examples)

- **Mechanical:** AliExpress, eBay, local CNC supply shops
- **Electronics:** Digi-Key, Mouser, AliExpress
- **Raspberry Pi:** Official distributors, Adafruit, SparkFun
- **Spindle/VFD:** AliExpress, eBay (search "1.5kW spindle ER11")
- **Linear rails:** AliExpress (Hiwin-compatible), Amazon

---

**Document Version:** 1.0  
**Last Updated:** 2024
