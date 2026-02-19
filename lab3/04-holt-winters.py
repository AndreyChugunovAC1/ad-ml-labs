from dataget import read_data
from statsmodels.tsa.holtwinters import ExponentialSmoothing
import matplotlib.pyplot as plt
import numpy as np

temps = read_data("plots/temps.json")

train_size = int(0.8 * len(temps))
predictor = ExponentialSmoothing(
    temps[:train_size],
    trend='add',
    seasonal='add',
    seasonal_periods=365,
    initialization_method='estimated'
).fit()

temps_predicted = np.concatenate([
    predictor.fittedvalues,
    predictor.forecast(len(temps) - train_size)
])

plt.plot(temps)
plt.plot(temps_predicted)
plt.plot(np.array(temps) - np.array(temps_predicted))
plt.vlines(train_size, min(temps)-1, max(temps)+1, color='red')
plt.show()
