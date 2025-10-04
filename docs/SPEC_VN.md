# Thông số kỹ thuật và Thiết kế Tổng quan - Máy CNC 3 Trục

**Dự án:** Máy CNC 3 trục gia công gỗ (mở rộng 4-5 trục)  
**Kích thước làm việc:** ~500mm × 500mm × 80-120mm (X × Y × Z)  
**Động cơ:** NEMA 23 stepper motors  
**Kiến trúc điều khiển:** Raspberry Pi (host) + MCU (STM32/ESP32) real-time controller

---

## 1. MỤC TIÊU VÀ PHẠM VI

### 1.1. Ứng dụng
- Gia công gỗ: cắt, khắc 2.5D, tạo bảng signage, chi tiết nhỏ
- Vật liệu chính: gỗ thông, gỗ sồi, gỗ xoan đào, MDF
- Độ chính xác mục tiêu: ±0.05-0.1mm

### 1.2. Khung làm việc
- **Trục X:** ~500mm (hành trình hữu dụng)
- **Trục Y:** ~500mm (hành trình hữu dụng)
- **Trục Z:** 80-120mm (tuỳ cấu hình)
- Tổng kích thước khung: ~800mm × 800mm × 600mm (bao gồm cấu trúc)

### 1.3. Nâng cấp tương lai
- **Trục A (rotary axis):** Khắc trụ tròn, tiện đơn giản
- **Trục B hoặc C:** Tilt/rotary cho gia công 4.5D hoặc indexing
- **Chế độ:** Indexing (dừng rồi gia công) hoặc simultaneous 5-axis (nâng cao)

### 1.4. Kiến trúc điều khiển
- **Host Controller:** Raspberry Pi 4
  - Xử lý cao cấp: nhận G-code, preview, quản lý job
  - Giao diện: Web UI, network access
  - Streaming G-code xuống MCU
- **Real-time Controller:** MCU (STM32 F4/F7 hoặc ESP32)
  - Parser G-code
  - Motion planner (acceleration, look-ahead)
  - Step pulse generator (low jitter)
  - I/O control: spindle, coolant, limits, probe

---

## 2. CẤU TRÚC CƠ KHÍ

### 2.1. Khung và Vật liệu

#### Khung chính
- **Nhôm định hình:** 2040, 2060, hoặc 2080 (tùy độ cứng yêu cầu)
- **Thép hộp:** Tùy chọn cho phần chịu lực nặng
- **Giằng chéo:** Tăng độ cứng, giảm rung

#### Bàn máy
- **Spoiler board:** MDF dày 18-25mm (thay thế được)
- **Bàn nhôm:** Tùy chọn cho độ phẳng cao
- **Hệ thống kẹp:** T-slot clamps, vacuum table (tùy chọn)

#### Gantry (cổng trục)
- **Vật liệu:** Tấm nhôm dày 8-12mm hoặc 2060 extrusion
- **Cấu trúc:** Chữ H hoặc chữ C
- **Yêu cầu:** Cứng vững, song song với bàn máy

### 2.2. Truyền động

#### Lựa chọn 1: Vitme (Ballscrew)
- **Loại:** SFU1204 hoặc SFU1605
- **Bước:** 5mm hoặc 10mm
- **Ưu điểm:** Độ chính xác cao, lực lớn, backlash thấp
- **Nhược điểm:** Giá cao hơn, cần bôi trơn

#### Lựa chọn 2: Dây đai (Belt Drive)
- **Loại:** GT2 (2mm pitch) hoặc GT3 (3mm pitch)
- **Ưu điểm:** Tốc độ cao, êm, giá rẻ
- **Nhược điểm:** Độ cứng thấp hơn, có thể dãn theo thời gian

#### Khuyến nghị
- **Trục X, Y:** Vitme hoặc dây đai (tùy ngân sách)
- **Trục Z:** Vitme (bắt buộc) để tránh trượt và đảm bảo độ chính xác

### 2.3. Dẫn hướng

#### Ray trượt tuyến tính
- **MGN12 hoặc MGN15:** Độ chính xác cao, preload điều chỉnh được
- **MGN15H:** Cho tải trọng nặng hơn
- **Thanh trượt tròn:** Giá rẻ, phù hợp cho ngân sách thấp

#### Yêu cầu lắp đặt
- Song song tuyệt đối với nhau
- Preload hợp lý (không quá lỏng, không quá chặt)
- Độ phẳng bề mặt gá < 0.05mm

### 2.4. Spindle / Trục chính

#### Thông số kỹ thuật
- **Công suất:** 0.8-1.5 kW (1-2 HP)
- **Tốc độ:** 8,000-24,000 RPM
- **Collet:** ER11 (max 7mm) hoặc ER16 (max 10mm)
- **Làm mát:** Air-cooled hoặc water-cooled

