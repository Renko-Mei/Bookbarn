import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import Chart from './components/Chart';
import BookSaleChart from './components/BookSaleChart';

class App extends Component {
  constructor(){
    super();
    this.state={}        
  }
  
  componentWillMount(){
    this.getChartData();
  }
  

  getChartData(){
    //make ajx call here
    this.setState({
      chartData:{
        labels:['Computer Science', 'Kinesiology', 'Business', 'Physics', 'Gender Study'],
        datasets:[
            {
                label:'Discipline',
                data:[
                    10,20,20,30,40
                ]
            }
        ]
      },
      bookData:{
        labels:['January', 'February', 'March', 'April'],
        datasets:[{
          label: 'Book Sale Overtime',
          fill: false,
          lineTension: 0.1,
          backgroundColor: 'rgba(75,192,192,0.4)',
          borderColor: 'rgba(75,192,192,1)',
          borderCapStyle: 'butt',
          borderDash: [],
          borderDashOffset: 0.0,
          borderJoinStyle: 'miter',
          data:[65,59,80,81]
        }]
      }
    })
  }


  render() {
    return (
      <div className="App">
        <div className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
        </div>
         <Chart chartData={this.state.chartData} />
         <BookSaleChart bookData={this.state.bookData} />
      </div>
    );
  }
}

export default App;