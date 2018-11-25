#include "stdafx.h"
#include "Determinator.h"

CDeterminator::CDeterminator(const std::string& inputfileName)
{
	ReadStateMachineInfoFromFile(inputfileName);
	DeterminateStateMachine();
}

CDeterminator::~CDeterminator()
{
}

void CDeterminator::ReadStateMachineInfoFromFile(const std::string& inputfileName)
{
	std::ifstream inputFile(inputfileName + ".txt");
	if (!inputFile.is_open())
	{
		std::cout << "Can't open the file for reading! " << inputfileName << std::endl;

		return;
	}

	inputFile >> _numberOfInputSignals;
	inputFile >> _numberOfStates;
	inputFile >> _numberOfFinalStates;
	for (int i = 0; i < _numberOfFinalStates; ++i)
	{
		int tempNumb;
		inputFile >> tempNumb;
		_finalStates.insert(tempNumb);
	}

	ReadStateMachineFromFile(inputFile);
}

void CDeterminator::ReadStateMachineFromFile(std::ifstream& inputFile)
{
	_initialStateMachine = GetEmptyStateMachine(_numberOfStates, _numberOfInputSignals);
	std::string str;
	std::getline(inputFile, str);
	int index = 0;
	while (std::getline(inputFile, str))
	{
		if (index < _numberOfStates)
		{
			std::stringstream stream(str);
			int state;
			int signal;
			while (stream >> state >> signal)
			{
				_initialStateMachine[index][signal].states.insert(state);
			}
		}
		
		index++;
	}
}

std::vector<std::vector<StateMachineState>> CDeterminator::GetEmptyStateMachine(int numberOfStates, int numberOfInputSignals)
{
	std::vector<std::vector<StateMachineState>> stateMachine(numberOfStates, std::vector<StateMachineState>(numberOfInputSignals));
	for (int i = 0; i < stateMachine.size(); ++i)
	{
		for (int j = 0; j < stateMachine[i].size(); ++j)
		{
			StateMachineState st;
			stateMachine[i][j] = st;
		}
	}

	return stateMachine;
}

void CDeterminator::DeterminateStateMachine()
{
	std::queue<std::set<int>> statesQueue;
	_statesList.insert(std::pair<std::set<int>, int>({ 0 }, 0));
	std::set<int> set = { 0 };
	StateMachineState st(0, set);
	AddStateInFinalStates(st);
	AddNewState(set, statesQueue);
	while (statesQueue.size() != 0) 
	{
		std::set<int> state = statesQueue.front();
		statesQueue.pop();
		AddNewState(state, statesQueue);
	}
}

void CDeterminator::AddNewState(const std::set<int>& initStates, std::queue<std::set<int>>& queue)
{
	std::vector<StateMachineState> states(_numberOfInputSignals);
	for (int i = 0; i < _initialStateMachine[0].size(); ++i)
	{
		InsertStatesInState(initStates, states[i], i);
		if (states[i].states.size() != 0)
		{
			auto search = _statesList.find(states[i].states);
			if (search == _statesList.end())
			{
				states[i].stateIndex = _statesList.size();
				_statesList.insert(std::pair<std::set<int>, int>(states[i].states, states[i].stateIndex));
				queue.push(states[i].states);
				AddStateInFinalStates(states[i]);
			}
			else 
			{
				states[i].stateIndex = search -> second;
			}
		}
	}

	_determinatedStateMachine.push_back(states);
}

void CDeterminator::InsertStatesInState(const std::set<int>& initStates, StateMachineState& state, int initStateIndex)
{
	bool isFirstIteration = false;
	for (auto initState : initStates)
	{
		if (isFirstIteration)
		{
			StateMachineState st;
			state = st;
		}

		for (auto st : _initialStateMachine[initState][initStateIndex].states)
		{
			state.states.insert(st);
		}
	}
}

void CDeterminator::AddStateInFinalStates(StateMachineState state)
{
	for (auto finalState : _finalStates)
	{
		if (state.states.count(finalState))
		{
			_determinatedFinalStates.insert(state.stateIndex);
			return;
		}
	}
}

void CDeterminator::ShowDeterminatedStateMachine(const std::string& outputFileName)
{
	std::ofstream outputFile(outputFileName + ".txt");
	if (!outputFile.is_open())
	{
		std::cout << "Can't open the file for writing! " << outputFileName << std::endl;

		return;
	}

	outputFile << _numberOfInputSignals << std::endl;
	outputFile << _determinatedStateMachine.size() << std::endl;
	outputFile << _determinatedFinalStates.size() << std::endl;
	for (auto finalState : _determinatedFinalStates)
	{
		outputFile << finalState << " ";
	}

	outputFile << std::endl;
	for (int i = 0; i < _determinatedStateMachine[0].size(); ++i)
	{
		for (int j = 0; j < _determinatedStateMachine.size(); ++j)
		{
			outputFile << _determinatedStateMachine[j][i].stateIndex << " ";
		}

		outputFile << std::endl;
	}
}

void CDeterminator::CreateDotFile(const std::string& dotFileName)
{
	std::ofstream dotFile(dotFileName + ".dot");
	dotFile << "digraph DeterminatedStateMachine {" << std::endl;
	for (size_t i = 0; i < _determinatedStateMachine.size(); ++i) 
	{
		if (_determinatedFinalStates.count(i)) 
		{
			dotFile << i << " [shape = box]" << std::endl;
		}
		else 
		{
			dotFile << i << std::endl;
		}
	}

	for (size_t i = 0; i < _determinatedStateMachine.size(); ++i)
	{
		for (size_t j = 0; j < _determinatedStateMachine[i].size(); ++j)
		{
			if (_determinatedStateMachine[i][j].stateIndex != -1)
			{
				dotFile << "	" << i << "->" << _determinatedStateMachine[i][j].stateIndex << "[label=" << j << ']' << std::endl;
			}
		}
	}

	dotFile << "}";
	dotFile.close();
}
