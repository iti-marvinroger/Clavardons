import { Action, action, thunk, Thunk } from 'easy-peasy'
import { StoreModel } from '.'
import { MessageEvent } from '../../messages'

export interface Message {
  id: string
  text: string
  userId: string
}

export interface MessagesModel {
  messages: Message[]

  addMessages: Action<MessagesModel, Message[]>

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
  messages: [
    { id: '1', text: 'Salut !', userId: 'id' },
    { id: '2', text: 'Hey :)', userId: 'id2' },
    { id: '3', text: 'Ca va ? ;)', userId: 'id' },
    { id: '4', text: 'Oui et toi ?', userId: 'id2' },
    { id: '5', text: 'Nickel...', userId: 'id' },
    { id: '6', text: 'Salut !', userId: 'id' },
    { id: '7', text: 'Hey :)', userId: 'id2' },
    { id: '8', text: 'Ca va ? ;)', userId: 'id' },
    { id: '9', text: 'Oui et toi ?', userId: 'id2' },
    { id: '10', text: 'Nickel...', userId: 'id' },
  ],

  addMessages: action((state, messages) => {
    state.messages = state.messages.concat(messages)
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
        },
      ])
    })
  }),
}

export default messagesModel