#### Điều khiển
- **PWM:** 0-10V analog (cần module chuyển đổi)
- **Modbus RTU:** RS485 communication (ổn định hơn, có phản hồi tốc độ)
- **Relay:** On/Off đơn giản

#### Lưu ý
- Gỗ thường chạy RPM cao (>18,000) để giảm xé sợi
- Cần tính chip load: feed_per_tooth × số me × RPM

---

## 3. HỆ THỐNG ĐIỆN VÀ ĐIỀU KHIỂN

### 3.1. Động cơ bước (Stepper Motors)

#### NEMA 23
- **Holding torque:** 2.2-3.0 N·m
- **Dòng điện:** 2.8-3.0A per phase
- **Góc bước:** 1.8° (200 steps/rev)
- **Kết nối:** 4-wire hoặc 6-wire (bipolar)

#### Lưu ý quan trọng
- Moment giảm theo tốc độ → cần điện áp cao hơn (48V)
- Microstepping 1/8 đến 1/16 là tối ưu cho CNC gỗ
- Tránh 1/32 trở lên nếu MCU không đủ tốc độ

### 3.2. Driver Stepper

#### Các lựa chọn

**DM542 / TB6600 (Cơ bản)**
- Giá rẻ, phổ biến
- Microstepping: 1/2 đến 1/16
- Điện áp: 18-50VDC
- Dòng: 1-4.2A

**Leadshine (Ổn định)**
- Chất lượng tốt hơn
- Anti-resonance
- Điện áp: 20-50VDC

**TMC5160 (Cao cấp)**
- StealthChop2 (êm)
- StallGuard (phát hiện missed steps)
- SPI interface
- Microstepping lên đến 1/256

#### Cấu hình khuyến nghị
- Microstepping: 1/8 hoặc 1/16
- Dòng: 80% của rated current
- Decay mode: Mixed hoặc Fast (tùy driver)

### 3.3. Nguồn điện (Power Supply)

#### Nguồn chính (Stepper)
- **Điện áp:** 48VDC (khuyến nghị) hoặc 36VDC
- **Dòng:** 10-15A (3-4 motors × 3A + reserve)
- **Loại:** Switching PSU (Mean Well, TDK-Lambda)

#### Nguồn phụ
- **24VDC:** Relay, quạt, solenoid, LED (2-5A)
- **5VDC:** Logic, Raspberry Pi, Arduino (5A)
- **3.3VDC:** MCU logic (1-2A)

#### An toàn nguồn
- Fuse/circuit breaker cho mỗi rail
- EMI filter cho VFD
- Star ground (một điểm mass chung)
- Tách riêng digital ground và power ground nếu cần

### 3.4. Cảm biến và I/O

#### Limit Switches (Công tắc hành trình)
- **Số lượng:** 2 per axis (6 total) hoặc 1 per axis + soft limit
- **Loại:** Mechanical (micro switch) hoặc optical
- **Kết nối:** Normally Closed (NC) cho fail-safe
- **Debounce:** Software hoặc RC filter

#### Homing Switches
- Có thể dùng chung với limit switches
- Độ lặp lại: < 0.01mm
- Repeatability switch hoặc inductive probe

#### Z-Probe (Touch Plate)
- **Công dụng:** Auto zero trục Z
- **Loại:** Touchplate (đơn giản) hoặc 3D probe (nâng cao)
- **Input:** Pullup resistor + debounce

#### Emergency Stop (E-Stop)
- **Loại:** Nút bấm NC, mushroom head
- **Hành động:** Ngắt nguồn driver + disable spindle (mạch cứng)
- **Vị trí:** Dễ tiếp cận, rõ ràng

#### Spindle Control
- **PWM:** 0-100% duty cycle → 0-10V (qua optocoupler + DAC)
- **Modbus:** RS485 RTU protocol
- **Feedback:** Actual RPM từ VFD (nếu có)

### 3.5. Giao tiếp giữa Raspberry Pi và MCU

#### Phương án 1: USB Serial (GRBL-style)
- **Ưu điểm:** Đơn giản, tương thích nhiều firmware
- **Nhược điểm:** Latency cao hơn
- **Giao thức:** ASCII G-code lines
- **Baudrate:** 115200 hoặc 230400

#### Phương án 2: UART
- **Ưu điểm:** Direct connection, ít overhead
- **Nhược điểm:** Cần level shifter (3.3V ↔ 5V nếu cần)
- **Giao thức:** Custom binary protocol + CRC

#### Phương án 3: SPI
- **Ưu điểm:** Tốc độ cao (MHz), đồng bộ
- **Nhược điểm:** Phức tạp hơn, cần xử lý master/slave
- **Ứng dụng:** Streaming lệnh realtime

