#pragma once
#include <string>

namespace Model {
    struct DrillBits {
        int id;
        std::string name;
        std::string type;
        double diameter;
        int flute_count;
        std::string material;
        double length;
        double stick_out;
        int max_rpm;
        double recommended_feed;
        double recommended_plunge;
        double step_over;
        double step_down;
        std::string comment;
    };
}
