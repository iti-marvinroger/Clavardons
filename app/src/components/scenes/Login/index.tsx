import React, { useCallback, useState } from 'react'

import Box from '../../Box'
import Button from '../../Button'
import FormBox from '../../FormBox'
import TextInput from '../../TextInput'
import Logo from '../../Logo'

import logoSvg from '../../../logo.svg'
import { useStoreState, useStoreActions } from '../../../store'

export const Login: React.FC = () => {
  const [name, setName] = useState('')
  const isLoading = useStoreState(state => state.auth.loading)
  const loginWithName = useStoreActions(actions => actions.auth.loginWithName)

  const handleNameChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      setName(e.target.value)
    },
    []
  )
  const handleSubmit = useCallback(
    (e: React.FormEvent<HTMLFormElement>) => {
      e.preventDefault()
      loginWithName(name)
    },
    [name, loginWithName]
  )

  return (
    <Box flexDirection="column">
      <Logo src={logoSvg} width={300} />
      {!isLoading && (
        <FormBox onSubmit={handleSubmit} mt={3}>
          <TextInput
            placeholder="Ton nom"
            value={name}
            onChange={handleNameChange}
            flexGrow={1}
            mr={3}
          />
          <Button type="submit">Clavardons !</Button>
        </FormBox>
      )}
    </Box>
  )
}

export default Login
