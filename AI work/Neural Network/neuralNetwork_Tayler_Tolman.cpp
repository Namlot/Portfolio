// neural attempt 2.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <vector>
#include <random>

using namespace std;

float GetRandom() {
    float rando = (rand() % 100) / 100.0f;
    if (rand() % 2 == 0)
        rando -= 1;
    return rando;
}

float Sigmoid(float number) {
    return  1 / (1 + exp(-number));
}

float derSigmoid(float number) {
    return number * (1 - number);
}

 class MyMatrix {
 public:
    int rows;
    int columns;
    vector<vector<float>> pointData;
    MyMatrix() {};
    MyMatrix(const int numrows,const int numcolumns) {
        rows = numrows;
        columns = numcolumns;

        for (int i = 0; i < numrows; i++) {
            vector<float> howdydoo;
            pointData.push_back(howdydoo);
            for (int j = 0; j < numcolumns; j++) {
                pointData[i].push_back(0);
            }
        }
    }
    void DebugPrint() {
        for (int i = 0; i < rows; i++) {
            cout << endl;
            for (int j = 0; j < columns; j++)
                cout << pointData[i][j] << " ";
        }
    }
    void Randomizer() {
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                pointData[i][j] = GetRandom();
            }
        }
    }


     MyMatrix Transpose(MyMatrix target) {
        MyMatrix temp = MyMatrix(target.columns, target.rows);
        for (int i = 0; i < target.rows; i++) {
            for (int j = 0; j < target.columns; j++) {
                temp.pointData[j][i] = target.pointData[i][j];
            }
        }
        return temp;
     }

     MyMatrix CopyFromVector(vector<float> somevector) {
         MyMatrix temp = MyMatrix(somevector.size(), 1);
         for (int i = 0; i < somevector.size();  i++) {
             temp.pointData[i][0] = somevector[i];
         }
         return temp;
     }

     static MyMatrix SubtractMatrices(MyMatrix x, MyMatrix y) {
         MyMatrix temp = MyMatrix(x.rows, x.columns);
         for (int i = 0; i < x.rows; i++) {
             for (int j = 0; j < x.columns; j++) {
                 temp.pointData[i][j] = x.pointData[i][j] - y.pointData[i][j];
             }
         }
         return temp;
     }

     static MyMatrix AddMatrices(MyMatrix x, MyMatrix y) {
         MyMatrix temp = MyMatrix(x.rows, x.columns);
         for (int i = 0; i < x.rows; i++) {
             for (int j = 0; j < x.columns; j++) {
                 temp.pointData[i][j] = x.pointData[i][j] + y.pointData[i][j];
             }
         }
         return temp;
     }

     static MyMatrix MultiplyMatrices(MyMatrix x, MyMatrix y) {
         MyMatrix temp = MyMatrix(x.rows, y.columns);
         for (int i = 0; i < temp.rows; i++) {
             for (int j = 0; j < temp.columns; j++) {
                 float answer = 0;
                 for (int k = 0; k < x.columns; k++)
                     answer += x.pointData[i][k] * y.pointData[k][j];
                 temp.pointData[i][j] = answer;
             }
         }
         return temp;
     }
     
     static MyMatrix AddConstant(MyMatrix x, float y) {
         MyMatrix temp = MyMatrix(x.rows, x.columns);
         for (int i = 0; i < x.rows; i++) {
             for (int j = 0; j < x.columns; j++) {
                 temp.pointData[i][j] = x.pointData[i][j] + y;
             }
         }
         return temp;
     }
     static MyMatrix SubtractConstant(MyMatrix x, float y) {
         return AddConstant(x, -y);
     }
     static MyMatrix MultiplyConstant(MyMatrix x, float y) {
         MyMatrix temp = MyMatrix(x.rows, x.columns);
         for (int i = 0; i < x.rows; i++) {
             for (int j = 0; j < x.columns; j++) {
                 temp.pointData[i][j] = x.pointData[i][j] * y;
             }
         }
         return temp;
     }



};

 class NeuralNetwork {
 public:
     int inputnodes;
     int outputnodes;
     int hiddennodes;
     float learningRate;
     MyMatrix hiddenBias = MyMatrix(1, 1);
     MyMatrix outputBias = MyMatrix(1, 1);
     MyMatrix hiddenInputWeights = MyMatrix(1, 1);
     MyMatrix hiddenOutputWeights = MyMatrix(1, 1);

     NeuralNetwork(int numInput, int numHidden, int numOutput, float learning) {
          inputnodes = numInput;
          outputnodes = numOutput;
          hiddennodes = numHidden;
          learningRate = learning;
          hiddenInputWeights = MyMatrix(hiddennodes, inputnodes);
          hiddenInputWeights.Randomizer();
          hiddenOutputWeights = MyMatrix(outputnodes, hiddennodes);
          hiddenOutputWeights.Randomizer();
          hiddenBias = MyMatrix(hiddennodes, 1);
          hiddenBias.Randomizer();
          outputBias = MyMatrix(outputnodes, 1);
          outputBias.Randomizer();


     };

     vector<float> FeedForward(vector<float> input) {
         MyMatrix inputs = MyMatrix(1, 1);
         inputs = inputs.CopyFromVector(input);
         MyMatrix hiddenlayer = inputs.MultiplyMatrices(hiddenInputWeights, inputs);

         hiddenlayer = hiddenlayer.AddMatrices(hiddenlayer, hiddenBias);
         for (int i = 0; i < hiddenlayer.rows; i++) {
             for (int j = 0; j < hiddenlayer.columns; j++) {
                 hiddenlayer.pointData[i][j] = Sigmoid(hiddenlayer.pointData[i][j]);
             }
         }

         MyMatrix outputlayer = hiddenlayer.MultiplyMatrices(hiddenOutputWeights, hiddenlayer);
         outputlayer = outputlayer.AddMatrices(outputBias, outputlayer);
         for (int i = 0; i < outputlayer.rows; i++) {
             for (int j = 0; j < outputlayer.columns; j++) {
                 outputlayer.pointData[i][j] = Sigmoid(outputlayer.pointData[i][j]);
             }
         }

         vector<float> answer;
         for (int i = 0; i < outputlayer.rows; i++) {
             for (int j = 0; j < outputlayer.columns; j++) {
                 answer.push_back(outputlayer.pointData[i][j]);

             }
         }
         return answer;

     };

     void TrainNetwork(vector<float> input, vector<float> targetOutput) {
         vector<float> givenOutput = FeedForward(input);

         MyMatrix outputmatrix;
         outputmatrix = outputmatrix.CopyFromVector(givenOutput);
         MyMatrix targetmatrix = outputmatrix.CopyFromVector(targetOutput);
         MyMatrix outputErrors = outputmatrix.SubtractMatrices(targetmatrix, outputmatrix);
      //   outputErrors.DebugPrint();

         for (int i = 0; i < outputmatrix.rows; i++) {
             for (int j = 0; j < outputmatrix.columns; j++) {
                 outputmatrix.pointData[i][j] = derSigmoid(outputmatrix.pointData[i][j]);
             }
         }
         MyMatrix gradient = outputmatrix.MultiplyMatrices(outputErrors, outputmatrix);
         gradient = gradient.MultiplyConstant(gradient, learningRate);

         MyMatrix inputs = inputs.CopyFromVector(input);
         MyMatrix hiddenlayer = hiddenlayer.MultiplyMatrices(hiddenInputWeights, inputs);

         hiddenlayer = hiddenlayer.AddMatrices(hiddenlayer, hiddenBias);
         for (int i = 0; i < hiddenlayer.rows; i++) {
             for (int j = 0; j < hiddenlayer.columns; j++) {
                 hiddenlayer.pointData[i][j] = Sigmoid(hiddenlayer.pointData[i][j]);
             }
         }

         MyMatrix hiddenLayerTranspose = hiddenlayer.Transpose(hiddenlayer);
         MyMatrix hiddenOutputWeightChange = hiddenOutputWeightChange.MultiplyMatrices(gradient, hiddenLayerTranspose);

         hiddenOutputWeights = hiddenOutputWeights.AddMatrices(hiddenOutputWeights, hiddenOutputWeightChange);
         outputBias = outputBias.AddMatrices(gradient, outputBias);

         MyMatrix hiddenToOutputTranspose = hiddenOutputWeights.Transpose(hiddenOutputWeights);
         MyMatrix hiddenErrors = hiddenErrors.MultiplyMatrices(hiddenToOutputTranspose, outputErrors);

         MyMatrix hiddenGradient = hiddenlayer.AddConstant(hiddenlayer, 0);
         for (int i = 0; i < hiddenGradient.rows; i++) {
             for (int j = 0; j < hiddenGradient.columns; j++) {
                 hiddenGradient.pointData[i][j] = derSigmoid(hiddenGradient.pointData[i][j]);
             }
         }
         hiddenGradient = hiddenGradient.MultiplyMatrices(hiddenErrors, hiddenGradient);
         hiddenGradient = hiddenGradient.MultiplyConstant(hiddenGradient, learningRate);

         MyMatrix inputs_T = inputs.Transpose(inputs);
         MyMatrix weight_ih_deltas = hiddenGradient.MultiplyMatrices(hiddenGradient, inputs_T);

         hiddenInputWeights = hiddenInputWeights.AddMatrices(hiddenInputWeights, weight_ih_deltas);
         hiddenBias = hiddenBias.AddMatrices(hiddenBias, hiddenGradient);
        // cout << "aloha";
     };





     void DoNothing(vector<float>,vector<float>) {
         cout << "nothing";
     };
 };

 void PrintVector(vector<float> myvector) {
     for (int i = 0; i < myvector.size(); i++) {
         cout << myvector[i] << " ";
     }
     cout << endl;
 }

 vector<vector<float>> TrainingData = { {0,0,0,0,1}, {1,0,1,1,1}, {0,0,1,1,0},{1,0,0,0,0} };
 vector<vector<float>> SolutionData = { {1,0},{0,1},{0,1},{1,0} };
