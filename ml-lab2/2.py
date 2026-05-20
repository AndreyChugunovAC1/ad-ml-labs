import numpy as np
import struct
import matplotlib.pyplot as plt
import torch
import torch.nn as nn
import torch.optim as optim
from torch.utils.data import DataLoader, TensorDataset
import copy
import os

# loading
def read_idx(filename):
    with open(filename, 'rb') as f:
        _, _, dims = struct.unpack('>HBB', f.read(4))
        shape = tuple(struct.unpack('>I', f.read(4))[0] for d in range(dims))
        return np.frombuffer(f.read(), dtype=np.uint8).reshape(shape)

# MNIST
X_train_mnist = read_idx('data/MNIST/train-images.idx3-ubyte')
y_train_mnist = read_idx('data/MNIST/train-labels.idx1-ubyte')
X_test_mnist = read_idx('data/MNIST/t10k-images.idx3-ubyte')
y_test_mnist = read_idx('data/MNIST/t10k-labels.idx1-ubyte')

# FashionMNIST
X_train_fashion = read_idx('data/FashionMNIST/train-images-idx3-ubyte')
y_train_fashion = read_idx('data/FashionMNIST/train-labels-idx1-ubyte')
X_test_fashion = read_idx('data/FashionMNIST/t10k-images-idx3-ubyte')
y_test_fashion = read_idx('data/FashionMNIST/t10k-labels-idx1-ubyte')

X_train_mnist = X_train_mnist.astype(np.float32) / 255.0
X_test_mnist = X_test_mnist.astype(np.float32) / 255.0
X_train_fashion = X_train_fashion.astype(np.float32) / 255.0
X_test_fashion = X_test_fashion.astype(np.float32) / 255.0

X_train_mnist = X_train_mnist[:, np.newaxis, :, :]
X_test_mnist = X_test_mnist[:, np.newaxis, :, :]
X_train_fashion = X_train_fashion[:, np.newaxis, :, :]
X_test_fashion = X_test_fashion[:, np.newaxis, :, :]

X_train_mnist_t = torch.tensor(X_train_mnist)
y_train_mnist_t = torch.tensor(y_train_mnist, dtype=torch.long)
X_test_mnist_t = torch.tensor(X_test_mnist)
y_test_mnist_t = torch.tensor(y_test_mnist, dtype=torch.long)

X_train_fashion_t = torch.tensor(X_train_fashion)
y_train_fashion_t = torch.tensor(y_train_fashion, dtype=torch.long)
X_test_fashion_t = torch.tensor(X_test_fashion)
y_test_fashion_t = torch.tensor(y_test_fashion, dtype=torch.long)

train_loader_mnist = DataLoader(TensorDataset(X_train_mnist_t, y_train_mnist_t), batch_size=64, shuffle=True)
test_loader_mnist = DataLoader(TensorDataset(X_test_mnist_t, y_test_mnist_t), batch_size=64, shuffle=False)

train_loader_fashion = DataLoader(TensorDataset(X_train_fashion_t, y_train_fashion_t), batch_size=64, shuffle=True)
test_loader_fashion = DataLoader(TensorDataset(X_test_fashion_t, y_test_fashion_t), batch_size=64, shuffle=False)

# 2 heads
class MultiHeadCNN(nn.Module):
    def __init__(self):
        super().__init__()
        self.shared_conv = nn.Sequential(
            nn.Conv2d(1, 32, kernel_size=3, padding=1),
            nn.ReLU(),
            nn.MaxPool2d(2),
            nn.Conv2d(32, 64, kernel_size=3, padding=1),
            nn.ReLU(),
            nn.MaxPool2d(2),
            nn.Flatten()
        )
        
        self.head1 = nn.Sequential(
            nn.Linear(64 * 7 * 7, 128),
            nn.ReLU(),
            nn.Linear(128, 10)
        )
        
        self.head2 = nn.Sequential(
            nn.Linear(64 * 7 * 7, 128),
            nn.ReLU(),
            nn.Linear(128, 10)
        )
    
    def forward(self, x, head='head1'):
        x = self.shared_conv(x)
        if head == 'head1':
            return self.head1(x)
        else:
            return self.head2(x)

