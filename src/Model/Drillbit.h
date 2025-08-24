#include <string>


using namespace std;
namespace Model {
    struct DrillBits {
        int id;
        string name;
        string type;
        double diameter;
        int flute_count;
        string material;
        double length;
        double stick_out;
        int max_rpm;
        double recommended_feed;
        double recommended_plunge;
        double step_over;
        double step_down;
        string comment;
    };
}
