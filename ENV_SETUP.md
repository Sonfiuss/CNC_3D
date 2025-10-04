# Environment Setup & Build Guide

# Environment Setup & Build Guide

## 1. Toolchain & Versions (Project Stack)

### Development Machine (Windows/Linux/Mac)
- **OS:** Windows 10/11, Ubuntu 20.04+, or macOS 11+
- **Compiler (C++):** MinGW g++ 6.3.0+ (Windows) or GCC 9.0+ (Linux)
- **CMake:** ≥3.10 (3.20+ recommended)
- **IDE:** VS Code, CLion, or Qt Creator

### Raspberry Pi (Host Controller)
- **Hardware:** Raspberry Pi 4 Model B (4GB RAM recommended)
- **OS:** Raspberry Pi OS (64-bit, Debian Bookworm/Bullseye)
- **Node.js:** v18+ (for CNCjs) or Python 3.9+ (for Flask UI)
- **Display:** HDMI monitor or headless (SSH/VNC)

### MCU (Real-time Controller)
- **Option A - STM32F407:**
  - Board: STM32F407VET6 or Nucleo-F407ZG
  - Clock: 168 MHz
  - Toolchain: arm-none-eabi-gcc + OpenOCD or ST-Link
  - IDE: STM32CubeIDE or PlatformIO
- **Option B - ESP32:**
  - Board: ESP32-WROOM-32 or ESP32-DevKitC
  - Clock: 240 MHz (dual-core)
  - Toolchain: ESP-IDF or Arduino framework
  - IDE: Arduino IDE, PlatformIO, or ESP-IDF

### Qt (UI - Currently Excluded)
- **Planned:** Qt 6.9.1 (desktop) / Qt 5.15.15 (Raspberry Pi)
- **Not yet integrated** in build

## 2. Repository Layout

```
CNC_3D/
├── CMakeLists.txt          # Root build script (Model, Control, Utils, Lib, Common, Program)
├── README.md               # Project overview
├── ENV_SETUP.md            # This file: environment setup
├── .gitignore              # Ignore build artifacts
├── build_program.bat       # Windows build script
│
├── src/                    # Application source code
│   ├── Program.cpp         # Entry point (non-UI build)
│   ├── Model/              # Data models (e.g., DrillBits)
│   ├── Control/            # Control logic (motion planning, etc.)
│   ├── Utils/              # Helper utilities
│   ├── Lib/                # Shared libraries
│   ├── Common/             # Shared configs (DrillBitConfig, ToolConfig)
│   ├── Config/             # Config data (binary/csv tool data)
│   └── UI/                 # Qt UI (currently excluded from build)
│
├── docs/                   # Documentation
│   ├── SPEC_VN.md          # Vietnamese specification (15 sections)
│   ├── ARCHITECTURE.md     # System architecture (Pi + MCU)
│   ├── hardware/
│   │   └── BOM.md          # Bill of materials
│   ├── firmware/           # MCU firmware docs (planned)
│   └── software/           # Software docs (planned)
│
├── raspberry_pi/           # Raspberry Pi host software (planned)
│   ├── cncjs/              # CNCjs configuration
│   └── UI/                 # Custom web UI (if not using CNCjs)
│
├── firmware/               # MCU firmware (planned)
│   ├── stm32/              # STM32 grblHAL port
│   └── esp32/              # ESP32 FluidNC port
│
└── build/                  # Build output (gitignored)
```

## 3. Prerequisites Installation (Windows)
1. Install MinGW (ensure `g++`, `mingw32-make` in PATH):
   - Recommended: MSYS2 or WinLibs build for newer GCC.
2. Install CMake (>=3.10) and add to PATH.
3. (Optional) Install Ninja if you prefer faster incremental builds.
4. (Optional UI) Install Qt:
   - Use Qt Online Installer; select Qt 6.9.1 MinGW 64-bit AND/OR Qt 5.15.15.
5. (Optional) Git client for version control.

## 4. Environment Variables (Optional)
Add to PATH (example):
```
C:\MinGW\bin;C:\Program Files\CMake\bin;C:\Qt\6.9.1\mingw_64\bin
```
Verify:
```
g++ --version
cmake --version
```

