const TOKEN_KEY = 'token'

export const getTokenFromStorage = () => {
  return window.localStorage.getItem(TOKEN_KEY)
}

export const saveTokenToStorage = (token: string) => {
  return window.localStorage.setItem(TOKEN_KEY, token)
}

export const clearTokenFromStorage = () => {
  return window.localStorage.removeItem(TOKEN_KEY)
}