#### Phương án 4: CAN Bus
- **Ưu điểm:** Ổn định nhiễu, multi-node, dây dài
- **Nhược điểm:** Cần CAN transceiver
- **Ứng dụng:** Hệ thống lớn, nhiều MCU

#### Khuyến nghị
- **MVP:** USB Serial (dễ nhất)
- **Production:** UART hoặc SPI (hiệu suất cao)
- **Industrial:** CAN Bus (độ tin cậy cao)

---

## 4. KIẾN TRÚC PHẦN MỀM

### 4.1. Kiến trúc phân lớp (Layered Architecture)

```
┌──────────────────────────────────────────────────────┐
│           APPLICATION LAYER (Raspberry Pi)           │
├──────────────────────────────────────────────────────┤
│ • Web UI (CNCjs / Custom Node.js / Flask)           │
│ • G-code Preview (3D visualization)                  │
│ • Job Management (queue, history, macros)            │
│ • File Management (upload, organize, CAM)            │
│ • User Authentication & Settings                     │
└─────────────────┬────────────────────────────────────┘
                  │ USB / UART / SPI / CAN
┌─────────────────▼────────────────────────────────────┐
│        FIRMWARE LAYER (STM32 / ESP32 MCU)            │
├──────────────────────────────────────────────────────┤
│ • G-code Parser (modal state machine)                │
│ • Motion Planner (look-ahead, acceleration)          │
│ • Step Pulse Generator (timer ISR)                   │
│ • Kinematics (XYZ → joint angles, 4-5 axis)          │
│ • I/O Handler (spindle, coolant, probe)              │
│ • Safety Monitor (limits, E-stop, watchdog)          │
└──────────────────────────────────────────────────────┘
```

### 4.2. Application Layer (Raspberry Pi)

#### Chức năng chính
1. **Web UI:** Dashboard hiển thị trạng thái máy
2. **G-code Management:** Upload, validate, preview
3. **Streaming:** Gửi G-code từng dòng xuống MCU
4. **Job Control:** Start, pause, resume, stop
5. **Macro System:** Custom routines (auto-zero, tool change)
6. **Logging:** Lưu lại lịch sử chạy, lỗi

#### Stack công nghệ khuyến nghị
- **Backend:** Node.js (CNCjs), Python (Flask), Go
- **Frontend:** React, Vue.js, hoặc HTML/CSS/JS đơn giản
- **Database:** SQLite (job history, settings)
- **Communication:** WebSocket cho real-time updates

### 4.3. Firmware Layer (MCU)

#### G-code Parser
- **Chức năng:** Parse ASCII G-code thành internal commands
- **Modal state:** G0/G1, G90/G91, G20/G21, feedrate, spindle speed
- **Error handling:** Invalid commands, out-of-range

#### Motion Planner
- **Look-ahead:** Tối ưu tốc độ tại góc cua
- **Acceleration profile:** Trapezoidal hoặc S-curve
- **Junction deviation:** Tính toán tốc độ tối đa tại junction
- **Buffer:** Ring buffer 16-32 blocks

#### Step Pulse Generator
- **Timer ISR:** Hardware timer (TIM1, TIM2 trên STM32)
- **Frequency:** Lên đến 100kHz per axis
- **STEP/DIR:** Pulse width > 2.5µs (tùy driver)
- **Jitter:** < 5µs (quan trọng cho độ mịn)

#### Kinematics
- **3-axis:** Đơn giản (X, Y, Z cartesian)
- **4-axis:** Thêm trục A (rotary), tính degrees ↔ steps
- **5-axis:** Phức tạp hơn, cần matrix transformation

### 4.4. Lựa chọn Firmware

#### grblHAL (Khuyến nghị cho STM32)
- **Website:** https://github.com/grblHAL
- **Ưu điểm:** Stable, well-documented, STM32 port
- **Nhược điểm:** Cần học API

#### FluidNC (Khuyến nghị cho ESP32)
- **Website:** https://github.com/bdring/FluidNC
- **Ưu điểm:** Web UI built-in, Wi-Fi, config via YAML
- **Nhược điểm:** Jitter cao hơn nếu dùng Wi-Fi đồng thời

#### Custom Firmware
- **Khi nào:** Cần tùy biến sâu, kinematics đặc biệt
- **Base:** Dựa trên grblHAL hoặc g2core skeleton
- **Workload:** Cao (phát triển + debug + maintain)

### 4.5. Mở rộng 4-5 trục

