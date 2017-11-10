import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface SearchState {
    results: SearchResult[];
    loading: boolean;
}

export class Search extends React.Component<RouteComponentProps<{}>, SearchState> {

   /* constructor() {
        super();
        this.state = { forecasts: [], loading: true };

        fetch('api/SampleData/WeatherForecasts')
            .then(response => response.json() as Promise<WeatherForecast[]>)
            .then(data => {
                this.setState({ forecasts: data, loading: false });
            });
    }*/

    public render() {
        return <div>
            <h1>Search</h1>

            <p>Search for books.</p>

            <form action="/api/Search/SearchResult">
                <input type="text" name="searchtext" />
                <input type="submit" value="Search" />
            </form>
        </div>;
    }

    /*private static renderResultsTable(forecasts: WeatherForecast[]) {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.dateFormatted}>
                        <td>{forecast.dateFormatted}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }*/

}


interface SearchResult {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}