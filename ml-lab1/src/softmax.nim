import math

type SoftArgMaxCrossEntropy* = object
  logitsCache: seq[float]
  probsCache: seq[float]

func softmax*(logits: seq[float]): seq[float] =
  let maxLogit = logits.max()
  var expSum = 0.0
  result = newSeq[float](logits.len)
  for i, logit in logits:
    result[i] = exp(logit - maxLogit)
    expSum += result[i]
  for i in 0..<result.len:
    result[i] /= expSum

proc forward*(loss: var SoftArgMaxCrossEntropy, logits: seq[float], target: int): float =
  loss.logitsCache = logits
  loss.probsCache = softmax(logits)
  let prob = max(loss.probsCache[target], 1e-15)
  result = -ln(prob)

proc backward*(loss: var SoftArgMaxCrossEntropy, target: int): seq[float] =
  result = newSeq[float](loss.probsCache.len)
  for i in 0..<loss.probsCache.len:
    result[i] = loss.probsCache[i]
  result[target] -= 1.0

proc forwardBatch*(loss: var SoftArgMaxCrossEntropy, logitsBatch: seq[seq[float]],
    targets: seq[int]): float =
  assert logitsBatch.len == targets.len
  result = 0.0
  for i in 0..<logitsBatch.len:
    let probs = softmax(logitsBatch[i])
    let prob = max(probs[targets[i]], 1e-15)
    result += -ln(prob)

  result /= float(logitsBatch.len)

proc backwardBatch*(loss: var SoftArgMaxCrossEntropy, logitsBatch: seq[seq[float]],
    targets: seq[int]): seq[seq[float]] =
  result = newSeq[seq[float]](logitsBatch.len)

  for i in 0..<logitsBatch.len:
    let probs = softmax(logitsBatch[i])
    result[i] = newSeq[float](probs.len)
    for j in 0..<probs.len:
      result[i][j] = probs[j]
    result[i][targets[i]] -= 1.0
    for j in 0..<probs.len:
      result[i][j] /= float(logitsBatch.len)