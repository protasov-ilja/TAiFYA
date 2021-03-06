// Minimization.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Minimizer.h"

int main(int argc, char* argv[])
{
	if (argc != 3)
	{
		std::cout << "enter the name of input file and name of output file" << std::endl;
		return 1;
	}

	CMinimizer minimizer(argv[1]);
	minimizer.ShowMinimizedStateMachine(argv[2]);
	minimizer.CreateDotFile();

	return 0;
}