## 5. Clean Build (CLI – MinGW Makefiles)
```
# From repo root
rmdir /s /q build 2>nul
mkdir build
cd build
cmake -G "MinGW Makefiles" ..
mingw32-make -j4
CNC_3D.exe
```

## 6. Switching Generators
If you previously configured with Ninja:
```
rmdir /s /q build
mkdir build && cd build
cmake -G "MinGW Makefiles" ..
```
Or keep Ninja:
```
cmake -G Ninja ..
ninja
```

## 7. Adding the UI Later (Qt)
Steps when ready:
1. Add Qt find + target in `CMakeLists.txt`:
```
find_package(Qt6 COMPONENTS Core Gui Qml Quick REQUIRED)
# or Qt5 ...
```
2. Append UI sources to target, enable AUTOMOC / AUTORCC / AUTOUIC.
3. Add `qt_add_qml_module` or resources for QML if needed.
4. Reconfigure + rebuild.

## 8. DrillBit Data Workflow
- Binary file path: `src/Config/DrillBit/DrilBit.bin`
- Managed via `Common::DrillBitConfig` class.
- To refresh data: modify loader or regenerate binary from CSV (script TBD).

## 9. Raspberry Pi Setup (Host Controller)

### Native Build on Raspberry Pi

#### Install Dependencies
```bash
sudo apt update
sudo apt install -y build-essential cmake git nodejs npm python3 python3-pip
```

#### Clone Repository
```bash
git clone https://github.com/Sonfiuss/CNC_3D.git
cd CNC_3D
mkdir build && cd build
cmake .. -DCMAKE_BUILD_TYPE=Release
make -j4
```

#### Install CNCjs (Recommended Host Software)
```bash
sudo npm install -g cncjs@latest --unsafe-perm
```

#### Auto-start CNCjs on Boot
```bash
sudo nano /etc/systemd/system/cncjs.service
```
Add:
```ini
[Unit]
Description=CNCjs
After=network.target

[Service]
Type=simple
User=pi
ExecStart=/usr/bin/cncjs
Restart=on-failure
RestartSec=10

[Install]
WantedBy=multi-user.target
```
Enable:
```bash
sudo systemctl enable cncjs
sudo systemctl start cncjs
```

#### Access CNCjs
Open browser: `http://raspberrypi.local:8000`

### Cross-Compile for Raspberry Pi (From PC)

#### Install Toolchain (Ubuntu/Debian)
```bash
sudo apt install -y gcc-arm-linux-gnueabihf g++-arm-linux-gnueabihf
```

#### Create Toolchain File
`toolchain-rpi.cmake`:
```cmake
set(CMAKE_SYSTEM_NAME Linux)
set(CMAKE_SYSTEM_PROCESSOR arm)

set(CMAKE_C_COMPILER arm-linux-gnueabihf-gcc)
set(CMAKE_CXX_COMPILER arm-linux-gnueabihf-g++)

set(CMAKE_FIND_ROOT_PATH /usr/arm-linux-gnueabihf)
set(CMAKE_FIND_ROOT_PATH_MODE_PROGRAM NEVER)
set(CMAKE_FIND_ROOT_PATH_MODE_LIBRARY ONLY)
set(CMAKE_FIND_ROOT_PATH_MODE_INCLUDE ONLY)
```

#### Cross-compile
```bash
mkdir build-rpi && cd build-rpi
cmake .. -DCMAKE_TOOLCHAIN_FILE=../toolchain-rpi.cmake -DCMAKE_BUILD_TYPE=Release
make -j$(nproc)
```

#### Transfer to Pi
```bash
scp CNC_3D pi@raspberrypi.local:/home/pi/
```

## 10. STM32 Firmware Setup (Real-time Controller)

### Option A: Using STM32CubeIDE

#### Install STM32CubeIDE
- Download from: https://www.st.com/en/development-tools/stm32cubeide.html
- Install on Windows/Linux/Mac

#### Clone grblHAL
```bash
git clone --recursive https://github.com/grblHAL/STM32F4xx.git
cd STM32F4xx
```

