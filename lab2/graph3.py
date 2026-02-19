import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

df_l1 = pd.read_csv('l1_graph.csv')
df_l2 = pd.read_csv('l2_graph.csv')

def filter_extreme_values(df, threshold=1e10):
    return df[np.abs(df.select_dtypes(include=[np.number])) < threshold].dropna(axis=1, how='all')

df_l1_clean = filter_extreme_values(df_l1)
df_l2_clean = filter_extreme_values(df_l2)

fig, axes = plt.subplots(1, 2, figsize=(14, 6))

ax1 = axes[0]
for col in df_l1_clean.columns[1:]:
    ax1.plot(df_l1_clean['regularization_coef'], df_l1_clean[col], alpha=0.7, linewidth=1.5)

ax1.set_xscale('log')
ax1.set_xlabel('L1')
ax1.set_ylabel('weight')
ax1.grid(True, alpha=0.3)
ax1.legend(loc='best', fontsize='small')

ax2 = axes[1]
for col in df_l2_clean.columns[1:]:
    ax2.plot(df_l2_clean['regularization_coef'], df_l2_clean[col], alpha=0.7, linewidth=1.5)

ax2.set_xscale('log')
ax2.set_xlabel('L2')
ax2.set_ylabel('weight')
ax2.grid(True, alpha=0.3)
ax2.legend(loc='best', fontsize='small')

plt.tight_layout()
plt.show()

fig, axes = plt.subplots(1, 2, figsize=(14, 6))

ax1 = axes[0]
l1_norm = df_l1_clean.iloc[:, 1:].abs().sum(axis=1)
ax1.plot(df_l1_clean['regularization_coef'], l1_norm, 'b-', linewidth=2)
ax1.set_xscale('log')
ax1.set_xlabel('L1')
ax1.set_ylabel('weight norm')
ax1.grid(True, alpha=0.3)

ax2 = axes[1]
l2_norm = (df_l2_clean.iloc[:, 1:] ** 2).sum(axis=1)
ax2.plot(df_l2_clean['regularization_coef'], l2_norm, 'r-', linewidth=2)
ax2.set_xscale('log')
ax2.set_xlabel('L2')
ax2.set_ylabel('weight norm')
ax2.grid(True, alpha=0.3)

plt.tight_layout()
plt.show()