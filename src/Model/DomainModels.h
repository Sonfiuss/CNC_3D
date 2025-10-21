#pragma once

#include <chrono>
#include <cstdint>
#include <filesystem>
#include <optional>
#include <string>
#include <vector>

struct Vector3D {
    double X{0.0};
    double Y{0.0};
    double Z{0.0};
};

enum class GCodeDownloadStatus {
    Pending,
    InProgress,
    Completed,
    Failed
};

struct MaterialInfo {
    std::string material_id;           // Mã chất liệu
    std::string material_type;         // Loại vật liệu (gỗ, nhựa, ...)
    Vector3D dimensions_mm;            // Kích thước phôi theo trục X/Y/Z (mm)
};

struct ToolInfo {
    int tool_number{0};                // Thứ tự dao trong chương trình CNC (T-code)
    std::string tool_id;               // Khóa chính trong database
    std::string name;                  // Tên hiển thị
    std::string type;                  // Nhóm dao (Endmill, Drill, ...)
    double diameter_mm{0.0};
    double length_mm{0.0};
    bool requires_calibration{false};  // Đánh dấu cần cân chỉnh trước khi chạy
};

struct UARTFrame {
    uint8_t command{0};
    std::vector<uint8_t> payload;      // Dữ liệu gửi tới STM32
    uint16_t checksum{0};
    uint32_t sequence{0};
    std::chrono::system_clock::time_point timestamp{
        std::chrono::system_clock::now()
    };
};

struct ControlFromAppCmd {
    std::string command;               // Mã lệnh (start, pause, ...)
    std::string source_device;         // Thiết bị gửi lệnh (id app, user)
    std::vector<std::string> parameters;   // Thông số bổ sung
    std::chrono::system_clock::time_point issued_at{
        std::chrono::system_clock::now()
    };
};

struct RqGcodeFile {
    std::filesystem::path local_path;  // Đường dẫn lưu file G-code
    GCodeDownloadStatus status{GCodeDownloadStatus::Pending};
    std::optional<std::string> error_message; // Lưu lỗi nếu tải thất bại
};

struct GCodeUpdateCommand {
    std::string program_id;            // Mã chương trình cần cập nhật
    std::string version;               // Phiên bản G-code
    std::vector<RqGcodeFile> files;    // Danh sách file G-code liên quan
    std::chrono::system_clock::time_point requested_at{
        std::chrono::system_clock::now()
    };
    std::optional<std::string> requested_by; // Người/thiết bị yêu cầu cập nhật
};

struct ProductPartInfo {
    std::string part_id;               // Mã chi tiết trong sản phẩm
    std::string name;                  // Tên chi tiết
    MaterialInfo material;             // Vật liệu sử dụng cho chi tiết
};

struct Product {
    std::string product_id;            // Mã sản phẩm hoàn thiện
    std::string name;                  // Tên sản phẩm
    std::vector<ProductPartInfo> parts;// Danh sách các chi tiết cấu thành
    std::string description;           // Mô tả thêm (tùy chọn)
};

struct ProgramInfo {
    std::string program_id;            // Định danh duy nhất chương trình
    std::string program_name;          // Tên chương trình
    std::string machine;               // Máy CNC chạy chương trình
    MaterialInfo material;             // Thông tin phôi chính
    std::vector<ToolInfo> tools;       // Danh sách dao sử dụng
    Vector3D zero_point;               // Điểm zero tham chiếu (X/Y/Z)
    std::filesystem::path source_file; // Đường dẫn tới file G-code gốc
    std::optional<std::string> notes;  // Ghi chú bổ sung (nếu có)
};
