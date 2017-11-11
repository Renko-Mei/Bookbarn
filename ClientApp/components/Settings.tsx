import * as React from 'react';
import { RouteComponentProps } from 'react-router';

export class Settings extends React.Component<RouteComponentProps<{}>, {}> {
  //state = {loading: true};

    componentDidMount() {
        // fetch("https://swapi.co/api/planets/5")
        //   .then(res => res.json())
        //   .then(
        //     planet => this.setState({ loading: false, planet }),
        //     error => this.setState({ loading: false, error })
        //   );

        //For now mocked data
        //settings => this.setState({loading:false, settings});
      }

      renderLoading() {
        return <div>Loading...</div>;
      }
    
      renderError() {
        return <div>I'm sorry! Please try again.</div>;
      }
      
      renderSettings(){
          return <div> 
            <h2>User Settings</h2>
            
            </div>;
      }

      public render() {
        return this.renderSettings();
        //   if (this.state.loading) {
      //     return this.renderLoading();
      //   } else if (this.state.settings) {
      //     return this.renderSettings();
      //   } else {
      //     return this.renderError();
      //     return this.renderSettings();
      //   }
      }
}
