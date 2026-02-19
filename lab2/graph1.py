import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy.interpolate import make_interp_spline

df = pd.read_csv('gd_emp_risk.csv')
x, y = df['x'].values, df['y'].values

x_smooth = np.linspace(x.min(), x.max(), 300)
spline = make_interp_spline(x, y, k=3)
y_smooth = spline(x_smooth)

plt.figure(figsize=(8, 6))
plt.plot(x_smooth, y_smooth, 'b-', linewidth=2)
plt.scatter(x, y, color='red', zorder=3)
plt.xlabel('training size')
plt.ylabel('empirical risk')
plt.grid(True, alpha=0.3)
plt.tight_layout()
plt.show()