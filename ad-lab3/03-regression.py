from dataget import get_raw_data
from datetime import *
from math import sin
import numpy as np

data = get_raw_data("daily-minimum-temperatures.csv")
temps = data["Temp"]
dates = list(map(lambda x: datetime.fromtimestamp(x, timezone.utc), data["Date"]))

avg_temp = sum(temps) / len(temps)

# raw data
raw_features = np.array([
    np.array([
        date.day,
        date.month,
        date.year,
        (date.day - 1) // 7 + 1,
        date.timetuple().tm_yday,
        date.weekday(),
        date.isocalendar()[1]
    ])
    for date, temp in zip(dates, temps)
])
# print(raw_features[350:370])

# нормализация
from sklearn import preprocessing
normalizer = preprocessing.StandardScaler().fit(raw_features)
features_base = normalizer.transform(raw_features)

# +sin,cos
features = [
    np.concatenate([time_markers, np.sin(time_markers), np.cos(time_markers)])
    for time_markers in features_base
]
# print(features[:10])

# целевой
targets = [1 if x > avg_temp else -1 for x in temps]
# print(targets)

from sklearn.linear_model import RidgeClassifier
classifier = RidgeClassifier().fit(features, targets)

ok_n = sum(classifier.predict(features) == targets)
bad_n = sum(classifier.predict(features) != targets)
print(ok_n)
print(bad_n)
print(ok_n / (bad_n + ok_n))

