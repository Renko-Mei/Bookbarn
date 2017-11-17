import React, {Component} from 'react';
import {Line} from 'react-chartjs-2';

class Chart extends Component{
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