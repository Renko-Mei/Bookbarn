import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
//import { NavLink } from 'react-router-dom';

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
            <Link to='/user/salesViz'>See Sales Visualization </Link>
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
