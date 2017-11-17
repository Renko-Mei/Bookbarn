import * as React from 'react'
import {RouteComponentProps} from 'react-router';
import BookSaleChart from './BookSaleChart';
//import Chart from './Chart';

export class SalesHistory extends React.Component<RouteComponentProps<{}>, {}>{
    constructor(){
        super();
    }
    componentWillMount(){
        //this.setState = this.getChartData();
    }

    payload = {
        labels:['January', 'February', 'March', 'April'],
        data:[65,59,80,100],
    }
    getChartData(){
    //make ajx call here - return payload  
    //return payload     
    }

    
    public render(){
        return <div><h2>Sales Viz</h2>
                <BookSaleChart labels={this.payload.labels}
                               data={this.payload.data} />
        </div>
    }
}