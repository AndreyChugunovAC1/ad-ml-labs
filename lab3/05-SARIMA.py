from statsmodels.tsa.statespace.sarimax import SARIMAX
from statsmodels.tsa.arima.model import ARIMA
from dataget import read_data
import matplotlib.pyplot as plt
import numpy as np

temps = read_data("plots/temps.json")

delta = round(365 // 12)
temps = [sum(temps[i:i+delta])/delta for i in range(0, len(temps), delta)]

train_size = int(0.8 * len(temps))

p, d, q = 1, 1, 1
P, D, Q, s = 1, 1, 1, 365 / delta
predictor = SARIMAX(
    temps[:train_size],
    order=(p, d, q),
    seasonal_order=(P, D, Q, s),
    enforce_stationarity=False,
    enforce_invertibility=False
)

results = predictor.fit()

temps_predicted = np.concatenate([
    results.get_prediction(start=0, end=train_size).predicted_mean,
    results.forecast(len(temps) - train_size)
])

plt.plot(temps)
plt.plot(temps_predicted)
plt.plot(np.array(temps) - np.array(temps_predicted)[:-1])
plt.vlines(train_size, min(temps)-1, max(temps)+1, color='red')
plt.show()
