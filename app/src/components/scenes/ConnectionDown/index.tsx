import React from 'react'

import Box from '../../Box'
import Logo from '../../Logo'
import Text from '../../Text'

import logoSvg from '../../../logo.svg'

export const ConnectionDown: React.FC = () => {
  return (
    <Box flexDirection="column" alignItems="center">
      <Logo src={logoSvg} width={300} />
      <Text mt={3}>Connexion au serveur en cours..</Text>
    </Box>
  )
}

export default ConnectionDown
