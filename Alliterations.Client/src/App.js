import React from 'react'
import { Route } from 'react-router'
import Layout from './components/Layout'
import Generator from './components/Generator'

export default () => (
  <Layout>
    <Route exact path="/" component={Generator} />
  </Layout>
)
