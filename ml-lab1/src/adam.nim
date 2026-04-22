import math, sequtils
import net

type
  ParamRef* = ref object
    data*: ptr seq[float]
    grad*: ptr seq[float]
  
  AdamOptimizer* = object
    learningRate*: float
    beta1*: float
    beta2*: float
    epsilon*: float
    t*: int
    m*: seq[seq[float]]
    v*: seq[seq[float]]
    params*: seq[ParamRef]

proc addParam*(opt: var AdamOptimizer, paramData: ptr seq[float], paramGrad: ptr seq[float]) =
  opt.params.add(ParamRef(data: paramData, grad: paramGrad))
  opt.m.add(newSeq[float](paramData[].len))
  opt.v.add(newSeq[float](paramData[].len))

proc initAdamOptimizer*(learningRate: float = 0.001, beta1: float = 0.9,
    beta2: float = 0.999, epsilon: float = 1e-8): AdamOptimizer =
  result = AdamOptimizer(
    learningRate: learningRate,
    beta1: beta1,
    beta2: beta2,
    epsilon: epsilon,
    t: 0,
    m: @[],
    v: @[],
    params: @[]
  )

proc step*(opt: var AdamOptimizer) =
  opt.t += 1
  
  for pIdx, param in opt.params:
    let
      grad = param.grad[]
      gradLen = grad.len
      beta1_t = opt.beta1 ^ opt.t
      beta2_t = opt.beta2 ^ opt.t

    for i in 0..<gradLen:
      opt.m[pIdx][i] = opt.beta1 * opt.m[pIdx][i] + (1 - opt.beta1) * grad[i]
      opt.v[pIdx][i] = opt.beta2 * opt.v[pIdx][i] + (1 - opt.beta2) * grad[i] * grad[i]      
      let m_hat = opt.m[pIdx][i] / (1 - beta1_t)
      let v_hat = opt.v[pIdx][i] / (1 - beta2_t)
      let update = m_hat / (sqrt(v_hat) + opt.epsilon)
      param.data[i] -= opt.learningRate * update

proc zeroGrad*(opt: var AdamOptimizer) =
  for param in opt.params:
    for i in 0..<param.grad[].len:
      param.grad[][i] = 0.0

proc collectParams*(layer: var Transform, opt: var AdamOptimizer) =
  case layer.kind
  of tkBasic:
    for i in 0..<layer.basic.weights.len:
      opt.addParam(
        addr layer.basic.weights[i],
        addr layer.basic.gradWeights[i]
      )
    opt.addParam(
      addr layer.basic.bias,
      addr layer.basic.gradBias
    )
  of tkRBF:
    for i in 0..<layer.rbf.centers.len:
      opt.addParam(
        addr layer.rbf.centers[i],
        addr layer.rbf.gradCenters[i]
      )
  of tkResidual:
    collectParams(layer.residual.module, opt)