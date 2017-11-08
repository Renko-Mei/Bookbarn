import * as React from 'react';
import { RouteComponentProps } from 'react-router';

export class Search extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <h1>Search</h1>

            <p>Search for books.</p>

            <form asp-controller="Search" asp-action="SearchResults">
                <input type="text" name="searchtext" />
                <input type="submit" value="Search" />
            </form>
        </div>;
    }

}
