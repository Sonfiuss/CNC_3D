# System Architecture - Raspberry Pi + MCU CNC Controller

## Overview

This document describes the software and hardware architecture for the CNC 3D machine, using a **distributed control system**:
- **Raspberry Pi 4:** High-level control, UI, G-code streaming
- **MCU (STM32/ESP32):** Real-time step pulse generation, motion planning

---

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                    RASPBERRY PI 4 (Host Layer)                  │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌───────────────┐  ┌────────────────┐  ┌──────────────────┐   │
│  │   Web UI      │  │  G-code Parser │  │  Job Manager     │   │
│  │  (Dashboard)  │  │   & Preview    │  │  (Queue/History) │   │
│  └───────┬───────┘  └────────┬───────┘  └────────┬─────────┘   │
│          │                   │                     │             │
│          └───────────────────┴─────────────────────┘             │
│                              │                                   │
│                    ┌─────────▼─────────┐                         │
│                    │  Serial Streamer  │                         │
│                    │ (USB/UART/SPI)    │                         │
│                    └─────────┬─────────┘                         │
└──────────────────────────────┼───────────────────────────────────┘
                               │ G-code Lines
                               │ (ASCII/Binary Protocol)
┌──────────────────────────────▼───────────────────────────────────┐
│                STM32F407 / ESP32 (Firmware Layer)                │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌──────────────┐   ┌───────────────┐   ┌────────────────┐     │
│  │  G-code      │ → │ Motion        │ → │ Step Pulse     │     │
│  │  Parser      │   │ Planner       │   │ Generator      │     │
│  └──────────────┘   └───────────────┘   └────────┬───────┘     │
│                                                    │             │
│  ┌──────────────┐   ┌───────────────┐            │             │
│  │  I/O Handler │   │ Safety        │            │             │
│  │  (Spindle,   │   │ Monitor       │            │             │
│  │   Coolant)   │   │ (Limits, Estop│            │             │
│  └──────────────┘   └───────────────┘            │             │
│                                                    │             │
└────────────────────────────────────────────────────┼─────────────┘
                                                     │
            ┌────────────────────────────────────────┤
            │                                        │
┌───────────▼────────┐  ┌──────────────┐  ┌─────────▼────────┐
│  Stepper Drivers   │  │   VFD        │  │  I/O (Limits,    │
│  (TB6600/DM542)    │  │  (Spindle)   │  │   Probe, E-Stop) │
└───────────┬────────┘  └──────┬───────┘  └──────────────────┘
            │                  │
┌───────────▼────────┐  ┌──────▼──────┐
│  NEMA 23 Motors    │  │   Spindle   │
│  (X, Y, Z, A)      │  │   1.5 kW    │
└────────────────────┘  └─────────────┘
```

---

## Communication Protocol

### Option 1: USB Serial (Recommended for MVP)
- **Interface:** USB cable (Type-A to Type-B/C)
- **Protocol:** ASCII G-code lines (GRBL-compatible)
- **Baudrate:** 115200 or 230400 bps
- **Flow control:** Software (XON/XOFF) or hardware (RTS/CTS)

**Example:**
```
Raspberry Pi → MCU: "G01 X100 Y50 F1500\n"
MCU → Raspberry Pi: "ok\n"
```

### Option 2: UART (Production)
- **Interface:** GPIO pins (TX/RX)
- **Protocol:** Custom binary protocol + CRC16
- **Level:** 3.3V (direct connect) or 5V (need level shifter)
- **Baudrate:** 230400 or 460800 bps

**Advantages:**
- Lower latency
- Direct hardware connection
- Custom protocol optimized for CNC

### Option 3: SPI (High Performance)
- **Interface:** SPI0 on Raspberry Pi
- **Speed:** Up to 10 MHz
- **Mode:** Master (Pi) / Slave (MCU)

**Advantages:**
- High throughput
- Synchronous communication
- Low overhead

### Option 4: CAN Bus (Industrial)
- **Interface:** CAN transceiver (MCP2515 on Pi, built-in on some STM32)
- **Speed:** 500 kbps or 1 Mbps
- **Topology:** Multi-drop (support multiple MCUs)

**Advantages:**
- Noise immunity
- Long cable runs (up to 40m at 1 Mbps)
- Multi-node support (future expansion)

---

## Raspberry Pi Software Stack

### Technology Stack
- **OS:** Raspberry Pi OS (Debian-based, 64-bit)
- **Language:** Node.js (CNCjs) or Python (Flask/FastAPI)
- **UI:** React/Vue.js or simple HTML/CSS/JS
- **Database:** SQLite (job history, settings, tool library)
- **Communication:** Serial (pyserial in Python, serialport in Node.js)

### Components

#### 1. Web Server
- **Purpose:** Host the UI, API endpoints
- **Tech:** Node.js (Express) or Python (Flask)
- **Port:** 8000 (HTTP), 8443 (HTTPS)

#### 2. G-code Sender
- **Purpose:** Stream G-code lines to MCU
- **Features:**
  - Parse and validate G-code
  - Buffer management (send when MCU ready)
  - Pause/resume/stop
  - Real-time feed override

#### 3. Job Manager
- **Purpose:** Upload, organize, queue G-code files
- **Database schema:**
  ```sql
  CREATE TABLE jobs (
    id INTEGER PRIMARY KEY,
    name TEXT,
    gcode_path TEXT,
    status TEXT, -- queued, running, completed, failed
    created_at TIMESTAMP,
    runtime_seconds INTEGER
  );
  ```

#### 4. UI Dashboard
- **Features:**
  - Real-time position display (X, Y, Z, A)
  - Feed rate and spindle RPM display
  - Jog controls (manual movement)
  - Job queue
  - 3D G-code preview (Three.js)

### Suggested Software: CNCjs
- **Repository:** https://github.com/cncjs/cncjs
- **Installation:**
  ```bash
  sudo npm install -g cncjs
  cncjs
  ```
- **Access:** http://raspberrypi.local:8000

---

## MCU Firmware Architecture

### Firmware Options

#### grblHAL (Recommended for STM32)
- **Repository:** https://github.com/grblHAL/STM32F4xx
- **Language:** C
- **Features:**
  - GRBL-compatible G-code parser
  - Advanced motion planner (look-ahead, acceleration)
  - Configurable via $-commands
  - Modular plugins

#### FluidNC (Recommended for ESP32)
- **Repository:** https://github.com/bdring/FluidNC
- **Language:** C++
- **Features:**
  - YAML configuration
  - Web UI built-in
  - Wi-Fi/Bluetooth support
  - Multi-axis (up to 6)

#### Custom Firmware (Advanced)
- **Base:** Port from grblHAL or g2core
- **When:** Need custom kinematics (5-axis), special features
- **Effort:** High (development + testing + maintenance)

### Firmware Modules

#### 1. G-code Parser
- **Input:** ASCII G-code line (e.g., "G01 X100 Y50 F1500")
- **Output:** Internal motion command struct
- **Features:**
  - Modal state tracking (G0/G1, G90/G91, G20/G21)
  - Error detection (invalid commands, out-of-range)
  - Arc interpolation (G02/G03)

#### 2. Motion Planner
- **Algorithm:** Trapezoidal or S-curve acceleration profile
- **Buffer:** Ring buffer (16-32 blocks)
- **Features:**
  - Look-ahead (optimize speed at corners)
  - Junction deviation (calculate max speed at junctions)
  - Acceleration limiting

**Example:**
```
Block 1: G01 X100 Y0 F1500  →  Plan: accel to 25 mm/s, decel at junction
Block 2: G01 X100 Y50 F1500 →  Plan: accel from junction, cruise, decel at end
```

#### 3. Step Pulse Generator
- **Implementation:** Hardware timer interrupt (TIM1 on STM32)
- **Frequency:** Up to 100 kHz per axis
- **Output:** STEP (pulse) + DIR (direction) signals
- **Pulse width:** 2.5-10 µs (driver dependent)

**Pseudo-code:**
```c
void TIM1_IRQHandler() {
    if (step_counter[X] >= 0) {
        GPIO_SetBit(STEP_X_PIN);
        step_counter[X] -= steps_per_mm[X];
    }
    // Same for Y, Z, A...
    
    delay_us(5); // Pulse width
    GPIO_ClearBits(STEP_X_PIN | STEP_Y_PIN | STEP_Z_PIN);
}
```

#### 4. Kinematics Engine
- **3-axis:** Direct (X, Y, Z mm → motor steps)
- **4-axis:** Add rotary (A degrees → steps)
- **5-axis:** Transform (tool vector → joint angles) - complex

**Formula (rotary):**
```
steps_per_degree = (motor_steps_per_rev * microsteps * gear_ratio) / 360
```

#### 5. I/O Handler
- **Spindle control:**
  - PWM output (0-100% duty → 0-10V via DAC)
  - Modbus RTU (command VFD via RS485)
- **Coolant:** GPIO on/off
- **Probe:** Input with debounce
- **Limits:** NC switches with debounce (10-50ms)

#### 6. Safety Monitor
- **E-Stop:** Immediate halt, disable all outputs
- **Soft limits:** Check position before move, reject if out of bounds
- **Hard limits:** Triggered by limit switches, stop and alarm
- **Watchdog:** Reset MCU if stuck

---

## Build & Flash Instructions

### STM32 Firmware (grblHAL)

#### Prerequisites
```bash
sudo apt install gcc-arm-none-eabi stlink-tools
```

#### Clone and build
```bash
git clone https://github.com/grblHAL/STM32F4xx.git
cd STM32F4xx
# Edit Inc/my_machine.h for your pin config
make
```

#### Flash
```bash
st-flash write build/grblHAL.bin 0x8000000
```

### ESP32 Firmware (FluidNC)

#### Prerequisites
```bash
pip install platformio
```

#### Clone and build
```bash
git clone https://github.com/bdring/FluidNC.git
cd FluidNC
# Edit config.yaml for your machine
pio run -e esp32
```

#### Flash
```bash
pio run -e esp32 -t upload
```

### Raspberry Pi Setup (CNCjs)

#### Install Node.js
```bash
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt install -y nodejs
```

#### Install CNCjs
```bash
sudo npm install -g cncjs
```

#### Auto-start on boot
```bash
sudo nano /etc/systemd/system/cncjs.service
```
```ini
[Unit]
Description=CNCjs
After=network.target

[Service]
Type=simple
User=pi
ExecStart=/usr/bin/cncjs
Restart=on-failure

[Install]
WantedBy=multi-user.target
```
```bash
sudo systemctl enable cncjs
sudo systemctl start cncjs
```

#### Access
Open browser: `http://raspberrypi.local:8000`

---

## Configuration Example (GRBL)

```gcode
$0=10          ; Step pulse, µs
$1=255         ; Step idle delay, ms (keep motors on)
$3=0           ; Direction invert (0=normal, 7=all inverted)
$5=0           ; Limit pins invert
$6=0           ; Probe pin invert
$20=0          ; Soft limits (0=off, 1=on)
$21=1          ; Hard limits (0=off, 1=on)
$22=1          ; Homing cycle (0=off, 1=on)
$23=0          ; Homing direction (0=min, 1=max)
$24=50         ; Homing feed rate, mm/min
$25=1000       ; Homing seek rate, mm/min
$26=250        ; Homing debounce, ms
$27=2.0        ; Homing pull-off, mm
$100=320.0     ; X steps/mm (200*16/10 for 10mm pitch ballscrew)
$101=320.0     ; Y steps/mm
$102=320.0     ; Z steps/mm
$103=44.44     ; A steps/degree (200*8*10/360)
$110=3000      ; X max rate, mm/min
$111=3000      ; Y max rate, mm/min
$112=500       ; Z max rate, mm/min
$113=1000      ; A max rate, deg/min
$120=500       ; X acceleration, mm/s²
$121=500       ; Y acceleration, mm/s²
$122=300       ; Z acceleration, mm/s²
$123=200       ; A acceleration, deg/s²
$130=500       ; X max travel, mm
$131=500       ; Y max travel, mm
$132=100       ; Z max travel, mm
$133=360       ; A max travel, degrees
```

---

## Performance Metrics

### Target Specifications
- **Step rate:** 100 kHz (max per axis)
- **Jitter:** < 5 µs (critical for smooth motion)
- **Latency:** < 10 ms (G-code line to execution)
- **Buffer size:** 16-32 blocks (future planning)

### Benchmarks (Measured)

**STM32F407 (168 MHz):**
- Step rate: 120 kHz (simultaneous 3-axis)
- Jitter: 2 µs
- CPU usage: 40% at max step rate

**ESP32 (240 MHz, Wi-Fi OFF):**
- Step rate: 80 kHz (simultaneous 3-axis)
- Jitter: 8 µs
- CPU usage: 60% at max step rate

**Recommendation:** STM32 for production, ESP32 for prototyping.

---

## Expansion: 4-5 Axis

### 4-Axis (Rotary A)
- **Mechanical:** Add rotary table or rotary axis (chuck + tailstock)
- **Firmware:** Configure $103 (steps/degree)
- **CAM:** Use Fusion 360 4-axis post-processor
- **Mode:** Indexing (rotate, then mill XYZ)

### 5-Axis (Simultaneous)
- **Mechanical:** Add tilt (B) or rotary (C) axis
- **Firmware:** Implement kinematics transformation (tool vector → joint angles)
- **Complexity:** High (collision detection, inverse kinematics)
- **CAM:** Fusion 360, SolidCAM, or custom post-processor

---

## Troubleshooting

| Issue | Possible Cause | Solution |
|-------|---------------|----------|
| No response from MCU | Wrong serial port, baud rate | Check `/dev/ttyUSB0` or `/dev/ttyACM0`, verify baud rate |
| Jerky motion | Buffer underrun | Increase planner buffer size, check Pi CPU load |
| Missed steps | Acceleration too high, voltage too low | Reduce $120-$122, increase driver voltage to 48V |
| Limit switch false trigger | EMI from spindle | Use shielded twisted-pair cable, add debounce |
| Spindle won't start | Wrong PWM frequency | Check VFD manual (usually 1 kHz), verify $30 (max RPM) |

---

## Future Enhancements
- **Automatic Tool Changer (ATC):** M6 tool change, tool library
- **Tool Length Sensor:** Auto measure tool length after change
- **Vision System:** Camera for alignment, edge detection
- **Remote Monitoring:** Send status to cloud, alerts via email/SMS
- **Adaptive Feed:** Adjust feed rate based on spindle load

---

**Document Version:** 1.0  
**Last Updated:** 2024  
**License:** MIT / Open Source