def train_epoch(model, loader, optimizer, criterion, head='head1'):
    model.train()
    running_loss = 0.0
    correct = 0
    total = 0
    
    for images, labels in loader:
        optimizer.zero_grad()
        outputs = model(images, head)
        loss = criterion(outputs, labels)
        loss.backward()
        optimizer.step()
        
        running_loss += loss.item() * images.size(0)
        _, predicted = torch.max(outputs, 1)
        total += labels.size(0)
        correct += (predicted == labels).sum().item()
    
    return running_loss / total, correct / total

def evaluate(model, loader, criterion, head='head1'):
    model.eval()
    running_loss = 0.0
    correct = 0
    total = 0
    
    with torch.no_grad():
        for images, labels in loader:
            outputs = model(images, head)
            loss = criterion(outputs, labels)
            
            running_loss += loss.item() * images.size(0)
            _, predicted = torch.max(outputs, 1)
            total += labels.size(0)
            correct += (predicted == labels).sum().item()
    
    return running_loss / total, correct / total

def plot_curves(history1, history2, title_suffix):
    fig, axes = plt.subplots(2, 2, figsize=(14, 10))

    axes[0, 0].plot(history1['train_loss'], label='Train')
    axes[0, 0].plot(history1['test_loss'], label='Test')
    axes[0, 0].set_xlabel('Epoch')
    axes[0, 0].set_ylabel('Loss')
    axes[0, 0].set_title(f'MNIST Loss {title_suffix}')
    axes[0, 0].legend()
    axes[0, 0].grid(True)

    # accuracy 1    
    axes[0, 1].plot(history1['train_acc'], label='Train')
    axes[0, 1].plot(history1['test_acc'], label='Test')
    axes[0, 1].set_xlabel('Epoch')
    axes[0, 1].set_ylabel('Accuracy')
    axes[0, 1].set_title(f'MNIST Accuracy {title_suffix}')
    axes[0, 1].legend()
    axes[0, 1].grid(True)
    
    # loss 2
    axes[1, 0].plot(history2['train_loss'], label='Train')
    axes[1, 0].plot(history2['test_loss'], label='Test')
    axes[1, 0].set_xlabel('Epoch')
    axes[1, 0].set_ylabel('Loss')
    axes[1, 0].set_title(f'FashionMNIST Loss {title_suffix}')
    axes[1, 0].legend()
    axes[1, 0].grid(True)
    
    # accuracy 2
    axes[1, 1].plot(history2['train_acc'], label='Train')
    axes[1, 1].plot(history2['test_acc'], label='Test')
    axes[1, 1].set_xlabel('Epoch')
    axes[1, 1].set_ylabel('Accuracy')
    axes[1, 1].set_title(f'FashionMNIST Accuracy {title_suffix}')
    axes[1, 1].legend()
    axes[1, 1].grid(True)
    
    plt.tight_layout()
    plt.show()

print("[Stage 1] Training head 1 on MNIST")
print("-----------------------------------")

model = MultiHeadCNN()
criterion = nn.CrossEntropyLoss()
optimizer = optim.Adam(model.parameters(), lr=0.001)

history1_init = {'train_loss': [], 'test_loss': [], 'train_acc': [], 'test_acc': []}

for epoch in range(5):
    # градиенты head2 не считаются
    train_loss, train_acc = train_epoch(model, train_loader_mnist, optimizer, criterion, 'head1')
    test_loss, test_acc = evaluate(model, test_loader_mnist, criterion, 'head1')
    
    history1_init['train_loss'].append(train_loss)
    history1_init['test_loss'].append(test_loss)
    history1_init['train_acc'].append(train_acc)
    history1_init['test_acc'].append(test_acc)
    
    print(f"Epoch {epoch+1}: Train Loss={train_loss:.4f}, Acc={train_acc:.4f} | Test Loss={test_loss:.4f}, Acc={test_acc:.4f}")

