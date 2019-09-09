import React, { useMemo } from 'react'

import { useStoreState } from './store'

import { Chat } from './components/scenes/Chat'
import { Login } from './components/scenes/Login'

const Router: React.FC = () => {
  const isLoggedIn = useStoreState(state => state.auth.loggedIn)

  const Route = useMemo(() => (isLoggedIn ? Chat : Login), [isLoggedIn])

  return <Route />
}

export default Router
