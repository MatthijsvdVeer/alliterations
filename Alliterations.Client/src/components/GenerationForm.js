import React from "react"
import { Field, reduxForm } from "redux-form"

let GenerationForm = props => {
  const { handleSubmit } = props
  return (
    <form onSubmit={handleSubmit}>
      <div className="form-group">
        <label htmlFor="count">Count</label>
        <Field
          className="form-control"
          placeholder="Enter an amount"
          name="count"
          id="count"
          component="input"
          type="number"
        />
      </div>
      <div className="form-group">
        <label htmlFor="startingCharacter">Starting character</label>
        <Field
          className="form-control"
          placeholder="Enter a starting character"
          name="startingCharacter"
          id="startingCharacter"
          component="input"
          type="text"
          maxLength="1"
        />
      </div>
      <button type="submit" className="btn btn-primary">
        Generate
      </button>
    </form>
  )
}

GenerationForm = reduxForm({
  form: "generate",
})(GenerationForm)

export default GenerationForm
