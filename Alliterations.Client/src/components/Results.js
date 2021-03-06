import React from "react"
import { bindActionCreators } from "redux"
import { connect } from "react-redux"
import { actionCreators } from "../store/Results"

const Results = props => {
  if (props.results.length > 0) {
    const results = props.results.map(result => (
      <li className="list-group-item" key={result}>{result}</li>
    ))
    return (
      <div>
        <h1>Results</h1>
        <ul className="list-group">{results}</ul>
      </div>
    )
  }

  return <div />
}

export default connect(
  state => state.results,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Results)