torch.save({
    'shared_conv': model.shared_conv.state_dict(),
    'head1': model.head1.state_dict(),
    'head2': model.head2.state_dict()
}, 'saved_parameters.pth')

print("\n[Stage 2] Training second head with frozen backbone")
print("----------------------------------------------------")

checkpoint = torch.load('saved_parameters.pth')
model.shared_conv.load_state_dict(checkpoint['shared_conv'])
model.head1.load_state_dict(checkpoint['head1'])
model.head2.load_state_dict(checkpoint['head2'])

for param in model.shared_conv.parameters():
    param.requires_grad = False
for param in model.head1.parameters():
    param.requires_grad = False

optimizer_head2 = optim.Adam(model.head2.parameters(), lr=0.001)

history1_frozen = {'train_loss': [], 'test_loss': [], 'train_acc': [], 'test_acc': []}
history2_frozen = {'train_loss': [], 'test_loss': [], 'train_acc': [], 'test_acc': []}

for epoch in range(5):
    # head2, head1 не обучается
    train_loss_f, train_acc_f = train_epoch(model, train_loader_fashion, optimizer_head2, criterion, 'head2')
    test_loss_f, test_acc_f = evaluate(model, test_loader_fashion, criterion, 'head2')
    
    test_loss_m, test_acc_m = evaluate(model, test_loader_mnist, criterion, 'head1')
    train_loss_m, train_acc_m = evaluate(model, train_loader_mnist, criterion, 'head1')
    
    history2_frozen['train_loss'].append(train_loss_f)
    history2_frozen['test_loss'].append(test_loss_f)
    history2_frozen['train_acc'].append(train_acc_f)
    history2_frozen['test_acc'].append(test_acc_f)
    
    history1_frozen['train_loss'].append(train_loss_m)
    history1_frozen['test_loss'].append(test_loss_m)
    history1_frozen['train_acc'].append(train_acc_m)
    history1_frozen['test_acc'].append(test_acc_m)
    
    print(f"Epoch {epoch+1}:")
    print(f"  FashionMNIST: Train Loss={train_loss_f:.4f}, Acc={train_acc_f:.4f} | Test Loss={test_loss_f:.4f}, Acc={test_acc_f:.4f}")
    print(f"  MNIST (frozen): Test Acc={test_acc_m:.4f}")

plot_curves(history1_frozen, history2_frozen, "(Frozen Backbone)")

print("\n[Stage 3] Stage 2 + unfreezing backbone and fine-tuning")
print("----------------------------------------------")

for param in model.shared_conv.parameters():
    param.requires_grad = True
for param in model.head1.parameters():
    param.requires_grad = True

optimizer_all = optim.Adam(model.parameters(), lr=0.0001)

history1_unfrozen = {'train_loss': [], 'test_loss': [], 'train_acc': [], 'test_acc': []}
history2_unfrozen = {'train_loss': [], 'test_loss': [], 'train_acc': [], 'test_acc': []}

