import { Action, action, Thunk, thunk, ThunkOn, thunkOn } from 'easy-peasy'
import { StoreModel } from '.'
import { LoginResponse } from '../../messages'
import { getTokenFromStorage, saveTokenToStorage } from '../../services/storage'

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

  handleConnection: Thunk<AuthModel, signalR.HubConnection, unknown, StoreModel>
  loginWithName: Thunk<AuthModel, string, unknown, StoreModel>
  loginWithToken: Thunk<AuthModel, string, unknown, StoreModel>

  onConnectionUp: ThunkOn<AuthModel, unknown, StoreModel>
}

let connection: signalR.HubConnection

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

  handleConnection: thunk((actions, connection_) => {
    connection = connection_
  }),
  loginWithName: thunk(async (actions, name) => {
    console.log('Logging in with name')
    const result = await connection.invoke<LoginResponse>('LoginWithName', name)
    actions.updateToken(result.token)
    actions.updateUser({ id: result.userId, name: result.name })
    actions.updateLoggedIn(true)

    saveTokenToStorage(result.token)

    console.log('Success')
  }),
  loginWithToken: thunk(async (actions, token) => {
    console.log('Logging in with stored token')
    const result = await connection.invoke<LoginResponse>(
      'LoginWithToken',
      token
    )

    if (!result.success) {
      console.log('Could not connect with the stored token')
    }

    actions.updateToken(result.token)
    actions.updateUser({ id: result.userId, name: result.name })
    actions.updateLoggedIn(true)

    console.log('Success')
  }),

  onConnectionUp: thunkOn(
    (_, storeActions) => storeActions.connection.updateIsConnected,
    (state, target) => {
      if (!target.payload) {
        return
      }

      // if we are connected

      const storedToken = getTokenFromStorage()

      if (!storedToken) {
        return
      }

      state.loginWithToken(storedToken)
    }
  ),
}

export default authModel
