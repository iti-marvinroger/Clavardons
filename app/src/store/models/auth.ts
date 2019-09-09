import { Action, action, Thunk, thunk } from 'easy-peasy'
import { StoreModel } from '.'

export interface AuthModel {
  loading: boolean
  loggedIn: boolean
  token: string
  user: {
    id: string
    name: string
  }

  updateLoading: Action<AuthModel, boolean>
  updateLoggedIn: Action<AuthModel, boolean>
  updateToken: Action<AuthModel, string>
  updateUser: Action<AuthModel, { id: string; name: string }>

  loginWithName: Thunk<AuthModel, string, unknown, StoreModel>
  loginWithToken: Thunk<AuthModel, string, unknown, StoreModel>
}

const authModel: AuthModel = {
  loading: false,
  loggedIn: false,
  token: '',
  user: {
    id: 'id',
    name: 'John DOE',
  },

  updateLoading: action((state, payload) => {
    state.loading = payload
  }),
  updateLoggedIn: action((state, payload) => {
    state.loggedIn = payload
  }),
  updateToken: action((state, payload) => {
    state.token = payload
  }),
  updateUser: action((state, payload) => {
    state.user = payload
  }),

  loginWithName: thunk(async (actions, name) => {
    actions.updateUser({ id: 'id', name })
    actions.updateLoggedIn(true)
  }),
  loginWithToken: thunk(async (actions, token) => {
    actions.updateUser({ id: 'id', name: 'John DOE' })
    actions.updateToken(token)
    actions.updateLoggedIn(true)
  }),
}

export default authModel
