import tables, parsecsv, strutils, math, random

type Data* = seq[seq[float]]

func initMapping(arr: openArray[string]): Table[string, float] =
  var t = initTable[string, float]()
  for i in arr:
    t[i] = float(t.len())
  return t

const mapRegion = initMapping(["Europe", "China", "USA", "RestOfWorld"])
const mapModel = initMapping(["X7", "X5", "X3", "i4", "3 Series", "iX", "5 Series", "MINI"])

proc splitFeatures(data: Data): (Data, seq[int]) =
  for row in data:
    result[0].add(@[row[0], row[1], row[3], row[4], row[5],
      row[6], row[7], row[8]#[ , row[9] ]#])
    result[1].add(int(row[2]))

proc readNumericData*(filename: string): (Data, seq[int]) =
  var parser: CsvParser
  parser.open("data.csv") # delimiter = ','
  parser.readHeaderRow()

  var data: Data
  while parser.readRow():
    let row = parser.row
    data.add(@[
      parseFloat(row[0]) * 12 + parseFloat(row[1]) - 1, # time
      mapRegion[row[2]], # class
      mapModel[row[3]], # class
      parseFloat(row[4]), # num
      # parseFloat(row[5]), # num
      parseFloat(row[6]), # num
      parseFloat(row[7]), # num
      parseFloat(row[8]), # num
      parseFloat(row[9]), # num
      parseFloat(row[10]) # num
    ])
  shuffle(data)
  return splitFeatures(data)

proc normalizeTrainTest*(train: var Data, test: var Data, pos: int) =
  var mean = 0.0
  var sigma = 0.0
  for row in train:
    mean += row[pos]
  mean /= float(train.len())
  for row in train:
    sigma += (row[pos] - mean)^2
  sigma = sqrt(sigma / float(train.len()))
  for row in train.mitems():
    row[pos] = (row[pos] - mean) / sigma
  for row in test.mitems():
    row[pos] = (row[pos] - mean) / sigma

