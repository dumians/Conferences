#include <Poco/MD5Engine.h>
#include <boost/regex.hpp>
#include <iostream>
int main() {
	Poco::MD5Engine md5;
	md5.update("Adc++19 Hello World from Container");
	std::string md5string = Poco::DigestEngine::digestToHex(md5.d
	std::cout << "MD5= " << md5string << '\n';
	boost::regex expr{R"(\w+\s\w+)"};
	std::cout << boost::regex_match("Adc++19 Hello World from Container", expr) << '\n';
	return EXIT_SUCCESS;
}