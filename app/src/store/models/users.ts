import { Action, action, Thunk, thunk } from 'easy-peasy'
import { StoreModel } from '.'
import { NewUserEvent, RemoveUserEvent } from '../../messages'

export interface User {
  id: string
  name: string
}

export interface UsersModel {
  users: {
    [key: string]: User
  }

  addUser: Action<UsersModel, User>
  removeUser: Action<UsersModel, string>
  clearUsers: Action<UsersModel>

  handleConnection: Thunk<
    UsersModel,
    signalR.HubConnection,
    unknown,
    StoreModel
  >
}

let connection: signalR.HubConnection

const usersModel: UsersModel = {
  users: {},

  addUser: action((state, user) => {
    state.users[user.id] = user
  }),
  removeUser: action((state, userId) => {
    delete state.users[userId]
  }),
  clearUsers: action(state => {
    state.users = {}
  }),

  handleConnection: thunk((actions, connection_) => {
    connection = connection_

    connection.on('AddUser', (newUser: NewUserEvent) => {
      console.log('Receiving new user...')
      actions.addUser(newUser)
    })

    connection.on('RemoveUser', (removedUser: RemoveUserEvent) => {
      console.log('Deleting user user...')
      actions.removeUser(removedUser.id)
    })
  }),
}

export default usersModel
