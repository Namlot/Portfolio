// genetic algo.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <vector>
#include <stdlib.h> 
#include <time.h>  
#include <chrono>

using namespace std;
int codeSize;
int POPULATIONMAXSIZE = 1000;
const string alphabetSoup = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
string code = "htj5jdjej5j";

int FitnessScore(string);
void PrintPop(vector<string> pop);

vector<string> TerminateUnderperformers(vector<string> pop) {
    bool noneToDelete = false;
    do  {
        noneToDelete = true;
        float sumOfFitness = 0;
        int maxScore = 0;
        for (string s : pop) {
            int currentScore = FitnessScore(s);
            sumOfFitness += currentScore;
            if (currentScore > maxScore)
                maxScore = currentScore;
            
        }
       // cout << sumOfFitness << endl;
        float averageFitness = sumOfFitness / pop.size();
       // cout << averageFitness << endl;
        for (int i = 0; i < pop.size(); i++) {
            if (FitnessScore(pop.at(i)) < maxScore) {
                pop.erase(pop.begin() + i);
                i--;
                noneToDelete = false;
            }

        }
    } while ((pop.size() > POPULATIONMAXSIZE && !noneToDelete));
   // PrintPop(pop);
    return pop;
}

void PrintPop(vector<string> pop) {
    for(string s : pop)
    {
        cout << s << " : " << FitnessScore(s) << endl;
    }
}

vector<string> GetInitialPop() {
    vector<string> initialPop;
    
    srand(time(NULL));
    for (int i = 0; i < POPULATIONMAXSIZE; i++) {
        string randomWord = "";
        for (int j = 0; j < codeSize; j++) {
            int guess = rand() % alphabetSoup.length();
            randomWord += alphabetSoup[guess];

        }
        initialPop.push_back(randomWord);
    }
    cout << code << endl;
   // PrintPop(initialPop);
    return initialPop;
   
}

int FitnessScore(string test) {
    int score = 0;
    for (int i = 0; i < test.length(); i++) {
        if (test[i] == code[i])
            score++;
    }
    return score;
}
int Algorithm(vector<string> currentPop);
int main()
{
    cout << "Please enter in your code: (valid characters are a-z, A-Z, and 0-9)" << endl;
    cin >> code;
    auto start = std::chrono::high_resolution_clock::now();
    codeSize = code.length();
    vector<string> initialPopulation = GetInitialPop();
    initialPopulation = TerminateUnderperformers(initialPopulation);
    int timesLooped = Algorithm(initialPopulation);
    auto finish = std::chrono::high_resolution_clock::now();
    cout << "It took " << timesLooped << " generations to solve it" << endl;
    auto elapsed = finish - start;
    cout << "The time it took is: " << elapsed.count()/1000000000.0 << " Seconds" << endl;
    cout << "Press any key to continue";
    getchar();
    getchar();
}

vector<string> Crossover(vector<string> pop);
vector<string> Mutate(vector<string> pop);

vector<string> SmartCrossOver(vector<string> pop) {
    float sumOfFitness = 0;
    for (string s : pop) {
        sumOfFitness += FitnessScore(s);
    }
    //cout << sumOfFitness << endl;
    float averageFitness = sumOfFitness / pop.size();

    int currentPopSize = pop.size();
    for (int j = 0; j < currentPopSize; j++) {
        int randomIndex1 = rand() % pop.size();
        int randomIndex2 = rand() % pop.size();
        if (randomIndex1 != randomIndex2) {
            string child;
            bool odd = rand() % 1;
            for (int i = 0; i < code.size(); i++) {
                if (odd == 1) {
                    child += pop.at(randomIndex1)[i];
                }
                else {
                    child += pop.at(randomIndex2)[i];
                }
                odd = !odd;
            }
            if(FitnessScore(child) > averageFitness)
            pop.push_back(child);
        }

    }
    return pop;
}

int Algorithm(vector<string> currentPop) {
    
    int loops = 0;
    while (true) {
        loops++;
        //Check if I have the answer yet
        for (string s : currentPop) {
            if (s == code) {
                cout << "Gotcha! your code is: " << s << endl;
                return loops;
            }
        }
        //Then do crossover
       // currentPop = Crossover(currentPop);
        currentPop = SmartCrossOver(currentPop);
        //PrintPop(currentPop);
        //Then mutate
        currentPop = Mutate(currentPop);
        //Then terminate
        currentPop = TerminateUnderperformers(currentPop);
        //PrintPop(l);
        //cout << loops << endl;
    } 
}

vector<string> Crossover(vector<string> pop) {
    int currentPopSize = pop.size();
    for (int j = 0; j < currentPopSize; j++) {
        int randomIndex1 = rand() % pop.size();
        int randomIndex2 = rand() % pop.size();
        if (randomIndex1 != randomIndex2) {
            string child;
            bool odd = rand() % 1;
            for (int i = 0; i < code.size(); i++) {
                if (odd == 1) {
                    child += pop.at(randomIndex1)[i];
                }
                else {
                    child += pop.at(randomIndex2)[i];
                }
                odd = !odd;
            }
            pop.push_back(child);
        }

    }
    return pop;
}


vector<string> Mutate(vector<string> pop) {
    int currentPopSize = pop.size();
    float sumOfFitness = 0;
    int maxScore = 0;
    for (string s : pop) {
        int currentScore = FitnessScore(s);
        sumOfFitness += currentScore;
        if (currentScore > maxScore)
            maxScore = currentScore;

    }
        

    for (int j = 0; j < currentPopSize; j++) {
        int randomIndex1 = rand() % pop.size();
       
        string child = pop.at(randomIndex1);
        int mutatationIndex = rand() % code.size();
        child[mutatationIndex] = alphabetSoup[rand() % alphabetSoup.size()];
       
        
            
        if(FitnessScore(child) >= maxScore)
        pop.push_back(child);
        

    }
    return pop;
}