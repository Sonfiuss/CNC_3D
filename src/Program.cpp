#include <iostream>
#include "Common/LoadDrillBit.h"

int main() {
	std::string filename = "src/Config/DrillBit/DrilBit.bin";
	Common::DrillBitConfig* drillConfig  = new Common::DrillBitConfig(filename);
	auto bits = drillConfig->getAllBits();
	std::cout << "List of DrillBits:\n";
	for (const auto& bit : bits) {
		std::cout << "ID: " << bit.id << ", Name: " << bit.name << ", Diameter: " << bit.diameter << std::endl;
	}
	delete drillConfig;
	return 0;
}
