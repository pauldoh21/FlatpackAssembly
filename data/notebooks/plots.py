import matplotlib.pyplot as plt
import csv

import numpy as np

def plot_line_chart(x_values, y_values_list, title="", x_label="", y_label=""):
    """
    Plot multiple lines on a single line chart, excluding zero values.

    Parameters:
    - x_values (list or array): The x-axis values.
    - y_values_list (list of lists or arrays): List containing y-values for each line.
    - title (str): The title of the chart (optional).
    - x_label (str): The label for the x-axis (optional).
    - y_label (str): The label for the y-axis (optional).
    """
    plt.figure(figsize=(12, 6))
    
    # Plot line chart
    plt.subplot(1, 2, 1)
    for y_values, label in zip(y_values_list, ['Paper', 'Animation', 'Tracking']):
        filtered_x_values = []
        filtered_y_values = []
        for x, y in zip(x_values, y_values):
            if y['avg'] != 0.01:
                print(y['avg'])
                filtered_x_values.append(x)
                filtered_y_values.append(y['avg'])
        plt.plot(filtered_x_values, filtered_y_values, marker='o', linestyle='-', label=label)
    plt.title("Average Issue Scores per Step")
    plt.xlabel(x_label)
    plt.ylabel(y_label)
    plt.legend()
    plt.grid(True)
    
    # Plot histograms
    plt.subplot(1, 2, 2)
    for y_values, label in zip(y_values_list, ['Paper', 'Animation', 'Tracking']):
        filtered_x_values = [value for value in calculate_density(y_values) if value != 0]
        plt.hist(filtered_x_values, bins=5, alpha=0.5, label=label, range=(1, 60))
    plt.title("Histogram")
    plt.xlabel(x_label)
    plt.ylabel("Frequency")
    plt.legend()

    plt.title('Frequency of Issues Across Steps')
    
    plt.tight_layout()
    plt.show()

def calculate_density(y_values):
    step_density = []
    for i in range(0, 59):
        if y_values[i]['avg'] != 0.01:
            to_add = i+1
            for j in range(y_values[i]['num']):
                step_density.append(to_add)

    #print(step_density)
    return step_density

def find_average(vals):
    return sum([v for v in vals])/len(vals)

def find_average_str(vals):
    return sum([float(v) for v in vals])/len(vals)

def plot_bar_chart(y_values_list):

    for y_values, label in zip(y_values_list, ['Paper', 'Animation', 'Tracking']):
        avg = find_average(y_values)
        plt.bar(label, y_values)
    plt.title("Average Total Score by Instructions")
    plt.xlabel("Instruction Type")
    plt.ylabel("Average Total Score")
    plt.show()   




y_values_paper = []
y_values_animation = []
y_values_tracking = []

with (open('allResultsCSV.csv', 'r') as f):
    csv_reader = csv.reader(f)

    for row in csv_reader:
        y_values_paper.append({'vals':[row[1],row[2],row[3]], 'avg': find_average_str(row[1:4]), 'num': len([x for x in [row[1],row[2],row[3]] if x != '0.01'])})
        y_values_animation.append({'vals':[row[5],row[6],row[7]], 'avg': find_average_str(row[5:8]), 'num': len([x for x in [row[5],row[6],row[7]] if x != '0.01'])})
        y_values_tracking.append({'vals':[row[9],row[10],row[11]], 'avg': find_average_str(row[9:12]), 'num': len([x for x in [row[9],row[10],row[11]] if x != '0.01'])})

scores_paper = []
scores_animation = []
scores_tracking = []
scores = [scores_paper, scores_animation, scores_tracking]

with (open('scoresCSV.csv', 'r') as f):
    csv_reader = csv.reader(f)

    for row in csv_reader:
        scores[int(row[0])-1].append({'part': row[1], 'location': row[2], 'orientation': row[3], 'progression': row[4], 'error': row[5]})

#plot_bar_chart_categories(scores)

#print(y_values_paper, '\n', y_values_animation, '\n', y_values_tracking)

# Example usage:
x_values = range(1, 60)  # Assuming x-values are from 1 to 59

plot_line_chart(x_values, [y_values_paper, y_values_animation, y_values_tracking], title="Comparison of Data", x_label="Steps", y_label="Average Issues Score")

total_values_paper = [79, 42, 40]
total_values_animation = [19, 36, 35]
total_values_tracking = [31, 28, 30]

#plot_bar_chart([total_values_paper, total_values_animation, total_values_tracking])