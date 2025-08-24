cmake_minimum_required(VERSION 3.10)
project(CNC_3D)

set(CMAKE_CXX_STANDARD 17)

# Thêm tất cả file nguồn trong src và các thư mục con
file(GLOB_RECURSE SOURCES
    src/*.cpp
)

# Thêm include directories
include_directories(
    src/Model
    src/Control
    src/Utils
    src/Config
)

add_executable(CNC_3D ${SOURCES})
