import React from "react"
import Search from "./Search"
import Results from "./Results"

const Generator = props => (
  <div className="row">
    <div className="col-sm-3" />
    <div className="col-sm-6">
      <div className="row">
        <Search />
      </div>
      <div className="row">
        <Results />
      </div>
    </div>
    <div className="col-sm-3" />
  </div>
)

export default Generator
