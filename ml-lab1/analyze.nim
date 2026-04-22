include prelude
import algorithm

var res: seq[(string, float)]
for line in lines("raw_res_3.txt"):
  let parts = line.splitWhitespace()
  res.add((parts[0], parseFloat(parts[1])))

res.sort(proc(x, y: (string, float)): auto = cmp(y[1], x[1]))
for i, val in res:
  echo i, " :: ", val[1], " :: ", val[0]