for epoch in range(5):
    train_loss_f, train_acc_f = train_epoch(model, train_loader_fashion, optimizer_all, criterion, 'head2')

    test_loss_f, test_acc_f = evaluate(model, test_loader_fashion, criterion, 'head2')
    test_loss_m, test_acc_m = evaluate(model, test_loader_mnist, criterion, 'head1')
    train_loss_m, train_acc_m = evaluate(model, train_loader_mnist, criterion, 'head1')
    
    history2_unfrozen['train_loss'].append(train_loss_f)
    history2_unfrozen['test_loss'].append(test_loss_f)
    history2_unfrozen['train_acc'].append(train_acc_f)
    history2_unfrozen['test_acc'].append(test_acc_f)
    
    history1_unfrozen['train_loss'].append(train_loss_m)
    history1_unfrozen['test_loss'].append(test_loss_m)
    history1_unfrozen['train_acc'].append(train_acc_m)
    history1_unfrozen['test_acc'].append(test_acc_m)
    
    print(f"Epoch {epoch+1}:")
    print(f"  FashionMNIST: Train Loss={train_loss_f:.4f}, Acc={train_acc_f:.4f} | Test Loss={test_loss_f:.4f}, Acc={test_acc_f:.4f}")
    print(f"  MNIST: Test Acc={test_acc_m:.4f}")

plot_curves(history1_unfrozen, history2_unfrozen, "(Unfrozen Fine-tuning)")

print("\n[Stage 4] Training from scratch with all layers unfrozen (no stage 2)")
print("--------------------------------------------------------")

model_fresh = MultiHeadCNN()
checkpoint = torch.load('saved_parameters.pth')
model_fresh.shared_conv.load_state_dict(checkpoint['shared_conv'])
model_fresh.head1.load_state_dict(checkpoint['head1'])
model_fresh.head2.load_state_dict(checkpoint['head2'])

for param in model_fresh.parameters():
    param.requires_grad = True

optimizer_fresh = optim.Adam(model_fresh.parameters(), lr=0.001)

history1_fresh = {'train_loss': [], 'test_loss': [], 'train_acc': [], 'test_acc': []}
history2_fresh = {'train_loss': [], 'test_loss': [], 'train_acc': [], 'test_acc': []}

for epoch in range(5):
    # FashionMNIST
    train_loss_f, train_acc_f = train_epoch(model_fresh, train_loader_fashion, optimizer_fresh, criterion, 'head2')
    test_loss_f, test_acc_f = evaluate(model_fresh, test_loader_fashion, criterion, 'head2')
    
    # MNIST
    test_loss_m, test_acc_m = evaluate(model_fresh, test_loader_mnist, criterion, 'head1')
    train_loss_m, train_acc_m = evaluate(model_fresh, train_loader_mnist, criterion, 'head1')
    
    history2_fresh['train_loss'].append(train_loss_f)
    history2_fresh['test_loss'].append(test_loss_f)
    history2_fresh['train_acc'].append(train_acc_f)
    history2_fresh['test_acc'].append(test_acc_f)
    
    history1_fresh['train_loss'].append(train_loss_m)
    history1_fresh['test_loss'].append(test_loss_m)
    history1_fresh['train_acc'].append(train_acc_m)
    history1_fresh['test_acc'].append(test_acc_m)
    
    print(f"Epoch {epoch+1}:")
    print(f"  FashionMNIST: Train Loss={train_loss_f:.4f}, Acc={train_acc_f:.4f} | Test Loss={test_loss_f:.4f}, Acc={test_acc_f:.4f}")
    print(f"  MNIST: Test Acc={test_acc_m:.4f}")

plot_curves(history1_fresh, history2_fresh, "(Unfrozen from start)")

print("\n[Stage 5] Final Comparison")
print("--------------------------")
print(f"Frozen backbone:        {history2_frozen['test_acc'][-1]:.4f}")
print(f"After unfreeze:         {history2_unfrozen['test_acc'][-1]:.4f}")
print(f"Unfrozen from start:    {history2_fresh['test_acc'][-1]:.4f}")

# Final plot
plt.figure(figsize=(12, 5))

plt.subplot(1, 2, 1)
epochs = range(1, 6)
plt.plot(epochs, history2_frozen['test_loss'], 'b-o', label='Frozen backbone')
plt.plot(epochs, history2_unfrozen['test_loss'], 'r-s', label='Unfrozen fine-tuning')
plt.plot(epochs, history2_fresh['test_loss'], 'g-^', label='Unfrozen from start')
plt.xlabel('Epoch')
plt.ylabel('Test Loss')
plt.title('FashionMNIST: Strategy Comparison')
plt.legend()
plt.grid(True)

