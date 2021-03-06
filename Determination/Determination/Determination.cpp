// Determination.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Determinator.h"

int main(int argc, char* argv[])
{
	if (argc != 3)
	{
		std::cout << "enter the name of input file and name of output file" << std::endl;

		return 1;
	}

	CDeterminator determinator(argv[1]);
	determinator.ShowDeterminatedStateMachine(argv[2]);
	determinator.CreateDotFile(argv[2]);

    return 0;
}