#### Configure Your Board
Edit `Inc/my_machine.h`:
```c
// Pin definitions for your board
#define X_STEP_PORT         GPIOA
#define X_STEP_PIN          0
#define X_DIRECTION_PORT    GPIOA
#define X_DIRECTION_PIN     1
// ... (repeat for Y, Z, A)
```

#### Build and Flash
1. Open STM32CubeIDE
2. Import existing project: `File → Import → Existing Projects`
3. Select `STM32F4xx` folder
4. Build: `Project → Build All`
5. Flash: Connect ST-Link, click Debug/Run

### Option B: Using PlatformIO (Command Line)

#### Install PlatformIO
```bash
pip install platformio
```

#### Clone grblHAL with PlatformIO Support
```bash
git clone https://github.com/grblHAL/STM32F4xx.git
cd STM32F4xx
```

Create `platformio.ini`:
```ini
[env:genericSTM32F407VET6]
platform = ststm32
board = genericSTM32F407VET6
framework = stm32cube
upload_protocol = stlink
build_flags = -DSTM32F407xx -DUSE_HAL_DRIVER
```

#### Build and Upload
```bash
pio run -e genericSTM32F407VET6
pio run -e genericSTM32F407VET6 -t upload
```

### Option C: Using arm-none-eabi-gcc (Makefile)

#### Install Toolchain (Ubuntu/Debian)
```bash
sudo apt install -y gcc-arm-none-eabi binutils-arm-none-eabi newlib-arm-none-eabi
sudo apt install -y stlink-tools openocd
```

#### Build
```bash
cd STM32F4xx
make clean
make all
```

#### Flash with ST-Link
```bash
st-flash write build/grblHAL.bin 0x8000000
```

### Serial Connection Test
```bash
# On Raspberry Pi
sudo apt install -y minicom
minicom -D /dev/ttyUSB0 -b 115200

# Type: $$ (to see grblHAL settings)
# Type: $H (to home the machine)
```

---

## 10.5. ESP32 Firmware Setup (Alternative Real-time Controller)

### Option A: Arduino IDE (Easiest)

#### Install Arduino IDE
- Download from: https://www.arduino.cc/en/software

#### Install ESP32 Board Support
1. Open Arduino IDE
2. `File → Preferences → Additional Board Manager URLs`
3. Add: `https://raw.githubusercontent.com/espressif/arduino-esp32/gh-pages/package_esp32_index.json`
4. `Tools → Board → Boards Manager → Search "ESP32" → Install`

#### Clone FluidNC
```bash
git clone https://github.com/bdring/FluidNC.git
cd FluidNC
```

#### Open in Arduino IDE
1. `File → Open → FluidNC/FluidNC.ino`
2. `Tools → Board → ESP32 Dev Module`
3. `Tools → Upload Speed → 921600`
4. Connect USB, select COM port
5. Click Upload

### Option B: PlatformIO (Recommended)

#### Install PlatformIO
```bash
pip install platformio
```

#### Clone and Build
```bash
git clone https://github.com/bdring/FluidNC.git
cd FluidNC
pio run -e esp32
pio run -e esp32 -t upload
```

#### Monitor Serial
```bash
pio device monitor -b 115200
```

### Option C: ESP-IDF (Advanced)

#### Install ESP-IDF
```bash
mkdir -p ~/esp
cd ~/esp
git clone --recursive https://github.com/espressif/esp-idf.git
cd esp-idf
./install.sh esp32
. ./export.sh
```

#### Build FluidNC
```bash
cd FluidNC
idf.py build
idf.py -p /dev/ttyUSB0 flash monitor
```

### Configuration (FluidNC)

#### Create config.yaml
```yaml
name: "CNC 500x500"
board: "ESP32 Dev Board"

stepping:
  engine: RMT
  idle_ms: 250
  pulse_us: 4
  dir_delay_us: 0
  disable_delay_us: 0

axes:
  x:
    steps_per_mm: 320
    max_rate_mm_per_min: 3000
    acceleration_mm_per_sec2: 500
    max_travel_mm: 500
    soft_limits: true
    homing:
      cycle: 1
      positive_direction: false
      mpos_mm: 0
      feed_mm_per_min: 50
      seek_mm_per_min: 1000
  y:
    steps_per_mm: 320
    max_rate_mm_per_min: 3000
    # ... (same as X)
  z:
    steps_per_mm: 320
    max_rate_mm_per_min: 500
    # ... (different for Z)

spi:
  miso_gpio: 19
  mosi_gpio: 23
  sck_gpio: 18
```

