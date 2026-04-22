from scipy.fft import fft, fftfreq, ifft
import numpy as np
import matplotlib.pyplot as plt
from dataget import *

data = get_raw_data("daily-minimum-temperatures.csv")
temps = np.array(list(data["Temp"]))

# тренд (скользящее окно):
period = 366
trend = [sum(temps[i:i + period]) / period for i in range(len(temps) - period)]
delta_sizes = len(temps) - len(trend)
temps = temps[period//2:-period//2]

write_data("plots/dates.json", list(data["Date"])[period//2:-period//2])
write_data("plots/trend.json", trend)
write_data("plots/temps.json", temps)

# +- сезонность:
temps_no_trend = np.array(temps) - trend
tempsf = fft(temps_no_trend)
temps_fft_crop_ifft = np.real(ifft(np.array([x * int(np.abs(x) > 500) for x in tempsf])))

write_data("plots/seasonal.json", temps_fft_crop_ifft)

# ошибка:
noise = temps_no_trend - temps_fft_crop_ifft

write_data("plots/error.json", temps_fft_crop_ifft)

# графики
plt.subplot(1, 2, 1)
plt.title("")
plt.ylim(-10, 40)
plt.plot(temps)
plt.plot(trend)
plt.plot(temps_fft_crop_ifft + trend, color='red')
plt.legend(["оригинал", "тренд", "сезонность"])

plt.subplot(1, 2, 2)
plt.title("")
plt.ylim(-10, 40)
plt.plot(noise)
plt.legend(["ошибка"])

plt.show()
