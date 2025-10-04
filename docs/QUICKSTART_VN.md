# Hướng dẫn Nhanh - CNC 3 Trục

**Quick Start Guide - Máy CNC gia công gỗ 500×500mm**

---

## Tài liệu Tham khảo Nhanh

### Tài liệu Chính
1. **[SPEC_VN.md](SPEC_VN.md)** - Thông số kỹ thuật đầy đủ (15 phần)
2. **[ARCHITECTURE.md](ARCHITECTURE.md)** - Kiến trúc hệ thống (Pi + MCU)
3. **[hardware/BOM.md](hardware/BOM.md)** - Danh sách linh kiện
4. **[SAFETY_EMI.md](SAFETY_EMI.md)** - An toàn và quản lý nhiễu
5. **[ROADMAP.md](ROADMAP.md)** - Lộ trình phát triển

---

## Bước 1: Mua Linh Kiện

### Linh kiện Chính (Tối thiểu)

#### Cơ khí ($400-600)
- ✅ Nhôm định hình 2060: 10m
- ✅ Vitme SFU1605: 3 cây (X, Y, Z)
- ✅ Ray MGN15: 2 cây (X, Y)
- ✅ Ray MGN12: 1 cây (Z)
- ✅ Spoilboard MDF: 500×500mm

#### Động cơ & Driver ($250-400)
- ✅ NEMA 23 motor: 3 hoặc 4 cái
- ✅ Driver TB6600 hoặc DM542: 3-4 cái
- ✅ Nguồn 48VDC 10A: 1 cái
- ✅ Nguồn 24VDC 5A: 1 cái (phụ)

#### Spindle ($200-350)
- ✅ Spindle 1.5kW ER11: 1 cái
- ✅ VFD 1.5kW: 1 cái
- ✅ Bộ collet ER11: 1 set

#### Điều khiển ($100-150)
- ✅ Raspberry Pi 4 (4GB): 1 cái
- ✅ STM32F407 board: 1 cái (hoặc ESP32)
- ✅ Dây USB: 1 cái

#### Cảm biến ($50-80)
- ✅ Công tắc hành trình: 6 cái (NC)
- ✅ Nút E-Stop: 1 cái (đỏ)
- ✅ Touch plate Z-probe: 1 cái

**Xem chi tiết:** [hardware/BOM.md](hardware/BOM.md)

---

## Bước 2: Lắp Ráp Cơ Khí

### 2.1. Khung chính (1-2 tuần)

```
1. Cắt nhôm định hình theo kích thước
2. Lắp khung với L-bracket
3. Kiểm tra vuông góc (đường chéo bằng nhau)
4. Thêm giằng chéo để tăng độ cứng
```

### 2.2. Lắp ray và vitme (1-2 tuần)

```
1. Gá ray MGN15 lên khung (X, Y)
   - Phải song song (kiểm tra bằng thước đo)
   - Khoảng cách đều nhau

2. Lắp vitme SFU1605
   - Dùng BK/BF bearing block
   - Coupler nối motor với vitme

3. Lắp Z-axis
   - Ray MGN12
   - Gá spindle lên Z
```

### 2.3. Kiểm tra chuyển động

```
✓ Đẩy gantry bằng tay → phải trơn, không kẹt
✓ Kiểm tra vuông góc X-Y với thước góc
✓ Đo độ song song ray với thước panme
✓ Preload block: không quá lỏng, không quá chặt
```

