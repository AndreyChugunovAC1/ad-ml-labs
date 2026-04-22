import random
import read, net, experiment
import random, strutils

randomize()
let (features, targets) = readNumericData("data.csv")

let n = features.len()
let m = features[0].len()
let nTrain = int(float(n) * 0.8)
let nTest = n - nTrain

var trainFeatures = features[0..<nTrain]
let trainTargets = targets[0..<nTrain]
var testFeatures = features[nTrain..^1]
let testTargets = targets[nTrain..^1]

for i in 0..<m:
  normalizeTrainTest(trainFeatures, testFeatures, i)

let inputSize = trainFeatures[0].len
let outputSize = 8

# experiments
for depth in 2..6:
  for af in ActivationFunc.items():
    var spec: seq[(int, TransformKind, ActivationFunc)] = @[]
    for d in 1..<depth:
      spec.add((inputSize, tkResidual, af))
    spec.add((outputSize, tkRBF, af))
    var nett = makeModel(inputSize, spec)
    echo depth, " ", $af, " ", train(nett, trainFeatures, trainTargets, testFeatures, testTargets, 10)

