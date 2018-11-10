import React from "react"
import { bindActionCreators } from "redux"
import { connect } from "react-redux"
import { actionCreators } from "../store/Results"
import GenerationForm from "./GenerationForm"

const Search = props => (
  <div>
    <h1>Generate Alliterations</h1>
    <GenerationForm
      onSubmit={values => {
        props.makeRequest(values)
      }}
    />
  </div>
)

export default connect(
  state => state.results,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Search)
