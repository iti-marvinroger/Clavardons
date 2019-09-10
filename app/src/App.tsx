import React from 'react'
import { StoreProvider } from 'easy-peasy'
import { ThemeProvider } from 'styled-components'

import store from './store'
import theme from './theme'

import Router from './Router'
import styled from 'styled-components'

const StyledApp = styled.div`
  display: flex;

  width: 100vw;
  height: 100vh;

  justify-content: center;
  align-items: center;
`

const App: React.FC = () => {
  return (
    <StoreProvider store={store}>
      <ThemeProvider theme={theme}>
        <StyledApp>
          <Router />
        </StyledApp>
      </ThemeProvider>
    </StoreProvider>
  )
}

export default App
