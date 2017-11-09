import * as  React from 'react';
import { RouteComponentProps } from 'react-router';
import {Line} from 'react-chartjs-2';

interface FetchBarChartData{
    BarchartData: BarchartData[];
    loading: boolean;
}

interface BarchartData {
    title: string;
    sales: number;
}

export class Chart extends React.Component<RouteComponentProps<{}>, > {
    constructor(props){
        super(props);
        this.state = {
           bookData: props.bookData
           
            }        
        }
    
    render(){
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

export default Chart;