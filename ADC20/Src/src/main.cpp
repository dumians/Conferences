#include <stdio.h>
#include <limits.h>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <limits.h>

//#include <boost/regex.hpp>
#include <iostream>

#ifdef __linux__ 
    //linux code goes here
	#include <unistd.h>
#elif defined(WIN32) || defined(_WIN32) || defined(_WIN64)
  // windows code goes here
	#include <windows.h>
	#include <Lmcons.h>
    
	#include "Winsock2.h" 
#endif

using namespace std;

int main() {

	char hostname[HOST_NAME_MAX];
	char username[LOGIN_NAME_MAX];
	 gethostname(hostname, HOST_NAME_MAX);

	 #if defined(WIN32) || defined(_WIN32) || defined(_WIN64)
		DWORD username_len = UNLEN+1;
		GetUserName(username, &username_len);
	#else
		 getlogin_r(username, LOGIN_NAME_MAX);
	#endif

	 cout << "ADC C++ 20 Docker hostname: " << hostname << ", login: " << username << endl; // prints Hello World!
 	 std::cout << "from Docker container!" << std::endl;

//  constexpr auto train = R"(
//     ___                     _                          __   ___    _|"|_   _|"|_  
//    |   \    ___     __     | |__    ___      _ _      /"/  / __|  |_   _| |_   _| 
//    | |) |  / _ \   / _|    | / /   / -_)    | '_|    / /  | (__     |_|     |_|   
//    |___/   \___/   \__|_   |_\_\   \___|   _|_|_   _/_/_   \___|   _____   _____  
//   _|"""""|_|"""""|_|"""""|_|"""""|_|"""""|_|"""""|_|"""""|_|"""""|_|     |_|     | 
//   "`-0-0-'"`-0-0-'"`-0-0-'"`-0-0-'"`-0-0-'"`-0-0-'"`-0-0-'"`-0-0-'"`-0-0-'"`-0-0-' 
//   )";

//      std::cout << train;
	
	return 0;
}