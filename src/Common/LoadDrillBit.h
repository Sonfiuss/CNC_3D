#pragma once
#include <vector>
#include "Drillbit.h"
#include "ToolConfig.h"

namespace Common {
     class DrillBitConfig : public ToolConfig {
          public:

               DrillBitConfig(const std::string& file);
               std::vector<Model::DrillBits> getAllBits() const;
               Model::DrillBits getBitById(int id) const;
               bool addTool(const Model::DrillBits& bit);
               bool removeToolById(int id);
               bool save(const std::string& filename, const std::vector<Model::DrillBits>& bits);
               
          private:
               std::string filename;
               std::vector<Model::DrillBits> bits;

               // Hàm ghi/đọc string vào file binary
               void writeString(std::ofstream& file, const std::string& str);
               std::string readString(std::ifstream& file);
               std::vector<Model::DrillBits> loadAll(const std::string& filename);

     };

}