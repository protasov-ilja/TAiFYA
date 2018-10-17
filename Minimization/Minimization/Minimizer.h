#pragma once

//Mealy
//Moore
enum StateMachineType
{
	MOORE = 1,
	MEALY = 2
};

struct EquivalenceClass
{
	int id = -1;
	std::set<int> states;
};

struct EquivalenceClassComponent
{
	int id = -1;
	int originState;
	std::vector<int> states;
	std::vector<int> actions;
};

struct StateMachineState
{
	int state;
	int action;
};

class CMinimizer
{
public:
	CMinimizer(const std::string& fileName);
	void ShowMinimizedStateMachine(const std::string& outputFileName);
	~CMinimizer();

private:
	void ReadStateMachinInfoFromFile(const std::string& fileName);
	void CreateFirstTableOfEquivalentClasses();
	std::vector<EquivalenceClassComponent> CreateEquivalenceClassComponents();
	int GetEquivalenceClassIdByState(const std::vector<EquivalenceClass>& equivalenceClasses, int state);
	void CreateMinimizedStatesMachine();
	std::vector<EquivalenceClass> GetNewVectorOfEquivalenceClasses(const std::vector<EquivalenceClass>& equiveClasses);
	void ReadMealyFromFile(std::ifstream& inputFile);
	void ReadMooreFromFile(std::ifstream& inputFile);
	void UpdateTableOfStates();
	void MinimizeStateMachine();
	void AddNewEquivalenceClass(std::vector<EquivalenceClass>& equivalenceClasses, int newId, int newState);
	void WriteMooreStateMachineFromFile(std::ofstream& outputFile);
	void WriteMealyStateMachineFromFile(std::ofstream& outputFile);
	void DivideEquivalenceClass(std::vector<EquivalenceClass>& equivalenceClasses, int prevStateId, int verifableState);

	StateMachineType _stateMachineType;
	int _numberOfInputSignals;
	int _numberOfActions;
	int _numberOfStates;
	std::vector<std::vector<StateMachineState>> _initialStateMachine;
	std::vector<std::vector<StateMachineState>> _minimizedStateMachine;
	std::vector<EquivalenceClassComponent> _equivalenceClassComponents;
	std::vector<EquivalenceClass> _currEquivalenceClasses;
	std::vector<EquivalenceClass> _prevEquivalenceClasses;
};
