#pragma once
#include "Drillbit.h"

namespace Common {
     class ToolConfig {
     public:
          virtual bool addTool(const Model::DrillBits& bit) = 0;
          virtual bool removeToolById(int id) = 0;
          virtual ~ToolConfig() = default;
     };
}
