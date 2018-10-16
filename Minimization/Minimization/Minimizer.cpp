#include "stdafx.h"
#include "Minimizer.h"

const std::string OUTPUT_FILE_NAME = "out.txt";
const int MIN_NUMBER_OF_ACTIONS = 1;
const int MIN_NUMBER_OF_INPUT_SIGNALS = 1;

CMinimizer::CMinimizer(const std::string& fileName)
{
	std::ifstream inputFile(fileName);
	if (!inputFile.is_open())
	{
		std::cout << "Can't open the file!" << fileName << std::endl;
		return;
	}

	int type;
	inputFile >> type;
	if (!(type == StateMachineType::MOORE || type == StateMachineType::MEALY))
	{
		std::cout << type << "number is not MUR: 1 or MILLY: 2" << std::endl;
		return;
	}

	_stateMachineType = (StateMachineType)type;

	inputFile >> _numberOfInputSignals;
	if (_numberOfInputSignals < MIN_NUMBER_OF_INPUT_SIGNALS)
	{
		std::cout << "number of states can't be negative number!" << std::endl;
		return;
	}

	inputFile >> _numberOfActions;
	if (_numberOfActions < MIN_NUMBER_OF_ACTIONS)
	{
		std::cout << "number of states can't be negative number!" << std::endl;
		return;
	}

	inputFile >> _numberOfStates;
	if (_numberOfStates < 0)
	{
		std::cout << "number of states can't be negative number!" << std::endl;
		return;
	}

	if (_stateMachineType == StateMachineType::MEALY)
	{
		ReadMealyFromFile(inputFile);
	}
	else
	{
		ReadMooreFromFile(inputFile);
	}

	CreateFirstListOfEquivalenceClasses();
}

void CMinimizer::ReadMealyFromFile(std::ifstream& inputFile)
{
	for (int i = 0; i < _numberOfInputSignals; ++i)
	{
		std::vector<StateMachineState> statesLine(_numberOfStates);
		for (int j = 0; j < _numberOfStates; ++j)
		{
			inputFile >> statesLine[j].state >> statesLine[j].action;
		}

		_initialStateMachine.push_back(statesLine);
	}
}

void CMinimizer::ReadMooreFromFile(std::ifstream& inputFile)
{
	std::vector<int> vactorOfActions(_numberOfStates);
	for (int i = 0; i < _numberOfStates; ++i)
	{
		inputFile >> vactorOfActions[i];
	}

	for (size_t i = 0; i < _numberOfInputSignals; ++i)
	{
		std::vector<StateMachineState> statesLine(_numberOfStates);
		for (int j = 0; j < _numberOfStates; ++j)
		{
			inputFile >> statesLine[j].state;
			statesLine[j].action = vactorOfActions[j];
		}

		_initialStateMachine.push_back(statesLine);
	}
}

CMinimizer::~CMinimizer()
{
}

void CMinimizer::CreateFirstListOfEquivalenceClasses()
{
	_equivalenceClassComponents = CreateEquivalenceClassComponents();

	int id = 0;
	for (int i = 0; i < _equivalenceClassComponents.size(); ++i)
	{
		for (int j = 0; j < _equivalenceClassComponents.size(); ++j)
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

	// create table of states by using before created classes of equivalence
	for (int i = 0; i < _initialStateMachine.size(); ++i)
	{
		for (int j = 0; j < _initialStateMachine[i].size(); ++j)
		{
			int newId = GetEquivalenceClassIdByState(_currEquivalenceClasses, _initialStateMachine[i][j].state);
			if (newId != -1)
			{
				_equivalenceClassComponents[j].states[i] = newId;
			}
		}
	}
	std::cout << "START" << std::endl;
	//create new class of equivalence
	// fill/update table of states by using before created classes of equivalence
	// check classes of equivalence
	while (_prevEquivalenceClasses.size() != _currEquivalenceClasses.size())
	{
		_currEquivalenceClasses = GetNewVectorOfEquivalenceClasses(_currEquivalenceClasses);
		UpdateTableOfStates();
		std::cout << "CONTINUE" << std::endl;
	}

	std::cout << "STOP" << std::endl;

	CreateMinimizedStatesMachine();
}

std::vector<EquivalenceClass> CMinimizer::GetNewVectorOfEquivalenceClasses(const std::vector<EquivalenceClass>& equiveClasses)
{
	_prevEquivalenceClasses = equiveClasses;
	std::vector<EquivalenceClass> newEquivalenceClasses;
	//create new class of equivalence
	for (int i = 0; i < _prevEquivalenceClasses.size(); ++i)
	{
		int testState = *_prevEquivalenceClasses[i].states.begin();
		int currStateId = 0;
		if (newEquivalenceClasses.size() != 0)
		{ // lastId + 1;
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
					else // add new elem
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

	CreateMinimizedStatesMachine();

	return newEquivalenceClasses;
}

void CMinimizer::UpdateTableOfStates()
{
	// fill/update table of states by using before created classes of equivalence
	for (int i = 0; i < _initialStateMachine.size(); ++i)
	{
		for (int j = 0; j < _initialStateMachine[i].size(); ++j)
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
		for (int j = 0; j < _initialStateMachine.size(); ++j)
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
	for (int k = 0; k < equivalenceClasses.size(); ++k)
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

	/*for (int i = 0; i < _currEquivalenceClasses.size(); ++i)
	{
		for (auto elem : _currEquivalenceClasses[i].states)
		{
			std::cout << elem << " ";
		}

		std::cout << "|";
	}
	std::cout << std::endl;*/
	for (int j = 0; j < _numberOfInputSignals; ++j)
	{
		std::vector<StateMachineState> stateMachineStates(_currEquivalenceClasses.size());
		for (int i = 0; i < _currEquivalenceClasses.size(); ++i)
		{
			StateMachineState stateMachineState;
			int currState = *_currEquivalenceClasses[i].states.begin();
			stateMachineState.state = _equivalenceClassComponents[currState].states[j];
			stateMachineState.action = _equivalenceClassComponents[currState].actions[j];
			stateMachineStates[i] = stateMachineState;
		}

		minimilizeAutomat[j] = stateMachineStates;
	}

	//for (int i = 0; i < _numberOfInputSignals; ++i)
	//{
	//	for (int j = 0; j < _currEquivalenceClasses.size(); ++j)
	//	{
	//		std::cout << minimilizeAutomat[i][j].state << ":" << minimilizeAutomat[i][j].action << " ";
	//	}

	//	std::cout << std::endl;
	//}

	/*std::cout << std::endl;
	std::cout << std::endl;*/
}
