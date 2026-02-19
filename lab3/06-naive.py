from dataget import read_data
from statsmodels.tsa.holtwinters import ExponentialSmoothing
import matplotlib.pyplot as plt
import numpy as np

temps = read_data("plots/temps.json")

train_size = int(0.8 * len(temps))

temps_predicted_1 = temps[:train_size]
temps_predicted_2 = temps[:train_size]

# Скользящее окно в предыдущем периоде
period = 365
eps = 3
for i in range(train_size, len(temps) + 1000):
    center = i - period
    crop = temps_predicted_1[center - eps:center + eps]
    temps_predicted_1.append(sum(crop) / len(crop))

# Среднее по всем периодам
period = 365
eps = 3
for i in range(train_size, len(temps) + 1000):
    pos = i - period
    sm, cnt = 0, 0
    while pos > 0:
        sm += temps_predicted_2[pos]
        cnt += 1
        pos -= period
    temps_predicted_2.append(sm / cnt if cnt != 0 else 0)

plt.subplot(1, 2, 1)
plt.plot(temps)
plt.plot(temps_predicted_1)
plt.plot(np.array(temps) - np.array(temps_predicted_1)[:len(temps)])
plt.vlines(train_size, min(temps)-1, max(temps)+1, color='red')

plt.subplot(1, 2, 2)
plt.plot(temps)
plt.plot(temps_predicted_2)
plt.plot(np.array(temps) - np.array(temps_predicted_2)[:len(temps)])
plt.vlines(train_size, min(temps)-1, max(temps)+1, color='red')
plt.show()
