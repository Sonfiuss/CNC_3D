cmake_minimum_required(VERSION 3.10)
project(CNC_3D)

set(CMAKE_CXX_STANDARD 17)

# Default to Debug build if not specified (single-config generators like MinGW Makefiles)
if(NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Debug CACHE STRING "Build type" FORCE)
endif()

# Extra debug flags (non-intrusive). Release left untouched.
set(CMAKE_CXX_FLAGS_DEBUG "${CMAKE_CXX_FLAGS_DEBUG} -O0 -g -fno-omit-frame-pointer -DDEBUG")

# Chỉ build các phần Model, Control, Utils, Lib và Program.cpp
file(GLOB_RECURSE MODEL_SRC src/Model/*.cpp)
file(GLOB_RECURSE CONTROL_SRC src/Control/*.cpp)
file(GLOB_RECURSE UTILS_SRC src/Utils/*.cpp)
file(GLOB_RECURSE LIB_SRC src/Lib/*.cpp)
file(GLOB_RECURSE COMMON_SRC src/Common/*.cpp)
set(PROGRAM_SRC src/Program.cpp)

include_directories(
    src/Model
    src/Common
    src/Control
    src/Utils
    src/Lib
    src/Config
)

add_executable(CNC_3D
    ${MODEL_SRC}
    ${CONTROL_SRC}
    ${UTILS_SRC}
    ${LIB_SRC}
    ${COMMON_SRC}
    ${PROGRAM_SRC}
)

# Target-specific compile options (ensures flags even if multi-config generator later)
target_compile_options(CNC_3D PRIVATE
    $<$<CONFIG:Debug>:-O0 -g -fno-omit-frame-pointer -DDEBUG>
)

