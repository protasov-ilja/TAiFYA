#include "stdafx.h"
#include "Minimizer.h"

CMinimizer::CMinimizer(const std::string& inputfileName)
{
	ReadStateMachinInfoFromFile(inputfileName);
	CreateFirstTableOfEquivalentClasses();
	UpdateTableOfStates();
	MinimizeStateMachine();
	CreateMinimizedStatesMachine();
}

void CMinimizer::ReadStateMachinInfoFromFile(const std::string& inputfileName)
{
	std::ifstream inputFile(inputfileName);
	if (!inputFile.is_open())
	{
		std::cout << "Can't open the file!" << inputfileName << std::endl;
		return;
	}

	int type;
	inputFile >> type;
	_stateMachineType = (StateMachineType)type;
	inputFile >> _numberOfInputSignals;
	inputFile >> _numberOfActions;
	inputFile >> _numberOfStates;
	if (_stateMachineType == StateMachineType::MEALY)
	{
		ReadMealyFromFile(inputFile);
	}
	else
	{
		ReadMooreFromFile(inputFile);
	}
}

void CMinimizer::ReadMealyFromFile(std::ifstream& inputFile)
{
	for (size_t i = 0; i < _numberOfInputSignals; ++i)
	{
		std::vector<StateMachineState> statesLine(_numberOfStates);
		for (size_t j = 0; j < _numberOfStates; ++j)
		{
			inputFile >> statesLine[j].state >> statesLine[j].action;
			statesLine[j].action--;
		}

		_initialStateMachine.push_back(statesLine);
	}
}

void CMinimizer::ReadMooreFromFile(std::ifstream& inputFile)
{
	std::vector<int> vectorOfActions(_numberOfStates);
	for (size_t i = 0; i < _numberOfStates; ++i)
	{
		inputFile >> vectorOfActions[i];
		vectorOfActions[i]--;
	}

	for (size_t i = 0; i < _numberOfInputSignals; ++i)
	{
		std::vector<StateMachineState> statesLine(_numberOfStates);
		for (size_t j = 0; j < _numberOfStates; ++j)
		{
			inputFile >> statesLine[j].state;
			statesLine[j].action = vectorOfActions[j];
		}

		_initialStateMachine.push_back(statesLine);
	}
}

CMinimizer::~CMinimizer()
{
}

void CMinimizer::CreateFirstTableOfEquivalentClasses()
{
	_equivalenceClassComponents = CreateEquivalenceClassComponents();
	int id = 0;
	for (size_t i = 0; i < _equivalenceClassComponents.size(); ++i)
	{
		for (size_t j = 0; j < _equivalenceClassComponents.size(); ++j)
		{
			if ((_equivalenceClassComponents[j].id != -1) && (_equivalenceClassComponents[i].states == _equivalenceClassComponents[j].states))
			{
				_equivalenceClassComponents[i].id = _equivalenceClassComponents[j].id;
				_currEquivalenceClasses[_equivalenceClassComponents[i].id].states.insert(_equivalenceClassComponents[i].originState);

				break;
			}
		}

		if (_equivalenceClassComponents[i].id == -1)
		{
			_equivalenceClassComponents[i].id = id;
			EquivalenceClass eqvivClass;
			eqvivClass.id = id;
			eqvivClass.states.insert(_equivalenceClassComponents[i].originState);
			_currEquivalenceClasses.push_back(eqvivClass);
			++id;
		}
	}
}

void CMinimizer::MinimizeStateMachine()
{
	while (_prevEquivalenceClasses.size() != _currEquivalenceClasses.size())
	{
		_currEquivalenceClasses = GetNewVectorOfEquivalenceClasses(_currEquivalenceClasses);
		UpdateTableOfStates();
	}
}

std::vector<EquivalenceClass> CMinimizer::GetNewVectorOfEquivalenceClasses(const std::vector<EquivalenceClass>& equiveClasses)
{
	_prevEquivalenceClasses = equiveClasses;
	std::vector<EquivalenceClass> newEquivalenceClasses;
	for (size_t i = 0; i < _prevEquivalenceClasses.size(); ++i)
	{
		int testState = *_prevEquivalenceClasses[i].states.begin();
		int currStateId = 0;
		if (newEquivalenceClasses.size() != 0)
		{
			currStateId = newEquivalenceClasses[newEquivalenceClasses.size() - 1].id + 1;
		}

		EquivalenceClass newEquivalenceClass;
		newEquivalenceClass.id = currStateId;
		newEquivalenceClass.states.insert(testState);
		_equivalenceClassComponents[testState].id = newEquivalenceClass.id;
		newEquivalenceClasses.push_back(newEquivalenceClass);
		for (auto it = _prevEquivalenceClasses[i].states.begin(); it != _prevEquivalenceClasses[i].states.end(); ++it)
		{
			int verifableState = *it;
			if (_equivalenceClassComponents[testState].states != _equivalenceClassComponents[verifableState].states)
			{
				int newId = currStateId + 1;
				bool isNewPositionFound = false;
				while (!isNewPositionFound)
				{
					if (newId < newEquivalenceClasses.size())
					{
						int nextState = *newEquivalenceClasses[newId].states.begin();
						if (_equivalenceClassComponents[verifableState].states == _equivalenceClassComponents[nextState].states)
						{
							newEquivalenceClasses[newId].states.insert(verifableState);
							_equivalenceClassComponents[verifableState].id = newEquivalenceClasses[newId].id;
							isNewPositionFound = true;
						}
					}
					else
					{
						EquivalenceClass equivalenceClass;
						equivalenceClass.id = newId;
						equivalenceClass.states.insert(verifableState);
						newEquivalenceClasses.push_back(equivalenceClass);
						_equivalenceClassComponents[verifableState].id = newEquivalenceClasses[newId].id;
						isNewPositionFound = true;
					}

					newId++;
				}
			}
			else
			{
				newEquivalenceClasses[currStateId].states.insert(verifableState);
				_equivalenceClassComponents[verifableState].id = newEquivalenceClasses[currStateId].id;
			}
		}
	}

	return newEquivalenceClasses;
}

