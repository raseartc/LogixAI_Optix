    var dom = document.getElementById('container');
    var myChart = echarts.init(dom, null, {
      renderer: 'canvas',
      useDirtyRect: false
    });
    var app = {};
    
    var option;

    option = {
  dataset: {
    source: [
      ['score', 'Alarm Count', 'Alarm Details'],
      [$15, $15, '$10'],
      [$14, $14, '$09'],
      [$13, $13, '$08'],
      [$12, $12, '$07'],
      [$11, $11, '$06'],
    ]
  },
  grid: { containLabel: true },
  xAxis: { name: 'Alarm Count' },
  yAxis: { type: 'category' },
  visualMap: {
    orient: 'horizontal',
    left: 'center',
    min: 10,
    max: 300,
    text: ['High Count', 'Low Count'],
    // Map the score column to color
    dimension: 0,
    inRange: {
      color: ['#65B581', '#FFCE34', 'red']
    }
  },
  series: [
    {
      type: 'bar',
      encode: {
        // Map the "amount" column to X axis.
        x: 'Alarm Count',
        // Map the "product" column to Y axis
        y: 'Alarm Details'
      }
    }
  ]
};

    if (option && typeof option === 'object') {
      myChart.setOption(option);
    }

    window.addEventListener('resize', myChart.resize);