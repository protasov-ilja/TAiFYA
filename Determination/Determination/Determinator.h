#pragma once

struct StateMachineState
{
	StateMachineState(int index, std::set<int> sts)
	{
		states = sts;
		stateIndex = index;
	}

	StateMachineState()
	{
	}

	int stateIndex = -1;
	std::set<int> states;
};

class CDeterminator
{
public:
	CDeterminator(const std::string& inputfileName);
	~CDeterminator();
	void ShowDeterminatedStateMachine(const std::string& outputFileName);
	void CreateDotFile(const std::string& dotFileName);

private:
	void ReadStateMachineInfoFromFile(const std::string& fileName);
	void ReadStateMachineFromFile(std::ifstream& inputFile);
	void DeterminateStateMachine();
	void AddNewState(const  std::set<int>& initStates, std::queue<std::set<int>>& queue);
	void AddStateInFinalStates(StateMachineState state);
	std::vector<std::vector<StateMachineState>> GetEmptyStateMachine(int numberOfStates, int numberOfInputSignals);
	void InsertStatesInState(const std::set<int>& initStates, StateMachineState& state, int initStateIndex);

	int _numberOfInputSignals;
	int _numberOfStates;
	int _numberOfFinalStates;
	std::set<int> _finalStates;
	std::set<int> _determinatedFinalStates;
	std::vector<std::vector<StateMachineState>> _initialStateMachine;
	std::map<std::set<int>, int> _statesList;
	std::vector<std::vector<StateMachineState>> _determinatedStateMachine;
};

