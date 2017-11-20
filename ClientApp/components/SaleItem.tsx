import * as  React from 'react';
import { RouteComponentProps } from 'react-router';

export class SaleItem extends React.Component<RouteComponentProps<{}>, {}>{

  constructor(){
    super();
    this.state = {};

    fetch('saleitems')
        .then(response => response.json() as Promise<SaleItem[]>)
        .then(data => {
            this.setState({ results: data, loading: false });
            // console.log(data);
        });

  }

  public render(){
    return <div> <h1> testing! </h1> </div>;
  }

}
