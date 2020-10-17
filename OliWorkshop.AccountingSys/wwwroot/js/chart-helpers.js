// the basic helper to create many chart grafic type and fit the models

function chartMetrics(element, labels, datasets) {
    var ctx = document.getElementById(element);
    var sets = MakeSets(datasets);
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: sets
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

function MakeSets(datasets) {
    return datasets.map(function (current) {
        if (current.border === undefined || typeof current.border !== 'number') {
            current.border = 1;
        }
        return {
                    label: current.label,
                    data: current.values,
                    backgroundColor: current.backgroundColor,
                    borderColor: current.borderColor,
                    borderWidth: current.border
        }
    });
}