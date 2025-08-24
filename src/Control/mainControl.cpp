#include <fstream>
#include <iostream>
#include <sstream>
#include <string>
#include "Drillbit.h"

using namespace Model;
int main() {
    std::ifstream file("src/Config/DrillBit/DrilBit.csv");
    if (!file.is_open()) {
        std::cerr << "cann't open file CSV!" << std::endl;
        return 1;
    }

    std::string line;
    // Bỏ qua dòng tiêu đề
    std::getline(file, line);

    while (std::getline(file, line)) {
        std::stringstream ss(line);
        DrillBits bit;
        std::string value;

        // Đọc từng trường, ngăn cách bởi dấu phẩy
        std::getline(ss, value, ','); bit.id = std::stoi(value);
        std::getline(ss, bit.name, ',');
        std::getline(ss, bit.type, ',');
        std::getline(ss, value, ','); bit.diameter = std::stod(value);
        std::getline(ss, value, ','); bit.flute_count = std::stoi(value);
        std::getline(ss, bit.material, ',');
        std::getline(ss, value, ','); bit.length = std::stod(value);
        std::getline(ss, value, ','); bit.stick_out = std::stod(value);
        std::getline(ss, value, ','); bit.max_rpm = std::stoi(value);
        std::getline(ss, value, ','); bit.recommended_feed = std::stod(value);
        std::getline(ss, value, ','); bit.recommended_plunge = std::stod(value);
        std::getline(ss, value, ','); bit.step_over = std::stod(value);
        std::getline(ss, value, ','); bit.step_down = std::stod(value);
        std::getline(ss, bit.comment, ',');

        // In ra để kiểm tra
        std::cout << "DrillBit: " << bit.name << ", Diameter: " << bit.diameter << std::endl;
    }

    file.close();
    return 0;
}