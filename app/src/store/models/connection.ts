import { Action, action, Thunk, thunk } from 'easy-peasy'
import { StoreModel } from '.'

export interface ConnectionModel {
  url: string
  connected: boolean

  updateUrl: Action<ConnectionModel, string>
  updateIsConnected: Action<ConnectionModel, boolean>

  handleConnection: Thunk<
    ConnectionModel,
    signalR.HubConnection,
    unknown,
    StoreModel
  >
}

const connectionModel: ConnectionModel = {
  url: '',
  connected: false,

  updateUrl: action((state, payload) => {
    state.url = payload
  }),
  updateIsConnected: action((state, payload) => {
    state.connected = payload
  }),

  handleConnection: thunk((actions, connection, { getState }) => {
    async function start() {
      try {
        console.log(`Connecting to ${getState().url}...`)
        await connection.start()
        actions.updateIsConnected(true)
        console.log('Connection successful')
      } catch (err) {
        console.error(`Cannot connect!`, err)
        setTimeout(() => start(), 5000)
      }
    }

    connection.onclose(async () => {
      actions.updateIsConnected(false)
      await start()
    })

    start()
  }),
}

export default connectionModel
