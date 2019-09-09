import { Action, action, thunk, Thunk } from 'easy-peasy'
import { StoreModel } from '.'

export interface Message {
  id: string
  text: string
  userId: string
}

export interface MessagesModel {
  messages: Message[]

  addMessages: Action<MessagesModel, Message[]>

  sendMessage: Thunk<MessagesModel, string, unknown, StoreModel>
  monitorMessages: Thunk<MessagesModel, void, unknown, StoreModel>
}

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

  sendMessage: thunk((actions, text, { getStoreState }) => {
    actions.addMessages([
      { id: Date.now().toString(), text, userId: getStoreState().auth.user.id },
    ])
  }),
  monitorMessages: thunk((actions, _, { getStoreState }) => {
    let i = 0
    setInterval(() => {
      const meId = getStoreState().auth.user.id
      actions.addMessages([
        {
          id: Date.now().toString(),
          text: new Date().toISOString(),
          userId: ++i % 4 === 0 ? meId : 'foo',
        },
      ])
    }, 1500)
  }),
}

export default messagesModel
