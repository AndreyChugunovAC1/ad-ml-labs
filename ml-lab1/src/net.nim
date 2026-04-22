import sequtils, math, sugar, random

type ActivationFunc* = enum
  afTanh
  afIdentity
  afLeakyReLU

func getActivationFunc(af: ActivationFunc): (x: float) -> float =
  case af
  of afTanh:
    return tanh
  of afIdentity:
    return (x: float) => x
  of afLeakyReLU:
    return func (x: float): float =
      if x > 0: x else: 0.01 * x

func getActivationFuncDer(af: ActivationFunc): (x: float) -> float =
  case af
  of afTanh:
    return func(x: float): float =
      let t = tanh(x) 
      1.0 - t * t
  of afIdentity:
    return (x: float) => 1.0
  of afLeakyReLU:
    return func(x: float): float =
      if x > 0: 1.0 else: 0.01

type
  TransformKind* = enum
    tkBasic
    tkRBF
    tkResidual

  BasicData* = ref object
    weights*: seq[seq[float]]
    bias*: seq[float]
    af*: ActivationFunc
    preActivationCache*: seq[float]
    gradWeights*: seq[seq[float]]
    gradBias*: seq[float]

  RBFData* = ref object
    centers*: seq[seq[float]]
    gamma*: float
    gradCenters*: seq[seq[float]]
  
  ResidualData* = ref object
    module*: Transform
    useShortcut*: bool

  Transform* #[ {.acyclic.} ]# = object
    case kind*: TransformKind
    of tkBasic:
      basic*: BasicData
    of tkRBF:
      rbf*: RBFData
    of tkResidual:
      residual*: ResidualData
    inputCache: seq[float]
    outputCache: seq[float]

  NeuralNetwork* = object
    layers*: seq[Transform]
    # TODO: different loss funcs

proc forward*(layer: var Transform, x: seq[float]): seq[float]
proc backward*(layer: var Transform, gradOutput: seq[float]): seq[float]

proc forwardBasic(layer: var BasicData, x: seq[float], cache: var seq[float]): seq[float] =
  let
    m = layer.weights.len
    n = layer.weights[0].len
  var z = newSeq[float](m)

  for i in 0..<m:
    z[i] = layer.bias[i]
    for j in 0..<n:
      z[i] += layer.weights[i][j] * x[j]

  layer.preActivationCache = z

  let af = getActivationFunc(layer.af)
  result = newSeq[float](m)
  for i in 0..<m:
    result[i] = af(z[i])
  cache = x

proc forwardRBF(layer: var RBFData, x: seq[float], cache: var seq[float]): seq[float] =
  let
    numCenters = layer.centers.len
    numFeatures = layer.centers[0].len
  result = newSeq[float](numCenters)

  for i in 0..<numCenters:
    var distSq = 0.0
    for j in 0..<numFeatures:
      let diff = x[j] - layer.centers[i][j]
      distSq += diff * diff
    result[i] = exp(-layer.gamma * distSq)
  cache = x

proc backwardBasic(layer: var BasicData, gradOutput: seq[float], x: seq[float]): seq[float] =
  let
    m = layer.weights.len
    n = layer.weights[0].len
    afd = getActivationFuncDer(layer.af)
  var preGrad = newSeq[float](m)

  for i in 0..<m:
    preGrad[i] = gradOutput[i] * afd(layer.preActivationCache[i])

  for i in 0..<m:
    for j in 0..<n:
      layer.gradWeights[i][j] = 0.0
    layer.gradBias[i] = 0.0

  for i in 0..<m:
    for j in 0..<n:
      layer.gradWeights[i][j] += preGrad[i] * x[j]
    layer.gradBias[i] += preGrad[i]

  result = newSeq[float](n)
  for j in 0..<n:
    for i in 0..<m:
      result[j] += layer.weights[i][j] * preGrad[i]

proc backwardRBF(layer: var RBFData, gradOutput: seq[float], x, y: seq[float]): seq[float] =
  let
    numCenters = layer.centers.len
    numFeatures = layer.centers[0].len

  for i in 0..<numCenters:
    for j in 0..<numFeatures:
      layer.gradCenters[i][j] = 0.0
  
  result = newSeq[float](numFeatures)

  for i in 0..<numCenters:
    let dLdy = gradOutput[i]
    let yi = y[i]
    let factor = 2.0 * layer.gamma * dLdy * yi
    
    for j in 0..<numFeatures:
      let diff = x[j] - layer.centers[i][j]
      result[j] += -factor * diff
      layer.gradCenters[i][j] += factor * diff

proc forwardResidual(layer: var ResidualData, x: seq[float], 
                     inputCache, outputCache: var seq[float]): seq[float] =
  let moduleOut = forward(layer.module, x)
  
  if layer.useShortcut:
    result = newSeq[float](x.len)
    for i in 0..<x.len:
      result[i] = x[i] + moduleOut[i]
  else:
    result = moduleOut

  inputCache = x
  outputCache = result

proc backwardResidual(layer: var ResidualData, gradOutput: seq[float], 
                      x, y: seq[float]): seq[float] =
  let gradFromModule = backward(layer.module, gradOutput)
  
  if layer.useShortcut:
    result = newSeq[float](gradOutput.len)
    for i in 0..<gradOutput.len:
      result[i] = gradOutput[i] + gradFromModule[i]
  else:
    result = gradFromModule

proc forward(layer: var Transform, x: seq[float]): seq[float] =
  layer.outputCache = case layer.kind
    of tkBasic:
      forwardBasic(layer.basic, x, layer.inputCache)
    of tkRBF:
      forwardRBF(layer.rbf, x, layer.inputCache)
    of tkResidual:
      forwardResidual(layer.residual, x, layer.inputCache, layer.outputCache)
  return layer.outputCache

proc backward(layer: var Transform, gradOutput: seq[float]): seq[float] =
  case layer.kind
  of tkBasic:
    return backwardBasic(layer.basic, gradOutput, layer.inputCache)
  of tkRBF:
    return backwardRBF(layer.rbf, gradOutput, layer.inputCache, layer.outputCache)
  of tkResidual:
    return backwardResidual(layer.residual, gradOutput, layer.inputCache, layer.outputCache)

proc forward*(net: var NeuralNetwork, x: seq[float]): seq[float] =
  var outt = x
  for i in 0..<net.layers.len:
    outt = net.layers[i].forward(outt)
  return outt

proc backward*(net: var NeuralNetwork, grad: seq[float]) =
  var g = grad
  for i in countdown(net.layers.len - 1, 0):
    g = net.layers[i].backward(g)