cmake_minimum_required(VERSION 3.11)

project(Adc19
        VERSION 0.1
        DESCRIPTION "ADC19++ project using different c++ libs")

add_definitions(-D_WIN32_WINNT=0x0501)

add_executable(Adc19 main.cpp)

find_package(Threads)
find_package(Boost REQUIRED COMPONENTS system filesystem)
find_package(poco REQUIRED)

target_link_libraries(
    Adc19
    PRIVATE
        ${CMAKE_THREAD_LIBS_INIT}
        ${Boost_LIBRARIES}
		$(poco_DIR)
)

set_property(TARGET Adc19 PROPERTY CXX_STANDARD 14)