void CMinimizer::UpdateTableOfStates()
{
	for (size_t i = 0; i < _initialStateMachine.size(); ++i)
	{
		for (size_t j = 0; j < _initialStateMachine[i].size(); ++j)
		{
			int newId = GetEquivalenceClassIdByState(_currEquivalenceClasses, _initialStateMachine[i][j].state);
			if (newId != -1)
			{
				_equivalenceClassComponents[j].states[i] = newId;
			}
		}
	}
}

std::vector<EquivalenceClassComponent> CMinimizer::CreateEquivalenceClassComponents()
{
	std::vector<EquivalenceClassComponent> components;
	for (int i = 0; i < _initialStateMachine[0].size(); ++i)
	{
		EquivalenceClassComponent component;
		for (size_t j = 0; j < _initialStateMachine.size(); ++j)
		{
			component.originState = i;
			component.actions.push_back(_initialStateMachine[j][i].action);
			component.states.push_back(_initialStateMachine[j][i].action);
		}

		components.push_back(component);
	}

	return components;
}

int CMinimizer::GetEquivalenceClassIdByState(const std::vector<EquivalenceClass>& equivalenceClasses, int state)
{
	for (size_t k = 0; k < equivalenceClasses.size(); ++k)
	{
		if (equivalenceClasses[k].states.count(state) != 0)
		{
			return equivalenceClasses[k].id;
		}
	}

	return -1;
}

void CMinimizer::CreateMinimizedStatesMachine()
{
	std::vector<std::vector<StateMachineState>> minimilizeAutomat(_numberOfInputSignals);
	for (size_t j = 0; j < _numberOfInputSignals; ++j)
	{
		std::vector<StateMachineState> stateMachineStates(_currEquivalenceClasses.size());
		for (size_t i = 0; i < _currEquivalenceClasses.size(); ++i)
		{
			StateMachineState stateMachineState;
			int currState = *_currEquivalenceClasses[i].states.begin();
			stateMachineState.state = _equivalenceClassComponents[currState].states[j];
			stateMachineState.action = _equivalenceClassComponents[currState].actions[j];
			stateMachineStates[i] = stateMachineState;
		}

		minimilizeAutomat[j] = stateMachineStates;
	}

	_minimizedStateMachine = minimilizeAutomat;
}

void CMinimizer::ShowMinimizedStateMachine(const std::string& outputFileName)
{
	std::ofstream outputFile(outputFileName);
	if (!outputFile.is_open())
	{
		std::cout << "Cant open file " << outputFileName << " for writing" << std::endl;
		return;
	}

	outputFile << (int)_stateMachineType << std::endl;
	outputFile << _numberOfInputSignals << std::endl;
	outputFile << _numberOfActions << std::endl;
	outputFile << _minimizedStateMachine[0].size() << std::endl;
	if (_stateMachineType == StateMachineType::MOORE)
	{
		for (size_t j = 0; j < _minimizedStateMachine[0].size(); ++j)
		{
			outputFile << ++_minimizedStateMachine[0][j].action << " ";
		}

		outputFile << std::endl;
		for (size_t i = 0; i < _minimizedStateMachine.size(); ++i)
		{
			for (size_t j = 0; j < _minimizedStateMachine[i].size(); ++j)
			{
				outputFile << _minimizedStateMachine[i][j].state << " ";
			}

			outputFile << std::endl;
		}
	}
	else
	{
		for (size_t i = 0; i < _minimizedStateMachine.size(); ++i)
		{
			for (size_t j = 0; j < _minimizedStateMachine[i].size(); ++j)
			{
				outputFile << _minimizedStateMachine[i][j].state << " " << ++_minimizedStateMachine[i][j].action << " ";
			}

			outputFile << std::endl;
		}
	}

	if (!outputFile.flush())
	{
		std::cout << "Failed to save data on disk" << std::endl;
		return;
	}
}
