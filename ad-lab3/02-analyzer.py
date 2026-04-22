from dataget import read_data
from statsmodels.tsa.stattools import adfuller
from statsmodels.graphics.tsaplots import plot_acf
import matplotlib.pyplot as plt
from scipy.special import boxcox
import numpy as np

temps = np.array(read_data("plots/temps.json"))

# ====
plt.subplot(2, 3, 1)
plt.plot(temps)
plt.legend(["оригинал"])

# ====
plt.subplot(2, 3, 2)
plt.plot(np.log(temps + 1))
plt.legend(["log"])

# ====
plt.subplot(2, 3, 3)
plt.plot([temps[i + 1] - temps[i] for i in range(len(temps) - 1)])
plt.legend(["дифф."])

# ====
plt.subplot(2, 3, 4)
days = 7
rng = range(0, len(temps) - days, days)
temps_weekly = np.array([sum(temps[i:i + days]) / days for i in rng])
plt.plot(rng, temps_weekly)
plt.legend(["усред. за неделю"])

# ====
plt.subplot(2, 3, 5)
plt.plot(boxcox(temps_weekly + 1, 0.25))
plt.plot(boxcox(temps_weekly + 1, 0.5))
plt.plot(boxcox(temps_weekly + 1, -0.25))
plt.legend(["λ=0.25", "λ=0.5", "λ=-0.25"])

plt.show()

result = adfuller(temps)
print('p-value: %f' % result[1])

plt.subplot(1, 2, 1)
plot_acf(temps)
plt.show()