int main()
{
    cout << "Creating neural network for bit parity" << endl;
    NeuralNetwork myNetwork = NeuralNetwork(5, 4, 2,.1);
    vector<float> trainingData = { 1,0,1,1,1 };
    vector<float> trainingSolution = { 0,1 };
    cout << "Attempted guesses before training ({0,1} is even {1,0} is odd:" << endl << endl;
    for (int i = 0; i < TrainingData.size(); i++) {
        cout << " I think ";
        PrintVector(TrainingData[i]);
        cout << " should be: ";
        PrintVector(myNetwork.FeedForward(TrainingData[i]));
        cout << " it should actually be: ";
            PrintVector(SolutionData[i]);
            cout << endl;
    }

   
    cout << "TRAINING NOW:" << endl;
    for (int j = 0; j < 1000; j++) {
        for (int i = 0; i < TrainingData.size(); i++) {
         //   myNetwork.TrainNetwork(trainingData, trainingSolution);
            myNetwork.TrainNetwork(TrainingData[i], SolutionData[i]);
        }
    }
    cout << "Attempted guesses after training ({0,1} is even {1,0} is odd:" << endl << endl;
    for (int i = 0; i < TrainingData.size(); i++) {
        cout << " I think ";
        PrintVector(TrainingData[i]);
        cout << " should be: ";
        PrintVector(myNetwork.FeedForward(TrainingData[i]));
        cout << " it should actually be: ";
        PrintVector(SolutionData[i]);
        cout << endl;
    }

}

