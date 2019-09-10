import { Action, action, thunk, Thunk } from 'easy-peasy'
import { StoreModel } from '.'
import { MessageEvent } from '../../messages'

export interface Message {
  id: string
  text: string
  userId: string
  userName: string
}

export interface MessagesModel {
  messages: Message[]

  addMessages: Action<MessagesModel, Message[]>
  clearMessages: Action<MessagesModel>

  handleConnection: Thunk<
    MessagesModel,
    signalR.HubConnection,
    unknown,
    StoreModel
  >
  sendMessage: Thunk<MessagesModel, string, unknown, StoreModel>
}

let connection: signalR.HubConnection

const messagesModel: MessagesModel = {
  messages: [],

  addMessages: action((state, messages) => {
    state.messages = state.messages.concat(messages)
  }),
  clearMessages: action(state => {
    state.messages = []
  }),

  sendMessage: thunk(async (_, text) => {
    console.log('Sending message...')
    await connection.invoke('SendMessage', text)
    console.log('Message sent')
  }),
  handleConnection: thunk((actions, connection_) => {
    connection = connection_

    connection.on('ReceiveMessage', (message: MessageEvent) => {
      console.log('Receiving message...')
      actions.addMessages([
        {
          id: message.id,
          text: message.text,
          userId: message.userId,
          userName: message.userName,
        },
      ])
    })

    connection.on('StopSpamming', (message: MessageEvent) => {
      console.log('Stop spamming!')
      window.alert('Clavardons avec respect®, arrête de spammer.')
    })
  }),
}

export default messagesModel
