import * as  React from 'react';
import { RouteComponentProps } from 'react-router';
import {Line} from 'react-chartjs-2';

interface IBookSaleProp{
    labels: string[],
    data: number[]
}

export default class BookSaleChart extends React.Component<IBookSaleProp, any> {
    constructor(props : IBookSaleProp){
        super(props);
        // not expected to change states so no states is required.
    }

    public render(){
        return (        
            <div className="BookSale">
                <Line 
                data= {{
                    labels: this.props.labels,
                    datasets: [{
                        data: this.props.data,
                        backgroundColor: "rgba(75,192,192,0.4)",
                        borderColor: "rgba(75,192,192,1)",
                        fill: false,
                        label: "Sales Over Time"
                    }]
                }}
                /> 
            </div>
        )
    }
}