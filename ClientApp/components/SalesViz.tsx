import * as React from 'react'
import {RouteComponentProps} from 'react-router';
import BookSaleChart from './BookSaleChart';
//import Chart from './Chart';

export class SalesViz extends React.Component<RouteComponentProps<{}>, {}>{
    constructor(){
        super();
    }
    componentWillMount(){
        this.getChartData();
    }

    getChartData(){
    //make ajx call here
        this.setState({
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

    
    public render(){
        return <div><h2>Sales Viz</h2>
                <BookSaleChart/>
        </div>
    }
}