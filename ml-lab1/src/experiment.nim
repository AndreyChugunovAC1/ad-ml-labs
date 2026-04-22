import sequtils, random, math
import net, softmax, adam, read

proc argmax(v: seq[float]): int =
  result = 0
  for i in 1..<v.len:
    if v[i] > v[result]:
      result = i

proc accuracy*(preds: seq[seq[float]], targets: seq[int]): float =
  var correct = 0
  for i in 0..<preds.len:
    if argmax(preds[i]) == targets[i]:
      correct += 1
  result = float(correct) / float(preds.len)

proc f1Score*(preds: seq[seq[float]], targets: seq[int], numClasses: int): float =
  var tp = newSeq[int](numClasses)
  var fp = newSeq[int](numClasses)
  var fn = newSeq[int](numClasses)

  for i in 0..<preds.len:
    let p = argmax(preds[i])
    let t = targets[i]
    if p == t:
      tp[t] += 1
    else:
      fp[p] += 1
      fn[t] += 1
  var f1sum = 0.0
  for c in 0..<numClasses:
    let precision =
      if tp[c] + fp[c] == 0: 0.0
      else: float(tp[c]) / float(tp[c] + fp[c])
    let recall =
      if tp[c] + fn[c] == 0: 0.0
      else: float(tp[c]) / float(tp[c] + fn[c])
    let f1 =
      if precision + recall == 0: 0.0
      else: 2 * precision * recall / (precision + recall)
    f1sum += f1
  result = f1sum / float(numClasses)

proc train*(net: var NeuralNetwork, trainX: Data, trainY: seq[int],
    testX: Data, testY: seq[int], epochs: int): float =
  var lossFn = SoftArgMaxCrossEntropy.default
  var opt = initAdamOptimizer(0.001)
  var f1: float

  for i in 0..<net.layers.len:
    collectParams(net.layers[i], opt)

  for epoch in 0..<epochs:
    var trainLoss = 0.0
    var preds: seq[seq[float]]

    for i in 0..<trainX.len:
      let outt = net.forward(trainX[i])
      preds.add(outt)

      trainLoss += lossFn.forward(outt, trainY[i])
      net.backward(lossFn.backward(trainY[i]))
      opt.step()
      opt.zeroGrad()

    trainLoss /= float(trainX.len)

    var testPreds: seq[seq[float]]
    for i in 0..<testX.len:
      testPreds.add(forward(net, testX[i]))
    f1 = f1Score(testPreds, testY, 8)
    # echo f1
  return f1

proc initWeights*(m, n: int): seq[seq[float]] =
  result = newSeqWith(m, newSeq[float](n))
  for i in 0..<m:
    for j in 0..<n:
      result[i][j] = rand(1.0) * 2.0 - 1.0

proc newTransform*(inSize, outSize: int, kind: TransformKind,
                   af: ActivationFunc): Transform =
  case kind
  of tkBasic:
    result = Transform(
      kind: tkBasic,
      basic: BasicData(
        weights: initWeights(outSize, inSize),
        bias: newSeq[float](outSize),
        af: af,
        gradWeights: newSeqWith(outSize, newSeq[float](inSize)),
        gradBias: newSeq[float](outSize)
      )
    )

  of tkRBF:
    result = Transform(
      kind: tkRBF,
      rbf: RBFData(
        centers: initWeights(outSize, inSize),
        gamma: 1.0,
        gradCenters: newSeqWith(outSize, newSeq[float](inSize))
      )
    )
  of tkResidual:
    result = Transform(
      kind: tkResidual,
      residual: ResidualData(
        # TODO:
        module: newTransform(inSize, outSize, tkRBF, af),
        useShortcut: true
      )
    )

proc makeModel*(inSize: int, spec: seq[(int, TransformKind, ActivationFunc)]): NeuralNetwork =
  var layers: seq[Transform]
  var prevSize = inSize
  for (size, kind, af) in spec:
    layers.add(newTransform(prevSize, size, kind, af))
    prevSize = size
  return NeuralNetwork(layers: layers)