plt.subplot(1, 2, 2)
plt.plot(epochs, history2_frozen['test_acc'], 'b-o', label='Frozen backbone')
plt.plot(epochs, history2_unfrozen['test_acc'], 'r-s', label='Unfrozen fine-tuning')
plt.plot(epochs, history2_fresh['test_acc'], 'g-^', label='Unfrozen from start')
plt.xlabel('Epoch')
plt.ylabel('Test Accuracy')
plt.title('FashionMNIST: Strategy Comparison')
plt.legend()
plt.grid(True)

plt.tight_layout()
plt.show()

print("\n[Stage 6] Random Initialization Experiments")
print("-------------------------------------------")

class FlexibleCNN(nn.Module):
    def __init__(self):
        super().__init__()
        self.conv1 = nn.Conv2d(1, 32, kernel_size=3, padding=1)
        self.relu1 = nn.ReLU()
        self.pool1 = nn.MaxPool2d(2)
        
        self.conv2 = nn.Conv2d(32, 64, kernel_size=3, padding=1)
        self.relu2 = nn.ReLU()
        self.pool2 = nn.MaxPool2d(2)
        
        self.flatten = nn.Flatten()
        
        self.fc1 = nn.Linear(64 * 7 * 7, 128)
        self.relu3 = nn.ReLU()
        self.fc2 = nn.Linear(128, 10)
    
    def forward(self, x):
        x = self.pool1(self.relu1(self.conv1(x)))
        x = self.pool2(self.relu2(self.conv2(x)))
        x = self.flatten(x)
        x = self.relu3(self.fc1(x))
        x = self.fc2(x)
        return x
    
    def freeze_conv_layers(self, num_layers):
        layers = [self.conv1, self.conv2]
        for i, layer in enumerate(layers):
            if i < num_layers:
                for param in layer.parameters():
                    param.requires_grad = False
            else:
                for param in layer.parameters():
                    param.requires_grad = True
    
    def get_trainable_params_count(self):
        return sum(p.numel() for p in self.parameters() if p.requires_grad)

def train_model_flexible(model, train_loader, test_loader, epochs=10, lr=0.001):
    criterion = nn.CrossEntropyLoss()
    trainable_params = filter(lambda p: p.requires_grad, model.parameters())
    optimizer = optim.Adam(trainable_params, lr=lr)
    
    history = {'train_loss': [], 'test_loss': [], 'train_acc': [], 'test_acc': []}
    
    for epoch in range(epochs):
        model.train()
        running_loss = 0.0
        correct = 0
        total = 0
        
        for images, labels in train_loader:
            optimizer.zero_grad()
            outputs = model(images)
            loss = criterion(outputs, labels)
            loss.backward()
            optimizer.step()
            
            running_loss += loss.item() * images.size(0)
            _, predicted = torch.max(outputs, 1)
            total += labels.size(0)
            correct += (predicted == labels).sum().item()
        
        train_loss = running_loss / total
        train_acc = correct / total
        
        model.eval()
        running_loss = 0.0
        correct = 0
        total = 0
        
        with torch.no_grad():
            for images, labels in test_loader:
                outputs = model(images)
                loss = criterion(outputs, labels)
                
                running_loss += loss.item() * images.size(0)
                _, predicted = torch.max(outputs, 1)
                total += labels.size(0)
                correct += (predicted == labels).sum().item()
        
        test_loss = running_loss / total
        test_acc = correct / total
        
        history['train_loss'].append(train_loss)
        history['test_loss'].append(test_loss)
        history['train_acc'].append(train_acc)
        history['test_acc'].append(test_acc)
        
        if (epoch + 1) % 2 == 0:
            print(f"  Epoch {epoch+1}: Train Loss={train_loss:.4f}, Acc={train_acc:.4f} | Test Loss={test_loss:.4f}, Acc={test_acc:.4f}")
    
    return history

