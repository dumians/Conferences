cmake_minimum_required(VERSION 3.5)

project(HWord)

set(HWORD_VERSION_MAJOR 1)
set(HWORD_VERSION_MINOR 0)

set(SOURCE ${PROJECT_SOURCE_DIR}/main.cpp)
# include_directories(${PROJECT_SOURCE_DIR}/include)

add_executable(${PROJECT_NAME} ${SOURCE})