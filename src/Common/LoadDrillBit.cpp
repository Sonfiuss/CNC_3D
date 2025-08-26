#include "LoadDrillBit.h"
#include <fstream>
#include <sstream>

#include <algorithm>
namespace Common {

     DrillBitConfig::DrillBitConfig(const std::string& file) : filename(file) {
          bits = loadAll(file);
     }

     std::vector<Model::DrillBits> DrillBitConfig::getAllBits() const {
          return bits;
     }

     Model::DrillBits DrillBitConfig::getBitById(int id) const {
          for (const auto& bit : bits) {
               if (bit.id == id) return bit;
          }
          return Model::DrillBits{};
     }

     bool DrillBitConfig::addTool(const Model::DrillBits& bit) {
          bits.push_back(bit);
          return save(filename, bits);
     }

     bool DrillBitConfig::removeToolById(int id) {
          auto it = std::remove_if(bits.begin(), bits.end(), [id](const Model::DrillBits& bit) { return bit.id == id; });
          if (it != bits.end()) {
               bits.erase(it, bits.end());
               return save(filename, bits);
          }
          return false;
     }

     // Hàm ghi/đọc string vào file binary
     void DrillBitConfig::writeString(std::ofstream& file, const std::string& str) {
          size_t len = str.size();
          file.write(reinterpret_cast<const char*>(&len), sizeof(len));
          file.write(str.c_str(), len);
     }

     std::string DrillBitConfig::readString(std::ifstream& file) {
          constexpr size_t kMaxStringLen = 4096; // guard against corruption
          size_t len = 0;
          file.read(reinterpret_cast<char*>(&len), sizeof(len));
          if (!file.good()) {
               return {};
          }
          if (len > kMaxStringLen) { // suspicious length -> mark fail & return
               file.setstate(std::ios::failbit);
               return {};
          }
          std::string str(len, '\0');
          if (len > 0) {
               file.read(&str[0], len);
               if (!file.good()) {
                    return {};
               }
          }
          return str;
     }

     std::vector<Model::DrillBits> DrillBitConfig::loadAll(const std::string& filename) {
          std::vector<Model::DrillBits> bits;
          std::ifstream file(filename, std::ios::binary);
          if (!file.is_open()) return bits;

          while (true) {
               Model::DrillBits bit{};

               // Read fixed-size primitive fields in order, with early break on failure.
               if (!file.read(reinterpret_cast<char*>(&bit.id), sizeof(bit.id))) break;

               bit.name = readString(file); if (!file.good()) break;
               bit.type = readString(file); if (!file.good()) break;

               if (!file.read(reinterpret_cast<char*>(&bit.diameter), sizeof(bit.diameter))) break;
               if (!file.read(reinterpret_cast<char*>(&bit.flute_count), sizeof(bit.flute_count))) break;

               bit.material = readString(file); if (!file.good()) break;

               if (!file.read(reinterpret_cast<char*>(&bit.length), sizeof(bit.length))) break;
               if (!file.read(reinterpret_cast<char*>(&bit.stick_out), sizeof(bit.stick_out))) break;
               if (!file.read(reinterpret_cast<char*>(&bit.max_rpm), sizeof(bit.max_rpm))) break;
               if (!file.read(reinterpret_cast<char*>(&bit.recommended_feed), sizeof(bit.recommended_feed))) break;
               if (!file.read(reinterpret_cast<char*>(&bit.recommended_plunge), sizeof(bit.recommended_plunge))) break;
               if (!file.read(reinterpret_cast<char*>(&bit.step_over), sizeof(bit.step_over))) break;
               if (!file.read(reinterpret_cast<char*>(&bit.step_down), sizeof(bit.step_down))) break;

               bit.comment = readString(file); if (!file.good()) break;

               bits.push_back(std::move(bit));
          }

          // Optional: clear failbit if it was only due to EOF after a clean record
          // (Not strictly necessary; caller already copies vector.)
          return bits;
     }

     bool DrillBitConfig::save(const std::string& filename, const std::vector<Model::DrillBits>& bits) {
          std::ofstream file(filename, std::ios::binary);
          if (!file.is_open()) return false;
          for (const auto& bit : bits) {
               file.write(reinterpret_cast<const char*>(&bit.id), sizeof(bit.id));
               writeString(file, bit.name);
               writeString(file, bit.type);
               file.write(reinterpret_cast<const char*>(&bit.diameter), sizeof(bit.diameter));
               file.write(reinterpret_cast<const char*>(&bit.flute_count), sizeof(bit.flute_count));
               writeString(file, bit.material);
               file.write(reinterpret_cast<const char*>(&bit.length), sizeof(bit.length));
               file.write(reinterpret_cast<const char*>(&bit.stick_out), sizeof(bit.stick_out));
               file.write(reinterpret_cast<const char*>(&bit.max_rpm), sizeof(bit.max_rpm));
               file.write(reinterpret_cast<const char*>(&bit.recommended_feed), sizeof(bit.recommended_feed));
               file.write(reinterpret_cast<const char*>(&bit.recommended_plunge), sizeof(bit.recommended_plunge));
               file.write(reinterpret_cast<const char*>(&bit.step_over), sizeof(bit.step_over));
               file.write(reinterpret_cast<const char*>(&bit.step_down), sizeof(bit.step_down));
               writeString(file, bit.comment);
          }
          file.close();
          return true;
     }
} // namespace Common