configs = [
    {'name': '0 layers (all trainable)', 'frozen': 0},
    {'name': '1 layer (conv1 frozen)', 'frozen': 1},
    {'name': '2 layers (both conv layers frozen)', 'frozen': 2}
]

histories_random = {}
models_random = {}

for config in configs:
    print(f"\nConfig: {config['name']}")
    print("-------------------------------------------")
    
    model = FlexibleCNN()
    
    model.freeze_conv_layers(config['frozen'])
    trainable_params = model.get_trainable_params_count()
    total_params = sum(p.numel() for p in model.parameters())
    print(f"Trainable params: {trainable_params} / {total_params}")
    history = train_model_flexible(model, train_loader_fashion, test_loader_fashion, epochs=10, lr=0.001)
    
    histories_random[config['name']] = history
    models_random[config['name']] = model
    
    print(f"Final accuracy: {history['test_acc'][-1]:.4f}")

if histories_random['2 layers (both conv layers frozen)']['test_acc'][-1] < 0.3:
    print("\n[Warning] Model with 2 frozen layers performed poorly!")
    print("Reducing frozen layers to 1 (already tested above)")

print("\n[Stage 7] Final Visualization - All Random Initialization Experiments")
print("----------------------------------------------------------------------")

fig, axes = plt.subplots(2, 2, figsize=(16, 12))
colors = ['blue', 'red', 'green', 'orange', 'purple']
epochs = range(1, 11)

# Train Loss
ax = axes[0, 0]
for (name, history), color in zip(histories_random.items(), colors):
    ax.plot(epochs, history['train_loss'], label=name, color=color, linewidth=2)
ax.set_xlabel('Epoch')
ax.set_ylabel('Loss')
ax.set_title('Training Loss (FashionMNIST, Random Init)')
ax.legend(loc='upper right')
ax.grid(True, alpha=0.3)

# Test Loss
ax = axes[0, 1]
for (name, history), color in zip(histories_random.items(), colors):
    ax.plot(epochs, history['test_loss'], label=name, color=color, linewidth=2)
ax.set_xlabel('Epoch')
ax.set_ylabel('Loss')
ax.set_title('Test Loss (FashionMNIST, Random Init)')
ax.legend(loc='upper right')
ax.grid(True, alpha=0.3)

# Train Accuracy
ax = axes[1, 0]
for (name, history), color in zip(histories_random.items(), colors):
    ax.plot(epochs, history['train_acc'], label=name, color=color, linewidth=2)
ax.set_xlabel('Epoch')
ax.set_ylabel('Accuracy')
ax.set_title('Training Accuracy (FashionMNIST, Random Init)')
ax.legend(loc='lower right')
ax.grid(True, alpha=0.3)

# Test Accuracy
ax = axes[1, 1]
for (name, history), color in zip(histories_random.items(), colors):
    ax.plot(epochs, history['test_acc'], label=name, color=color, linewidth=2)
ax.set_xlabel('Epoch')
ax.set_ylabel('Accuracy')
ax.set_title('Test Accuracy (FashionMNIST, Random Init)')
ax.legend(loc='lower right')
ax.grid(True, alpha=0.3)

plt.suptitle('Comparison of Freezing Strategies with Random Initialization', fontsize=14, fontweight='bold')
plt.tight_layout()
plt.show()

# Summary table
print("\n[Stage 8] Summary Table - Random Initialization")
print("------------------------------------------------")
print(f"{'Configuration':<35} {'Test Accuracy':<20} {'Trainable Params':<20}")
print("-" * 75)
for name, history in histories_random.items():
    trainable = models_random[name].get_trainable_params_count()
    print(f"{name:<35} {history['test_acc'][-1]:.4f} ({history['test_acc'][-1]*100:.1f}%){'':<10} {trainable}")
