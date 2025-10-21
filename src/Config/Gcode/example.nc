gcode
// filepath: d:\dev\CNC\CNC_3D\src\Gcode\example
(Program: EXAMPLE)
(Description: Demo operations - face, drill, pocket, contour)
(Units: mm, Absolute positioning)

%
G90 G21 G17 G94          (Abs, mm, XY plane, feed/min)
G28                      (Home all)
T1 M6                    (Tool change: T1 Flat End Mill D6)
S12000 M3                (Spindle CW 12000 RPM)
G54                      (Work offset)
G0 X0 Y0
G0 Z15

(--- Facing pass 60x40 @ Z-0.5 ---)
G0 X-30 Y-20
G1 Z2 F600
G1 Z-0.5 F300  
G1 X30 Y-20 F1200
G1 X30 Y-18
G1 X-30 Y-18
G1 X-30 Y-16
G1 X30 Y-16
G1 X30 Y-14
G1 X-30 Y-14
G1 X-30 Y-12
G1 X30 Y-12
G1 X30 Y-10
G1 X-30 Y-10
G1 X-30 Y-8
G1 X30 Y-8
G1 X30 Y-6
G1 X-30 Y-6
G1 X-30 Y-4
G1 X30 Y-4
G1 X30 Y-2
G1 X-30 Y-2
G1 X-30 Y0
G1 X30 Y0
G1 X30 Y2
G1 X-30 Y2
G1 X-30 Y4
G1 X30 Y4
G1 X30 Y6
G1 X-30 Y6
G1 X-30 Y8
G1 X30 Y8
G1 X30 Y10
G1 X-30 Y10
G1 X-30 Y12
G1 X30 Y12
G1 X30 Y14
G1 X-30 Y14
G1 X-30 Y16
G1 X30 Y16
G1 X30 Y18
G1 X-30 Y18
G1 X-30 Y20
G1 X30 Y20
G0 Z15

(--- Peck drill 3 holes Ø6 target Z-12 ---)
(# Using same end mill for demo; real job use drill)
G0 X-20 Y-25
G1 Z2 F800
G83 X-20 Y-25 Z-12 R2 Q3 F250  (Peck: depth -12, retract plane 2, step 3)
G83 X0   Y-25 Z-12 R2 Q3 F250
G83 X20  Y-25 Z-12 R2 Q3 F250
G80
G0 Z15

(--- Rectangular pocket 20x15 center (0,10) depth -6 stepdown 2 ---)
G0 X-10 Y2
G1 Z2 F800
G1 Z-2 F300
G1 X10 Y2 F1000
G1 X10 Y18
G1 X-10 Y18
G1 X-10 Y2
G1 X-9 Y3
G1 X9 Y3
G1 X9 Y17
G1 X-9 Y17
G1 X-9 Y3
G1 Z-4 F300
G1 X10 Y2 F1000
G1 X10 Y18
G1 X-10 Y18
G1 X-10 Y2
G1 Z-6 F300
G1 X10 Y2 F1000
G1 X10 Y18
G1 X-10 Y18
G1 X-10 Y2
G0 Z15

(--- Outside contour 80x60 around origin, depth -4 stepdown 2 ---)
G0 X-40 Y-30
G1 Z2 F800
G1 Z-2 F300
G1 X40 Y-30 F1200
G1 X40 Y30
G1 X-40 Y30
G1 X-40 Y-30
G1 Z-4 F300
G1 X40 Y-30 F1200
G1 X40 Y30
G1 X-40 Y30
G1 X-40 Y-30
G0 Z15

(--- Finish & return ---)
G0 X0 Y0
M5
G28 Z0
G28 X0 Y0