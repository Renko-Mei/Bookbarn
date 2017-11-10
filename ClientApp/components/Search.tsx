import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface SearchState {
    results: SearchResult[];
    loading: boolean;
}

export class Search extends React.Component<RouteComponentProps<{}>, SearchState> {

    constructor() {
        super();
        this.state = { results: [], loading: true };

        fetch('api/SampleData/SearchResult')
            .then(response => response.json() as Promise<SearchResult[]>)
            .then(data => {
                this.setState({ results: data, loading: false });
            });
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Search.renderResultsTable(this.state.results);

        return <div>
            <h1>Search</h1>

            <p>Search for books.</p>

            <form action="Search">
                <input type="text" name="searchtext" />
                <input type="submit" value="Search" />
            </form>
            {contents}
        </div>;
    }

    private static renderResultsTable(results: SearchResult[]) {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Title</th>
                    <th>Author</th>
                </tr>
            </thead>
            <tbody>
                {results.map(result =>
                    <tr key={result.title}>
                        <td>{result.title}</td>
                        <td>{result.author}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }

   /* public getSearchData() {
        fetch('api/SampleData/SearchResult')
            .then(response => response.json() as Promise<SearchResult[]>)
            .then(data => {
                this.setState({ results: data, loading: false });
            });
    }*/

    //getSearchData: function(e) {

    //}

}


interface SearchResult {
    title: string;
    author: string;
}