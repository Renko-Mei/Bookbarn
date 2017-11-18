import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Search } from './components/Search';
import { Settings } from './components/Settings';
import { SalesHistory } from './components/SalesHistory';

//import { SalesHistory } from '/components/SalesHistory';
//import { BookSaleChart} from './components/BookSaleChart';

export const routes = <Layout>
    <Route exact path='/' component={ Home } />
    <Route path='/counter' component={ Counter } />
    <Route path='/fetchdata' component={ FetchData } />
    <Route path='/user/settings' component= { Settings } />
    <Route path='/user/SalesHistory' component= { SalesHistory } />
    {/* <Route path='/user/SalesHistory' component={SalesHistory} /> */}
    <Route path='/search' component={ Search } />
</Layout>;
