// Minimization.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Minimizer.h"


int main(int argc, char* argv[])
{
	if (argc != 2) {
		std::cout << "Enter name of file with automat info" << std::endl;
		return 1;
	}

	CMinimizer minimizer(argv[1]);
	//minimizer.ShowOriginalAutomat();
	std::cout << std::endl;
	//minimizer.ShowMinimilizeAutomat();

    return 0;
}

