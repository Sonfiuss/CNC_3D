# Environment Setup & Build Guide

## 1. Toolchain & Versions (Current Project Baseline)
- OS: Windows 10/11 (x64)
- Compiler: MinGW g++ 6.3.0 (Update recommended: 12.x+ for C++17 completeness)
- CMake: (Check with `cmake --version`) – Minimum required: 3.10
- Qt (UI part – currently excluded from build): Planned Qt 6.9.1 / Optionally Qt 5.15.15 for Raspberry Pi compatibility
- Raspberry Pi Target: Raspberry Pi OS (Debian based) – 32/64 bit (Bookworm/Bullseye)
- STM32 Tooling (planned): STM32CubeMX + arm-none-eabi-gcc (not yet integrated)

## 2. Repository Layout (Active Modules Only)
```
CMakeLists.txt         # Root build script (Model, Control, Utils, Lib, Common, Program)
src/
  Program.cpp          # Entry point (non-UI build)
  Model/               # Data models (e.g., DrillBits)
  Control/             # Control logic (motion planning, etc.)
  Utils/               # Helper utilities
  Lib/                 # (Placeholder for shared libs)
  Common/              # Shared configs (DrillBitConfig, ToolConfig)
  Config/              # Config data (binary/csv tool data)
```
(UI sources under `src/UI/` are intentionally excluded from current build.)

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

## 9. Raspberry Pi Cross / Native Build (Planned)
Option A – Native build on Pi:
```
sudo apt update
sudo apt install build-essential cmake git
mkdir build && cd build
cmake .. -DCMAKE_BUILD_TYPE=Release
make -j4
```
Option B – Cross compile (future): set up ARM toolchain + toolchain file.

## 10. STM32 Firmware (Planned Integration)
- Use `stm32_firmware/` subtree for axis controllers.
- Build with arm-none-eabi-gcc via CMake toolchain or STM32CubeIDE.

## 11. Common Issues & Fixes
| Issue | Cause | Fix |
|-------|-------|-----|
| Undefined reference to DrillBitConfig | Source not added | Ensure `src/Common/*.cpp` included in CMake |
| Fatal: cannot find Drillbit.h | Include path mismatch | Add `include_directories(src/Model)` & use `#include "Drillbit.h"` |
| Generator mismatch (Ninja vs MinGW) | Re-used cache | Delete `build/` before reconfigure |
| Qt header missing (qqmlprivate.h) | UI not yet configured | Exclude UI or install Qt dev libs |

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
- Integrate motion planner skeleton.
- Add logging abstraction (serial/file).

---
Maintainer Notes: Update this file when tool versions or build steps change.