#### Trục A (Rotary - Horizontal)
- **Cấu hình:** steps_per_degree = (steps_per_rev × microstep × gear_ratio) / 360
- **Ví dụ:** 200 × 8 × 10 / 360 = 44.44 steps/degree
- **Indexing:** Dừng quay, sau đó gia công XYZ
- **Simultaneous:** Quay cùng lúc với XYZ (cần interpolation)

#### Trục B (Tilt) hoặc C (Rotary Vertical)
- **Kinematics:** Tool vector transformation
- **Post-processor:** CAM cần hỗ trợ (Fusion 360, SolidCAM)
- **Complexity:** Cao, cần kiểm tra collision

#### Khuyến nghị
- **Bắt đầu:** 3-axis (XYZ)
- **Phase 2:** 4-axis (XYZ + A indexing)
- **Phase 3:** 5-axis (full simultaneous nếu cần)

---

## 5. THÔNG SỐ VẬN HÀNH ỨNG DỤNG (GỖ)

### 5.1. Feed Rate (Tốc độ tiến)
- **Roughing (phay thô):** 1,000-2,000 mm/min
- **Finishing (phay tinh):** 500-1,500 mm/min
- **Engraving (khắc):** 300-800 mm/min
- **Rapids (di chuyển nhanh):** 3,000-5,000 mm/min

### 5.2. Spindle Speed (Tốc độ trục chính)
- **Gỗ cứng (sồi, gỗ hương):** 18,000-24,000 RPM
- **Gỗ mềm (thông, MDF):** 15,000-20,000 RPM
- **Khắc chi tiết nhỏ:** 20,000-24,000 RPM

### 5.3. Depth of Cut (Độ sâu cắt)
- **Step down (một lần xuống):** 1-3mm (tùy độ cứng gỗ)
- **Full depth:** 2-10mm (tùy công suất spindle và dao)
- **Adaptive clearing:** Tối ưu hơn (CAM support)

### 5.4. Chip Load
- **Công thức:** Chip_load = Feed_rate / (RPM × num_flutes)
- **Gỗ mềm:** 0.1-0.2 mm/flute
- **Gỗ cứng:** 0.05-0.15 mm/flute
- **Ví dụ:** Feed 1,200 mm/min, RPM 18,000, 2 flutes → Chip load = 1200/(18000×2) = 0.033 mm/flute → OK

### 5.5. Step Rate (Tốc độ xung bước)

#### Tính toán ví dụ
**Giả sử:**
- Vitme bước 5mm (SFU1204)
- Motor 200 steps/rev
- Microstepping 1/8
- Feed rate 3,000 mm/min = 50 mm/s

**Steps per mm:**
```
steps_per_mm = (200 × 8) / 5 = 320 steps/mm
```

**Step rate:**
```
step_rate = 50 mm/s × 320 steps/mm = 16,000 steps/s = 16 kHz
```

**3 trục đồng thời:**
```
total_step_rate ≈ 16 kHz × √3 ≈ 28 kHz (diagonal move)
```

**Kết luận:** STM32 F4 (168 MHz) hoặc ESP32 (240 MHz) đều đủ mạnh.

---

## 6. CÔNG THỨC VÀ TÍNH TOÁN CƠ BẢN

### 6.1. Steps per mm (Vitme)
```
steps_per_mm = (steps_per_rev × microsteps) / pitch_mm
```
**Ví dụ:**
- 200 steps/rev × 16 microsteps / 10mm pitch = 320 steps/mm

### 6.2. Steps per degree (Rotary)
```
steps_per_degree = (steps_per_rev × microsteps × gear_ratio) / 360
```
**Ví dụ:**
- 200 × 8 × 10 / 360 = 44.44 steps/degree

### 6.3. Max Feedrate
```
max_feedrate_mm_per_min = (max_step_rate / steps_per_mm) × 60
```
**Ví dụ:**
- Max step rate 50,000 Hz, steps/mm 320 → Max feed = 9,375 mm/min

### 6.4. Acceleration
```
acceleration_mm_per_s2 = 300 - 1000 (typical for wood CNC)
```
- Gỗ: 500-800 mm/s² (cân bằng tốc độ và độ chính xác)
- Test và tune dựa trên rung động

### 6.5. Chip Load
```
chip_load = feed_rate_mm_per_min / (RPM × num_flutes)
```
**Ví dụ:**
- Feed 1,500 mm/min, RPM 18,000, 2 flutes → 0.042 mm/flute

### 6.6. Cutting Power
```
cutting_power_W ≈ MRR × specific_cutting_energy
```
- MRR (Material Removal Rate): depth × width × feed
- Specific cutting energy (gỗ): ~0.5-1.5 W/(mm³/s)

---

## 7. BẢO VỆ VÀ AN TOÀN