**Chi tiết:** [SPEC_VN.md - Phần 2](SPEC_VN.md#2-cấu-trúc-cơ-khí)

---

## Bước 3: Đấu Điện

### 3.1. Nguồn điện

```
48VDC ──┬──> Driver X
        ├──> Driver Y
        └──> Driver Z

24VDC ──┬──> Relay
        └──> Quạt

5VDC  ────> Raspberry Pi, MCU
```

### 3.2. Đấu motor và driver

```
Motor ──> Driver (A+, A-, B+, B-)
MCU   ──> Driver (STEP, DIR, EN)

Cấu hình driver:
- Microstepping: 1/8 hoặc 1/16
- Dòng điện: 80% của motor rated current
```

### 3.3. Cảm biến

```
Limit Switch (NC) ──> MCU input (pullup)
Z-Probe          ──> MCU input (pullup)
E-Stop (NC)      ──> Ngắt nguồn driver (hardware)
```

### 3.4. Spindle

```
VFD ──> Spindle (3 phase)
MCU ──> VFD (PWM hoặc Modbus)
```

**An toàn:** [SAFETY_EMI.md](SAFETY_EMI.md)

---

## Bước 4: Cài Đặt Firmware

### Lựa chọn 1: STM32 + grblHAL

```bash
# Cài toolchain
sudo apt install gcc-arm-none-eabi stlink-tools

# Clone firmware
git clone https://github.com/grblHAL/STM32F4xx.git
cd STM32F4xx

# Cấu hình pin trong Inc/my_machine.h
# Build
make

# Flash
st-flash write build/grblHAL.bin 0x8000000
```

### Lựa chọn 2: ESP32 + FluidNC

```bash
# Cài PlatformIO
pip install platformio

# Clone firmware
git clone https://github.com/bdring/FluidNC.git
cd FluidNC

# Build và flash
pio run -e esp32 -t upload

# Truy cập WebUI: http://fluidnc.local
```

**Chi tiết:** [../ENV_SETUP.md](../ENV_SETUP.md)

---

## Bước 5: Cài Đặt Raspberry Pi

### 5.1. Cài OS

```bash
# Flash Raspberry Pi OS (64-bit) lên SD card
# Boot và cấu hình cơ bản (Wi-Fi, SSH)
```

### 5.2. Cài CNCjs

```bash
# Cài Node.js
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt install -y nodejs

# Cài CNCjs
sudo npm install -g cncjs

# Chạy
cncjs
```

### 5.3. Truy cập Web UI

```
Mở trình duyệt: http://raspberrypi.local:8000
```

### 5.4. Kết nối với MCU

```
Serial port: /dev/ttyUSB0 hoặc /dev/ttyACM0
Baud rate: 115200
```

---

## Bước 6: Cấu hình GRBL

### Gửi lệnh qua CNCjs hoặc Terminal:

```gcode
$$                 # Xem tất cả cài đặt

# Cài đặt cơ bản
$100=320           # X steps/mm (tính theo công thức)
$101=320           # Y steps/mm
$102=320           # Z steps/mm

$110=3000          # X max rate (mm/min)
$111=3000          # Y max rate
$112=500           # Z max rate

$120=500           # X acceleration (mm/s²)
$121=500           # Y acceleration
$122=300           # Z acceleration

$130=500           # X max travel (mm)
$131=500           # Y max travel
$132=100           # Z max travel

$22=1              # Homing cycle (bật)
$20=0              # Soft limits (tắt khi test)
$21=1              # Hard limits (bật)
```

### Công thức tính steps/mm:

```
Vitme bước 5mm, motor 200 steps/rev, microstepping 1/8:
steps/mm = (200 × 8) / 5 = 320
```

**Chi tiết:** [SPEC_VN.md - Phần 6](SPEC_VN.md#6-công-thức-và-tính-toán-cơ-bản)

---

## Bước 7: Kiểm Tra & Hiệu Chỉnh

### 7.1. Test từng trục

```gcode
G91          # Relative mode
G0 X10       # Di chuyển X 10mm
G0 Y10       # Di chuyển Y 10mm
G0 Z10       # Di chuyển Z 10mm
```

Kiểm tra:
- ✅ Hướng đúng không? (nếu sai → $3=...)
- ✅ Khoảng cách chính xác không? (đo bằng thước)

### 7.2. Homing

```gcode
$H           # Chạy homing cycle
```

Kiểm tra:
- ✅ Tất cả trục về đúng vị trí home
- ✅ Lặp lại 5 lần → sai số < 0.01mm

### 7.3. Hiệu chỉnh steps/mm

```
1. Lệnh: G0 X100
2. Đo thực tế: 99.5mm
3. Tính lại: $100 = 320 × (100/99.5) = 321.6
4. Cài đặt: $100=321.6
5. Lặp lại cho Y, Z
```

---

## Bước 8: Cắt Thử Đầu Tiên

### 8.1. Chuẩn bị

- ✅ Kẹp phôi gỗ MDF (100×100mm)
- ✅ Lắp dao 6mm end mill
- ✅ Set Z zero (dùng Z-probe hoặc giấy)

### 8.2. G-code đơn giản (hình vuông)

```gcode
G21 G90          ; Metric, Absolute
G0 X10 Y10 Z5    ; Rapid to start
G1 Z-1 F100      ; Plunge
G1 X60 F600      ; Cut X
G1 Y60           ; Cut Y
G1 X10           ; Cut X
G1 Y10           ; Cut Y
G0 Z10           ; Retract
M2               ; End
```

### 8.3. Chạy job

```
1. Upload G-code vào CNCjs
2. Bật spindle (8,000 RPM)
3. Click "Start"
4. Quan sát cẩn thận
5. E-Stop nếu có vấn đề
```

### 8.4. Kiểm tra kết quả

- ✅ Hình vuông có vuông không?
- ✅ Kích thước chính xác không?
- ✅ Bề mặt mịn không?

**Nếu OK → Chúc mừng! Máy CNC của bạn đã hoạt động! 🎉**

---

## Bước Tiếp Theo

### Tối ưu hóa

1. **Tune acceleration và speed**
   - Tăng dần $120, $121, $122
   - Dừng khi bắt đầu bị rung hoặc mất bước
   
2. **Kiểm tra độ chính xác**
   - Cắt hình tròn, test arc
   - Đo với thước panme
   
3. **Thêm tính năng**
   - Z-probe auto zero
   - Dust collection
   - Macro surfacing

### Học CAM

1. **Cài Fusion 360** (miễn phí cho cá nhân)
2. **Học CAM workflow:**
   - Design part → CAM → G-code
3. **Thử các chiến lược:**
   - 2D Pocket
   - 2D Contour
   - 3D Adaptive clearing

### Tham gia cộng đồng

- GitHub Issues: Đặt câu hỏi, báo lỗi
- Chia sẻ project của bạn
- Giúp người khác

---

## Xử Lý Sự Cố Nhanh

| Vấn đề | Nguyên nhân | Giải pháp |
|--------|-------------|-----------|
| Motor không quay | Dây sai, driver chưa cấu hình | Kiểm tra dây A+, A-, B+, B- |
| Quay sai chiều | $3 setting sai | Đảo bit tương ứng trong $3 |
| Mất bước | Accel cao, voltage thấp | Giảm $120-122, dùng 48V |
| Limit switch false trigger | EMI | Dây shield, tăng debounce |
| Spindle không quay | VFD config sai | Check PWM frequency, VFD params |

**Chi tiết:** [SAFETY_EMI.md - Troubleshooting](SAFETY_EMI.md)

---

## Công Thức Hữu Ích

### Steps per mm (Vitme)
```
steps_per_mm = (steps_per_rev × microsteps) / pitch_mm

Ví dụ: (200 × 8) / 5 = 320
```

### Chip Load
```
chip_load = feed_rate / (RPM × num_flutes)

Ví dụ: 1200 / (18000 × 2) = 0.033 mm/flute
```

### Max Feed Rate
```
max_feed = (max_step_rate / steps_per_mm) × 60

Ví dụ: (50000 / 320) × 60 = 9375 mm/min
```

**Chi tiết:** [SPEC_VN.md - Phần 6](SPEC_VN.md#6-công-thức-và-tính-toán-cơ-bản)

---

## An Toàn - Quan Trọng!

### Trước khi chạy máy:

- ✅ Đeo kính bảo hộ
- ✅ Test E-Stop
- ✅ Kẹp phôi chắc chắn
- ✅ Tool đã thắt chặt
- ✅ Preview G-code
- ✅ Dry run (Z raised)

### Trong khi chạy:

- ❌ Không để tay vào vùng cắt
- ❌ Không rời xa máy
- ❌ Không mở cửa khi spindle đang quay
- ✅ Quan sát liên tục
- ✅ Sẵn sàng nhấn E-Stop

**Đọc đầy đủ:** [SAFETY_EMI.md](SAFETY_EMI.md)

---

## Tài Nguyên Hữu Ích

### Firmware
- grblHAL: https://github.com/grblHAL
- FluidNC: https://github.com/bdring/FluidNC

### Software
- CNCjs: https://cnc.js.org/
- Fusion 360: https://www.autodesk.com/products/fusion-360

### Học tập
- NYC CNC (YouTube): Milling operations
- Winston Moy (YouTube): CNC fundamentals
- grblHAL Wiki: https://github.com/grblHAL/core/wiki

### Community
- GitHub Issues: Đặt câu hỏi
- Facebook groups: CNC Vietnam
- Forums: CNCZone, PracticalMachinist

---

## Liên Hệ & Hỗ Trợ

- **GitHub Issues:** https://github.com/Sonfiuss/CNC_3D/issues
- **Email:** [Your email]
- **Documentation:** Xem các file .md trong thư mục docs/

---

**Chúc bạn thành công với máy CNC! 🎉**

**Ghi nhớ:** An toàn là trên hết. Nếu không chắc chắn, hãy dừng lại và hỏi.

---

**Phiên bản:** 1.0  
**Cập nhật:** 2024
