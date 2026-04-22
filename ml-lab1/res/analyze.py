import re
import random
import matplotlib.pyplot as plt
from collections import defaultdict

# ========= 1. PARSING =========

def parse_file(path):
    experiments = []

    with open(path, 'r') as f:
        lines = [line.strip() for line in f if line.strip()]

    i = 0
    while i < len(lines):
        line = lines[i]

        # матчим строку вида: "4 afLeakyReLU 16"
        match = re.match(r'(\d+)\s+(af\w+)\s+(\d+)', line)
        if match:
            depth = int(match.group(1))
            activation = match.group(2)
            size = int(match.group(3))

            i += 1
            curve = []

            # читаем числа до следующего блока
            while i < len(lines) and re.match(r'^[0-9.]+$', lines[i]):
                curve.append(float(lines[i]))
                i += 1

            experiments.append({
                "depth": depth,
                "activation": activation,
                "size": size,
                "curve": curve,
                "final_score": curve[-1] if curve else None
            })
        else:
            i += 1

    return experiments


# ========= 2. PLOTS =========

def plot_learning_curves(experiments, max_curves=10):
    plt.figure()

    exps = random.sample(experiments, max_curves)
    for exp in exps:
        label = f"{exp['depth']} {exp['activation']} {exp['size']}"
        plt.plot(exp['curve'], label=label)

    plt.title("Learning curves (sample)")
    plt.xlabel("Epoch")
    plt.ylabel("F1-score")
    plt.legend()
    plt.grid()
    plt.show()


def aggregate_and_plot(experiments, key, title):
    agg = defaultdict(list)

    for exp in experiments:
        if exp["final_score"] is not None:
            agg[exp[key]].append(exp["final_score"])

    xs = sorted(agg.keys())
    ys = [sum(agg[x]) / len(agg[x]) for x in xs]

    plt.figure()
    plt.plot(xs, ys, marker='o')
    plt.title(title)
    plt.xlabel(key)
    plt.ylabel("Mean final F1-score")
    plt.grid()
    plt.show()


def plot_activation(experiments):
    agg = defaultdict(list)

    for exp in experiments:
        if exp["final_score"] is not None:
            agg[exp["activation"]].append(exp["final_score"])

    names = list(agg.keys())
    values = [sum(v) / len(v) for v in agg.values()]

    plt.figure()
    plt.bar(names, values)
    plt.title("Quality vs Activation Function")
    plt.ylabel("Mean final F1-score")
    plt.grid(axis='y')
    plt.show()


# ========= 3. MAIN =========

if __name__ == "__main__":
    path = "res.txt"

    experiments = parse_file(path)

    # 1. Кривые обучения
    plot_learning_curves(experiments)

    # 2. Зависимость от функции активации
    plot_activation(experiments)

    # 3. Зависимость от размера слоя
    aggregate_and_plot(
        experiments,
        key="size",
        title="Quality vs Layer Size"
    )

    # 4. Зависимость от глубины
    aggregate_and_plot(
        experiments,
        key="depth",
        title="Quality vs Depth"
    )