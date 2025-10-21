
#include "Repository/GcodeReader.h"
#include <iostream>

int main() {
    ProgramInfo info = readGCodeFile("D:\\dev\\CNC\\CNC_3D\\src\\Config\\Gcode\\A80W133_DW_ Rev. B_POS_1.nc");
    std::cout << "Program: " << info.program_name << std::endl;
    std::cout << "Machine: " << info.machine << std::endl;
    std::cout << "Material: " << info.material.material_type << std::endl;
    std::cout << "Tool count: " << info.tools.size() << std::endl;
    std::cout << "Zero X: " << info.zero_point.X << std::endl;
    return 0;
}
