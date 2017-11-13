import * as  React from 'react';
import { RouteComponentProps } from 'react-router';
import {Line} from 'react-chartjs-2';

interface bookData{
    labels:string,
    datasets: [
        {
            data:number[]
        }
    ];
}

interface BookSaleChartState{
    loadingData: boolean;
    bookData:bookData[];
}

export default class BookSaleChart extends React.Component<any, BookSaleChartState> {
    constructor(){
        super();
        this.state = {
           loadingData:true,
           bookData:[]
        };
    }
    
    public render(){
        return (
            this.state.loadingData ?
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