#### Upload Config
1. Connect ESP32
2. Access WebUI: `http://esp32.local`
3. Upload `config.yaml`

### Wi-Fi Setup (FluidNC)
- **SSID:** Set in config or AP mode
- **Access:** `http://fluidnc.local` or IP address
- **Disable during machining:** Add `M116` (disable Wi-Fi) to job start G-code

## 11. Common Issues & Fixes

| Issue | Cause | Fix |
|-------|-------|-----|
| Undefined reference to DrillBitConfig | Source not added | Ensure `src/Common/*.cpp` included in CMake |
| Fatal: cannot find Drillbit.h | Include path mismatch | Add `include_directories(src/Model)` & use `#include "Drillbit.h"` |
| Generator mismatch (Ninja vs MinGW) | Re-used cache | Delete `build/` before reconfigure |
| Qt header missing (qqmlprivate.h) | UI not yet configured | Exclude UI or install Qt dev libs |
| **Raspberry Pi Issues** | | |
| CNCjs won't start | Node.js version too old | `curl -fsSL https://deb.nodesource.com/setup_18.x \| sudo -E bash -` |
| Cannot access on port 8000 | Firewall | `sudo ufw allow 8000` |
| Serial permission denied | User not in dialout group | `sudo usermod -a -G dialout pi` (re-login) |
| **STM32 Issues** | | |
| ST-Link not detected | Driver not installed | Install STM32CubeIDE or use `sudo apt install stlink-tools` |
| Flash failed | Wrong board | Check `platformio.ini` board type |
| No serial output | Wrong pins | Verify USART pins in CubeMX |
| **ESP32 Issues** | | |
| Upload failed | Wrong COM port | Check `pio device list` or Arduino Tools→Port |
| Boot loop | Corrupted firmware | `esptool.py erase_flash` then re-upload |
| Wi-Fi jitter on steps | Wi-Fi active during machining | Disable Wi-Fi: `M116` in G-code |

## 12. Recommended Upgrades
- Upgrade GCC (current 6.3.0 lacks full C++17 bug fixes).
- Add unit tests (e.g., GoogleTest) for config loaders.
- Introduce clang-format + static analysis.

## 13. Quick Test After Build
```
CNC_3D.exe
```
Expected: lists loaded DrillBits (if binary data present).

## 14. Next Steps

### For Application Development (Raspberry Pi Host)
- Install and configure CNCjs or build custom UI
- Set up job queue and G-code preview
- Implement macro system for common operations
- Add database for tool library and job history

### For Firmware Development (MCU)
- Configure grblHAL or FluidNC for your pin layout
- Calibrate steps/mm, acceleration, max rates
- Set up homing sequence and soft limits
- Test motion planner with complex G-code

### For System Integration
- Wire Raspberry Pi to MCU (USB Serial or UART)
- Connect stepper drivers to MCU
- Wire limit switches, E-stop, probe
- Configure VFD for spindle control (PWM or Modbus)
- Test full workflow: Upload G-code → Stream → Execute

### For Testing & Calibration
- Mechanical: Check squareness, parallelism, preload
- Electrical: Verify voltages, check for EMI
- Motion: Run test patterns (squares, circles, arcs)
- Accuracy: Measure actual vs. commanded movement

### Documentation to Read
1. **docs/SPEC_VN.md** - Full Vietnamese specification (15 sections)
2. **docs/ARCHITECTURE.md** - System architecture details
3. **docs/hardware/BOM.md** - Complete parts list
4. **grblHAL wiki** - https://github.com/grblHAL/core/wiki
5. **FluidNC wiki** - http://wiki.fluidnc.com/

---
Maintainer Notes: Update this file when tool versions or build steps change.
