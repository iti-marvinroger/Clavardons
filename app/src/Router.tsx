import React, { useMemo } from 'react'

import { useStoreState } from './store'

import { Chat } from './components/scenes/Chat'
import { ConnectionDown } from './components/scenes/ConnectionDown'
import { Login } from './components/scenes/Login'

const Router: React.FC = () => {
  const isLoggedIn = useStoreState(state => state.auth.loggedIn)
  const isConnectionUp = useStoreState(state => state.connection.connected)

  const Route = useMemo(() => {
    if (!isConnectionUp) {
      return ConnectionDown
    }

    return isLoggedIn ? Chat : Login
  }, [isLoggedIn, isConnectionUp])

  return <Route />
}

export default Router