### 7.1. Emergency Stop (E-Stop)
- **Nút bấm:** NC mushroom button, đỏ, lớn
- **Mạch:** Ngắt nguồn driver + disable spindle (relay)
- **Firmware:** Detect E-Stop signal, halt motion immediately
- **Reset:** Cần xoay hoặc pull để unlock

### 7.2. Limit Switches
- **Vị trí:** Đầu và cuối mỗi trục (hoặc chỉ một đầu + soft limit)
- **Kết nối:** NC (Normally Closed) cho fail-safe
- **Firmware:** Instant stop, alarm state
- **Debounce:** 10-50ms software filter

### 7.3. Soft Limits
- **Firmware:** Kiểm tra tọa độ trước khi move
- **Cấu hình:** $130, $131, $132 (GRBL)
- **Hành động:** Reject G-code move nếu vượt quá

### 7.4. Spindle Safety
- **Delay:** Chờ spindle lên tốc trước khi cắt (1-3 giây)
- **RPM Monitor:** Kiểm tra actual RPM (nếu có feedback)
- **Thermal:** Spindle overload protection (current sense)

### 7.5. Dust Collection
- **Công dụng:** Giảm bụi gỗ → tốt cho sức khỏe và thiết bị
- **Hệ thống:** Shop vac + dust shoe tại spindle
- **Auto On/Off:** Relay điều khiển bởi MCU (bật cùng spindle)

### 7.6. Enclosure
- **Mục đích:** Giảm tiếng ồn, chắn bụi, an toàn
- **Vật liệu:** Acrylic, polycarbonate, hoặc gỗ + sound foam
- **Cửa:** Có khóa liên động (open → disable spindle)

### 7.7. Electrical Safety
- **Grounding:** Khung máy nối mass (earth ground)
- **Fuse:** Mỗi power rail có fuse phù hợp
- **GFCI:** Ground Fault Circuit Interrupter nếu có nước gần
- **Cable management:** Drag chain, tránh dây chà vào cơ khí

---

## 8. QUẢN LÝ NHIỄU (EMI) VÀ ỔN ĐỊNH

### 8.1. Nguồn nhiễu chính
- **VFD / Spindle:** Tần số cao, switching noise
- **Stepper driver:** PWM, switching
- **Relay / Contactor:** Spark khi đóng/mở

### 8.2. Giải pháp giảm nhiễu

#### Cáp Spindle
- **Shield:** Dây có lưới bện (braided shield)
- **Grounding:** Shield nối mass tại một đầu (star ground)
- **Separation:** Cách xa tín hiệu step/dir ≥10-15cm

#### Step/Dir Signal
- **Cable:** Twisted pair hoặc shielded
- **Length:** Ngắn nhất có thể (< 1-2m)
- **Pull-up/Pull-down:** Đảm bảo tín hiệu rõ ràng

#### VFD
- **EMI Filter:** Lắp trên input AC
- **Ferrite Choke:** Clamp trên dây output VFD
- **Grounding:** VFD, motor, frame cùng một mass

#### Power Supply
- **Filter capacitor:** Thêm cap lớn (1000-4700µF) tại output
- **Common mode choke:** Giảm noise lan truyền
- **Star ground:** Tất cả mass về một điểm chung

### 8.3. Cách bố trí cáp
- **Power cables:** Riêng một bên, xa signal
- **Signal cables:** Bên kia, twisted pair
- **Drag chain:** Phân tách ngăn trong (power / signal)
- **Avoid loops:** Ground loop gây nhiễu → dùng star topology

---

## 9. LỘ TRÌNH PHÁT TRIỂN

### Giai đoạn 1: MVP 3 Trục (2-3 tháng)
- [ ] Thiết kế cơ khí: khung, vitme, ray, gantry
- [ ] Lắp ráp cơ khí: cân chỉnh vuông góc, song song
- [ ] Điện: driver, nguồn, wiring, E-stop
- [ ] Firmware: Flash grblHAL hoặc GRBL
- [ ] Cấu hình: steps/mm, acceleration, max rate
- [ ] Homing: Thiết lập limit switches
- [ ] Test chạy: Rapids, straight lines, circles
- [ ] First cut: Thử cắt gỗ MDF

### Giai đoạn 2: Tối ưu (1-2 tháng)
- [ ] Hiệu chỉnh: steps/mm chính xác (test 100mm move)
- [ ] Backlash compensation (nếu cần)
- [ ] Tune acceleration và junction deviation
- [ ] Z-probe: Auto tool zero
- [ ] Macro: Custom routines (surfacing, tool change)
- [ ] Logging: Raspberry Pi ghi lại metrics
- [ ] Dust collection: Tích hợp hệ thống hút bụi

