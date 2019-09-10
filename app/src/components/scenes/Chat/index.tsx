import React, { useCallback, useMemo, useState } from 'react'

import Box from '../../Box'
import FormBox from '../../FormBox'
import Text from '../../Text'
import TextInput from '../../TextInput'
import Logo from '../../Logo'

import logoSvg from '../../../logo.svg'
import { useStoreState, useStoreActions } from '../../../store'
import { Message } from '../../../store/models/messages'
import Button from '../../Button'

interface ChatMessageProps {
  message: Message
}

export const ChatMessage = React.memo<ChatMessageProps>(props => {
  const user = useStoreState(state => state.auth.user)

  const isSelf = useMemo(() => {
    return props.message.userId === user.id
  }, [props.message.userId, user])

  const bgColor = useMemo(() => {
    return isSelf ? 'selfMessage' : 'otherMessage'
  }, [isSelf])

  const alignSelf = useMemo(() => {
    return isSelf ? 'flex-end' : 'flex-start'
  }, [isSelf])

  return (
    <Box
      mb={3}
      flexDirection="column"
      alignSelf={alignSelf}
      maxWidth={[200, 300, 400]}
    >
      <Box flexDirection="column" maxHeight={50} style={{ overflow: 'hidden' }}>
        <Text alignSelf={alignSelf} color="placeholder">
          {props.message.userName}
        </Text>
      </Box>

      <Box
        mt={1}
        p={3}
        bg={bgColor}
        borderRadius={3}
        style={{ overflow: 'hidden' }}
      >
        <Text color="white">{props.message.text}</Text>
      </Box>
    </Box>
  )
})

export const Chat: React.FC = () => {
  const [message, setMessage] = useState('')
  const [sendDisabled, setSendDisabled] = useState(false)

  const user = useStoreState(state => state.auth.user)
  const messages = useStoreState(state => state.messages.messages)
  const sendMessage = useStoreActions(actions => actions.messages.sendMessage)

  const handleMessageChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      setMessage(e.target.value)
    },
    []
  )
  const handleSend = useCallback(
    async (e: React.FormEvent<HTMLFormElement>) => {
      e.preventDefault()

      setSendDisabled(true)
      try {
        await sendMessage(message)
        setMessage('')
      } catch (err) {
        window.alert(err)
      }

      setSendDisabled(false)
    },
    [message, sendMessage]
  )

  return (
    <Box flexDirection="column" flexGrow={1} alignSelf="stretch">
      <Box
        p={3}
        borderBottom="1px solid"
        borderBottomColor="placeholder"
        justifyContent={['center', 'space-between']}
        alignItems="center"
      >
        <Logo src={logoSvg} height={50} />
        <Box display={['none', 'flex']}>A toi le clavardage, {user.name} !</Box>
      </Box>

      <Box flexGrow={1} style={{ overflow: 'hidden' }}>
        <Box flexDirection="column" flexGrow={1}>
          <Box
            p={3}
            flexDirection="column-reverse"
            flexGrow={1}
            style={{ overflowY: 'auto' }}
          >
            {/* Trick to unreverse. The advantage of reverse is that the top
            of the list is at the bottom, so there's no need to scroll to stay
            in sync */}
            <Box flexDirection="column">
              {messages.map(message => (
                <ChatMessage key={message.id} message={message} />
              ))}
            </Box>
          </Box>

          <FormBox p={2} onSubmit={handleSend}>
            <TextInput
              value={message}
              onChange={handleMessageChange}
              flexGrow={1}
            />
            <Button ml={2} type="submit" disabled={sendDisabled}>
              Envoyer
            </Button>
          </FormBox>
        </Box>

        <Box
          p={3}
          display={['none', 'flex']}
          width={[0, 200, 300]}
          borderLeft="1px solid"
          borderLeftColor="placeholder"
        >
          Users
        </Box>
      </Box>
    </Box>
  )
}

export default Chat
