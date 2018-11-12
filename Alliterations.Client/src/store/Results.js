import { createQueryString } from "../utilities/queryString"
const requestResults = "REQUEST_RESULTS"
const receiveResults = "RECEIVE_RESULTS"
const initialState = { results: [], loading: false }

export const actionCreators = {
  makeRequest: values => {
    return async dispatch => {
      dispatch({ type: requestResults })

      const parameters = {}
      if (values.count) {
        parameters.count = values.count
      }
      if (values.startingCharacter) {
        parameters.startingCharacter = values.startingCharacter
      }

      const queryString = createQueryString(parameters)
      const url = `https://localhost:44320/api/alliterations${queryString}`
      const results = await fetch(url).then(response => response.json())

      dispatch({ type: receiveResults, results })
    }
  },
}

const actionHandlers = {
  [requestResults]: (state, action) => {
    return { ...state, results: [], loading: true }
  },
  [receiveResults]: (state, action) => {
    return { ...state, results: action.results, loading: false }
  },
}

export const reducer = (state, action) => {
  const actionHandler = actionHandlers[action.type]
  return actionHandler ? actionHandler(state, action) : state || initialState
}
