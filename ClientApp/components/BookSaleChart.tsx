import * as  React from 'react';
import { RouteComponentProps } from 'react-router';
import {Line} from 'react-chartjs-2';

interface bookSalesData{
    labels:string,
    datasets: [
        {
            data:number[]
        }
    ];
}

interface BookSaleChartState{
    loadingData: boolean;
    bookSalesData:bookSalesData[];
}

export default class BookSaleChart extends React.Component<any, any> {
    constructor(){
        super();
        this.state = {
           loadingData:true,
           bookSalesData:[]
        };
    }
    
    getChartData(){
        //make ajx call here
            this.setState({
                bookSalesData:{
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
        return (        
            <div className="BookSale">
                <Line 
                data = {this.state.bookData}
                options={{
                    title:{
                        display:true,
                        text:"Sales Over Time"
                    },
                    legend:{
                        display:true
                    }
                }}
                /> 
            </div>
        )
    }
}