### Giai đoạn 3: Mở rộng 4 trục (2-3 tháng)
- [ ] Thiết kế trục A (rotary): khung, chuck, tailstock
- [ ] Lắp ráp và cân chỉnh
- [ ] Firmware: Cấu hình 4-axis
- [ ] Kinematics: Indexing mode
- [ ] CAM: Fusion 360 4-axis post-processor
- [ ] Test: Khắc trụ tròn, ống gỗ

### Giai đoạn 4: Nâng cao 5 trục (3-6 tháng)
- [ ] Thiết kế trục B hoặc C: tilt mechanism
- [ ] Lắp ráp phức tạp, alignment critical
- [ ] Firmware: Full 5-axis kinematics
- [ ] Collision detection (software hoặc sensor)
- [ ] CAM: 5-axis simultaneous toolpath
- [ ] Test: Gia công hình phức tạp

### Giai đoạn 5: Production Ready (ongoing)
- [ ] Enclosure: Giảm tiếng ồn và bụi
- [ ] Automatic tool changer (ATC) - tùy chọn
- [ ] Tool length sensor
- [ ] Vision system: Camera alignment
- [ ] Remote monitoring: Web dashboard
- [ ] Maintenance schedule: Greasing, calibration

---

## 10. DANH SÁCH THÀNH PHẦN (BOM - Bill of Materials)

### 10.1. Cơ khí

| Thành phần | Số lượng | Thông số | Ghi chú |
|------------|----------|----------|---------|
| Nhôm định hình 2060 | 10m | 6 slots | Khung chính |
| Vitme SFU1605 | 3 × 600mm | C7 hoặc C5 | X, Y, Z |
| Đai ốc vitme | 3 | Flange nut + BK/BF | Anti-backlash |
| Ray MGN15 | 2 × 600mm | + 4 blocks | Trục X, Y |
| Ray MGN12 | 1 × 200mm | + 2 blocks | Trục Z |
| Coupler | 3 | 6.35mm-8mm | Motor → Ballscrew |
| Pulley/belt (nếu dùng) | Set | GT2 | Thay vì vitme |
| Spoiler board MDF | 1 | 500×500×18mm | Thay thế được |
| Tấm nhôm gá Z | 1 | 200×150×10mm | Gantry |
| L-bracket, T-nut | Various | M5 | Lắp ráp khung |

### 10.2. Động lực

| Thành phần | Số lượng | Thông số | Ghi chú |
|------------|----------|----------|---------|
| NEMA 23 stepper | 3-4 | 2.8A, 2.2N·m | X, Y, Z (+ A) |
| Driver TB6600 | 3-4 | 4.2A max | Hoặc DM542 |
| Nguồn 48VDC | 1 | 10-15A, 500W+ | Switching PSU |
| Nguồn 24VDC | 1 | 5A (phụ) | Relay, fan, LED |
| Nguồn 5VDC | 1 | 5A | Raspberry Pi + logic |

### 10.3. Điều khiển

| Thành phần | Số lượng | Thông số | Ghi chú |
|------------|----------|----------|---------|
| Raspberry Pi 4 | 1 | 4GB RAM | Host controller |
| STM32F407 board | 1 | 168MHz | Hoặc ESP32 |
| USB cable | 1 | Type A-B hoặc C | Pi ↔ MCU |
| Breakout board | 1 | Step/Dir output | CNC shield |

### 10.4. Spindle và công cụ

| Thành phần | Số lượng | Thông số | Ghi chú |
|------------|----------|----------|---------|
| Spindle | 1 | 1.5kW, ER11 | Air-cooled |
| VFD | 1 | 1.5-2.2kW | Modbus hoặc PWM |
| Collet ER11 | Set | 1-7mm | Nhiều kích thước |
| End mills | Set | 2-flute, carbide | Gỗ |
| V-bit | 2-3 | 60°, 90° | Engraving |

### 10.5. Cảm biến và I/O

| Thành phần | Số lượng | Thông số | Ghi chú |
|------------|----------|----------|---------|
| Limit switch | 6-9 | NC micro switch | Hoặc 3 nếu dùng soft limit |
| Z-probe touch plate | 1 | Aluminum plate | Auto zero |
| E-Stop button | 1 | NC mushroom, red | Safety |
| Feed hold button | 1 | NO momentary | Pause |
| Relay 24VDC | 2-3 | 10A | Spindle, vacuum |

### 10.6. Dây và phụ kiện

| Thành phần | Số lượng | Thông số | Ghi chú |
|------------|----------|----------|---------|
| Cáp stepper 4-wire | 10m | Shielded | 2.5mm² |
| Cáp spindle shielded | 3m | 3-phase | 1.5mm² |
| Cáp signal | 10m | Twisted pair | 22-24 AWG |
| Drag chain | 3m | 15×15mm | X, Y, Z |
| Terminal block | 10 | 10A | Wiring hub |
| Fuse holder | 5 | 5-20A | Safety |
| Heat shrink tubing | Set | Various | Insulation |
| Cable ties | 100 | 200mm | Cable management |

