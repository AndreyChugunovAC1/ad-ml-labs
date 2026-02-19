import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

df = pd.read_csv('rr_target_functions.csv')

full_dataset_value = None
if df.iloc[-1, 0] == -1 or df.iloc[-1, 0] == 'full' or len(df) % 30 != 0:
    full_dataset_value = df.iloc[-1, 1]
    df = df.iloc[:-1]

df['test_ratio'] = pd.to_numeric(df['test_ratio'])
df['empirical_risk'] = pd.to_numeric(df['empirical_risk'])

grouped = df.groupby('test_ratio')['empirical_risk']
means = grouped.mean()
stds = grouped.std()
counts = grouped.count()

ci = 1.96 * stds / np.sqrt(counts)

plt.figure(figsize=(10, 6))
plt.plot(means.index, means.values, 'b-', linewidth=2, label='avg_error')
plt.fill_between(means.index, 
                 means.values - ci, 
                 means.values + ci, 
                 alpha=0.3, color='blue', label='95% confidence interval')

if full_dataset_value is not None:
    plt.axhline(y=full_dataset_value, color='red', linestyle='--', 
                linewidth=2, label=f'full_dataset_emp_risk: {full_dataset_value:.3f}')

plt.xlabel('test_size')
plt.ylabel('emp_risk')
plt.grid(True, alpha=0.3)
plt.legend()
plt.tight_layout()
plt.show()