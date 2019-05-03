var donutOptions = {
    chart: {
        width: 380,
        type: 'donut',
    },
    dataLabels: {
        enabled: false
    },
    series: [44, 55, 13, 33],
    responsive: [{
        breakpoint: 480,
        options: {
            chart: {
                width: 200
            },
            legend: {
                show: false
            }
        }
    }],
    legend: {
        position: 'right',
        offsetY: 0,
        height: 230,
    }
};

var chart = new ApexCharts(
    document.querySelector("#donut-chart"),
    donutOptions
);

chart.render();