### 10.7. Tủ điện

| Thành phần | Số lượng | Thông số | Ghi chú |
|------------|----------|----------|---------|
| Enclosure box | 1 | 400×300×200mm | IP54 hoặc IP65 |
| Din rail | 1m | 35mm | Mount drivers |
| Fan 24VDC | 2 | 120mm | Cooling |
| LED indicator | 3-5 | R, G, B | Status |
| Contactor | 1 | 25A | E-stop cutoff |

### 10.8. Ước tính chi phí (USD)

| Hạng mục | Chi phí (USD) |
|----------|---------------|
| Cơ khí (khung, vitme, ray) | $400-600 |
| Động lực (motor, driver, PSU) | $250-400 |
| Spindle + VFD | $200-350 |
| Điều khiển (Pi, MCU, board) | $100-150 |
| Cảm biến và I/O | $50-80 |
| Dây, phụ kiện | $80-120 |
| Tủ điện | $50-80 |
| **Tổng** | **$1,130-1,780** |

*Lưu ý: Giá có thể thay đổi tùy nhà cung cấp và quốc gia.*

---

## 11. SO SÁNH STM32 VS ESP32

| Tiêu chí | STM32 (F4/F7) | ESP32 |
|----------|---------------|-------|
| **Clock speed** | 168-216 MHz | 240 MHz (dual-core) |
| **Timers** | Hardware timers nhiều | Ít timers hardware hơn |
| **Real-time** | Tốt (low jitter) | Khá (jitter cao hơn nếu dùng Wi-Fi) |
| **Wi-Fi/Bluetooth** | Không (cần module) | Tích hợp sẵn |
| **Ethernet** | Có (F4/F7) hoặc cần module | Cần module (ETH PHY) |
| **USB** | Full-speed USB | USB (serial/JTAG) |
| **GPIO** | 50-100+ pins | 34 GPIO |
| **Development** | STM32CubeIDE, ARM GCC | Arduino IDE, ESP-IDF |
| **grblHAL support** | Excellent | Good (FluidNC better) |
| **Price** | $5-15 | $3-8 |
| **Khi nào chọn STM32** | Ưu tiên độ chính xác, jitter thấp | |
| **Khi nào chọn ESP32** | Cần Wi-Fi nhanh, web UI tích hợp | |

### Khuyến nghị
- **STM32F407:** Best choice cho production CNC (stable, low jitter)
- **ESP32:** Good cho prototyping, web UI, home use (tắt Wi-Fi khi chạy tốc độ cao)

---

## 12. GỢI Ý TÀI LIỆU VÀ PHẦN MỀM THAM KHẢO

### 12.1. Firmware

**grblHAL**
- Repository: https://github.com/grblHAL/core
- STM32 port: https://github.com/grblHAL/STM32F4xx
- Docs: https://github.com/grblHAL/core/wiki

**FluidNC**
- Repository: https://github.com/bdring/FluidNC
- Docs: http://wiki.fluidnc.com/

**LinuxCNC**
- Website: http://linuxcnc.org/
- Cho hệ thống PC-based (không cần MCU riêng)

### 12.2. Host Software

**CNCjs**
- Website: https://cnc.js.org/
- Node.js, Web UI, works với GRBL
- Tốt cho Raspberry Pi

**OpenBuilds CONTROL**
- Website: https://software.openbuilds.com/
- Desktop app (Windows, Mac, Linux)
- Wizard-based, beginner friendly

**bCNC**
- Website: https://github.com/vlachoudis/bCNC
- Python, cross-platform
- Advanced features (probing, leveling)

**Universal Gcode Sender (UGS)**
- Website: https://winder.github.io/ugs_website/
- Java, cross-platform
- Simple và stable

### 12.3. CAM Software

**Fusion 360**
- Website: https://www.autodesk.com/products/fusion-360
- Free for hobby/startup
- Full CAD/CAM/CAE

**FreeCAD Path**
- Website: https://www.freecad.org/
- Open-source, free
- Decent cho 2.5D-3D

**VCarve / Aspire**
- Website: https://www.vectric.com/
- Tốt cho signage, 3D carving
- Có bản Desktop và Pro

**Estlcam**
- Website: https://www.estlcam.de/
- Giá rẻ (~$60), dễ dùng
- Tốt cho 2.5D, PCB

### 12.4. Sách và khóa học

