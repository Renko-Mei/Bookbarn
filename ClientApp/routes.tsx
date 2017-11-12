import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Settings } from './components/Settings';
import { SalesViz } from '/components/SalesViz';

export const routes = <Layout>
    <Route exact path='/' component={ Home } />
    <Route path='/counter' component={ Counter } />
    <Route path='/fetchdata' component={ FetchData } />
    <Route path='/user/settings' component= { Settings } />
    <Route path='/user/salesViz' component={SalesViz} />
</Layout>;
