import React from 'react'
import ReactDOM from 'react-dom'
import store from './store'
import { createConnection } from './services/signalr'

import './index.css'
import App from './App'

const DEFAULT_URL = 'http://127.0.0.1:5000'

const urlParams = new URLSearchParams(window.location.search)
const serverUrl = urlParams.get('server_url') || DEFAULT_URL

const connection = createConnection(serverUrl)

const actions = store.getActions()

actions.connection.updateUrl(serverUrl)
actions.connection.handleConnection(connection)
actions.auth.handleConnection(connection)
actions.messages.handleConnection(connection)

ReactDOM.render(<App />, document.getElementById('root'))