**CNC Programming Handbook**
- Author: Peter Smid
- Toàn diện về G-code và CNC

**Make: CNC**
- Practical guide cho CNC tự làm

**YouTube Channels**
- Winston Moy (CNC fundamentals)
- NYC CNC (milling operations)
- Chris's Basement (CNC builds)

---

## 13. RỦI RO VÀ GIẢI PHÁP

| Rủi ro | Nguyên nhân | Triệu chứng | Giải pháp |
|--------|-------------|-------------|-----------|
| **Mất bước (missed steps)** | Gia tốc quá cao, lực cắt lớn, điện áp thấp | Vị trí không chính xác, shift | Giảm accel, tăng voltage driver (48V), dao sắc hơn |
| **Cháy dao** | Feed quá thấp, RPM quá cao (ma sát) | Dao nóng, sắc cạnh cháy đen | Tối ưu chip load: tăng feed hoặc giảm RPM |
| **Nhiễu limit switch** | Dây song song với spindle, không shield | False trigger, alarm | Dây twisted pair + shield, debounce firmware |
| **Buffer underrun** | Host truyền chậm, MCU buffer nhỏ | Jerky motion, pausing | Tăng buffer size, dùng flow control (XON/XOFF) |
| **Rung (chatter)** | Khung thiếu cứng, RPM resonance | Bề mặt xấu, tiếng ồn | Thêm giằng chéo, giảm overhang Z, đổi RPM |
| **Quá nhiệt driver** | Tản nhiệt kém, dòng quá cao | Driver tắt hoặc hỏng | Heatsink lớn hơn + quạt, giảm current 80% rated |
| **Backlash** | Đai ốc vitme lỏng, dây đai dãn | Vị trí không lặp lại | Thay đai ốc anti-backlash, căng đai, tune firmware |
| **Spindle không lên tốc** | PWM không đúng, VFD config sai | Spindle quay chậm hoặc không quay | Check PWM frequency (thường 1kHz), VFD params |
| **Z-probe lỗi** | Dây nhiễu, pullup sai | Auto-zero sai, crash dao | Dây shield, 10k pullup, debounce |
| **E-stop không hoạt động** | Dây NC lỏng, firmware không check | Máy không dừng khi nhấn | Test định kỳ, NC switch + hardware cutoff |

---

## 14. TÓM TẮT NGẮN GỌN

Máy CNC gỗ **500×500mm 3 trục** (mở rộng 4-5 trục) sử dụng:
- **Động cơ:** NEMA 23 stepper motors
- **Truyền động:** Vitme SFU1605 hoặc dây đai GT2
- **Spindle:** 1-1.5 kW, ER11, 8,000-24,000 RPM
- **Kiến trúc:** Raspberry Pi (host, UI, G-code) + STM32/ESP32 (real-time step generation)
- **Firmware:** grblHAL (STM32) hoặc FluidNC (ESP32)
- **Giao tiếp:** USB Serial / UART / SPI / CAN Bus
- **Mở rộng:** Trục A (rotary) dễ dàng, 5-axis cần kinematics phức tạp
- **Thiết kế:** Cứng vững khung, quản lý EMI tốt, an toàn E-Stop, kiến trúc phần mềm phân lớp

**Ưu tiên:** Ổn định → Tốc độ → Tính năng nâng cao

---

## 15. BƯỚC TIẾP THEO - HỖ TRỢ CHI TIẾT

Tôi có thể hỗ trợ thêm:
1. **Sơ đồ kết nối điện chi tiết** (pin-level wiring diagram)
2. **So sánh vitme vs dây đai** với bảng ưu/nhược điểm
3. **Hướng dẫn cài đặt grblHAL** trên STM32 + Raspberry Pi streaming
4. **Tính chip load cụ thể** cho dao 2-flute cắt gỗ với ví dụ thực tế
5. **Thiết kế cơ khí CAD** (frame, gantry, Z-axis assembly)
6. **Firmware skeleton code** (parser, planner, ISR)
7. **Web UI mockup** cho dashboard
8. **CAM workflow** từ Fusion 360 → G-code → CNC

**Câu hỏi cho bạn:**
- Bạn muốn đi sâu phần nào tiếp theo?
- Ngân sách dự kiến là bao nhiêu?
- Ưu tiên tốc độ hay độ chính xác?
- Đã có kinh nghiệm lập trình MCU chưa?

---

**Tài liệu này là nền tảng để xây dựng máy CNC 3 trục gia công gỗ chuyên nghiệp với khả năng mở rộng lên 4-5 trục. Mọi phần đều có thể tùy chỉnh theo nhu cầu và ngân sách cụ thể.**

**Phiên bản:** 1.0  
**Cập nhật:** 2024  
**License:** MIT / Open Source
