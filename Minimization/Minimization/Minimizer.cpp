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
	for (int i = 0; i < _numberOfInputSignals; ++i)
	{
		std::vector<StateMachineState> statesLine(_numberOfStates);
		for (int j = 0; j < _numberOfStates; ++j)
		{
			inputFile >> statesLine[j].state >> statesLine[j].action;
			--statesLine[j].action;
		}

		_initialStateMachine.push_back(statesLine);
	}
}

void CMinimizer::ReadMooreFromFile(std::ifstream& inputFile)
{
	std::vector<int> vectorOfActions(_numberOfStates);
	for (int i = 0; i < _numberOfStates; ++i)
	{
		inputFile >> vectorOfActions[i];
		--vectorOfActions[i];
	}

	for (int i = 0; i < _numberOfInputSignals; ++i)
	{
		std::vector<StateMachineState> statesLine(_numberOfStates);
		for (int j = 0; j < _numberOfStates; ++j)
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

		AddNewEquivalenceClass(newEquivalenceClasses, currStateId, testState);
		for (auto it = _prevEquivalenceClasses[i].states.begin(); it != _prevEquivalenceClasses[i].states.end(); ++it)
		{
			int verifableState = *it;
			if (_equivalenceClassComponents[testState].states != _equivalenceClassComponents[verifableState].states)
			{
				DivideEquivalenceClass(newEquivalenceClasses, currStateId, verifableState);
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

void CMinimizer::DivideEquivalenceClass(std::vector<EquivalenceClass>& equivalenceClasses, int prevStateId, int verifableState)
{
	int newStateId = prevStateId + 1;
	bool isNewPositionFound = false;
	while (!isNewPositionFound)
	{
		if (newStateId < equivalenceClasses.size())
		{
			int nextState = *equivalenceClasses[newStateId].states.begin();
			if (_equivalenceClassComponents[verifableState].states == _equivalenceClassComponents[nextState].states)
			{
				equivalenceClasses[newStateId].states.insert(verifableState);
				_equivalenceClassComponents[verifableState].id = equivalenceClasses[newStateId].id;
				isNewPositionFound = true;
			}
		}
		else
		{
			AddNewEquivalenceClass(equivalenceClasses, newStateId, verifableState);
			isNewPositionFound = true;
		}

		newStateId++;
	}
}

void CMinimizer::AddNewEquivalenceClass(std::vector<EquivalenceClass>& equivalenceClasses, int newId, int newState)
{
	EquivalenceClass newEquivalenceClass;
	newEquivalenceClass.id = newId;
	newEquivalenceClass.states.insert(newState);
	_equivalenceClassComponents[newState].id = newEquivalenceClass.id;
	equivalenceClasses.push_back(newEquivalenceClass);
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
	for (int j = 0; j < _numberOfInputSignals; ++j)
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
		WriteMooreStateMachineFromFile(outputFile);
	}
	else
	{
		WriteMealyStateMachineFromFile(outputFile);
	}

	if (!outputFile.flush())
	{
		std::cout << "Failed to save data on disk" << std::endl;
		return;
	}
}

void CMinimizer::WriteMooreStateMachineFromFile(std::ofstream& outputFile)
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

void CMinimizer::WriteMealyStateMachineFromFile(std::ofstream& outputFile)
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

//void MakeSimpleDot()
//{
//	using Edge = std::pair<int, int>;
//	using Graph = boost::adjacency_list<boost::vecS,
//		boost::vecS, boost::directedS,
//		boost::property<boost::vertex_color_t,
//			boost::default_color_type>,
//		boost::property<boost::edge_weight_t, double>>;
//	const int VERTEX_COUNT = 15;
//	std::vector<Edge> edges = {
//		{ 0, 4 },
//		{ 0, 6 },
//		{ 0, 1 },
//		{ 1, 6 },
//		{ 1, 11 },
//		{ 2, 6 },
//		{ 2, 9 },
//		{ 2, 11 },
//		{ 3, 4 },
//		{ 4, 5 },
//		{ 5, 8 },
//		{ 6, 7 },
//		{ 7, 8 },
//		{ 8, 13 },
//		{ 9, 10 },
//		{ 10, 13 },
//		{ 11, 12 },
//		{ 12, 13 },
//		{ 13, 14 },
//	};
//	std::vector<double> weights(edges.size());
//	std::fill(weights.begin(), weights.end(), 1.0);
//	weights[1] = 0.5;
//	weights[2] = 2.5;
//	weights[3] = 4.3;
//	Graph graph(edges.begin(), edges.end(), weights.begin(),
//		VERTEX_COUNT);
//
//	boost::dynamic_properties dp;
//	dp.property("weight", boost::get(boost::edge_weight, graph));
//	dp.property("label", boost::get(boost::edge_weight, graph));
//	dp.property("node_id", boost::get(boost::vertex_index, graph));
//	std::ofstream ofs("test.dot");
//	boost::write_graphviz_dp(ofs, graph, dp);
//}
