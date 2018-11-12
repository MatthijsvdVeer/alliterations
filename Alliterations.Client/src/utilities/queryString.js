export function createQueryString(parameters) {
  const keys = Object.keys(parameters)
  if (keys.count <= 0) {
    return ""
  }

  const value = keys.reduce((accumulator, value) => {
    if (!accumulator) {
      return `?${value}=${parameters[value]}`
    }

    return `${accumulator}&${value}=${parameters[value]}`
  }, "")

  return value
}
