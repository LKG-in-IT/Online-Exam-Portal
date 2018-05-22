var canvas = document.getElementById("barChart");
var ctx = canvas.getContext('2d');

// Global Options:
 Chart.defaults.global.defaultFontColor = 'black';
 Chart.defaults.global.defaultFontSize = 16;

var data = {
    labels: ["She returns it ", "She keeps it"],
      datasets: [
        {
            fill: true,
            backgroundColor: [
                'black',
                'white'],
            data: [5, 95],
// Notice the borderColor 
            borderColor:	['black', 'black'],
            borderWidth: [2,2]
        }
    ]
};

// Notice the rotation from the documentation.

var options = {
        title: {
                  display: true,
                  text: 'What happens when you lend your favorite t-shirt to a girl ?',
                  position: 'top'
              },
        rotation: -0.7 * Math.PI
};


// Chart declaration:
var myBarChart = new Chart(ctx, {
    type: 'pie',
    data: data,
    options: options
});

// Fun Fact: I've lost exactly 3 of my favorite T-shirts and 2 hoodies this way :|