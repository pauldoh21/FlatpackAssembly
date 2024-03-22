import matplotlib.pyplot as plt
import csv

import numpy as np

def create_bar_chart():
    # Data
    groups = ['Paper', 'Animation', 'Tracking']
    categories = ['Part ID', 'Location', 'Orientation', 'Progression']
    counts = {
        'Paper': [10, 19, 12, 8],
        'Animation': [14, 3, 10, 7],
        'Tracking': [12, 14, 8, 6]
    }
    
    # Create figure and axis
    fig, ax = plt.subplots()
    
    # Width of a single bar
    bar_width = 0.2
    
    # X locations for each group
    x = np.arange(len(categories))
    
    # Plot bars for each group
    for i, group in enumerate(groups):
        ax.bar(x + i * bar_width, counts[group], bar_width, label=group)
    
    # Set labels and title
    ax.set_xlabel('Categories')
    ax.set_ylabel('Issue Count')
    ax.set_title('Issue Count by Category and Group')
    ax.set_xticks(x + bar_width)
    ax.set_xticklabels(categories)
    ax.legend()
    
    # Show plot
    plt.tight_layout()
    plt.show()

# Call the function to create the bar chart
create_bar_chart()