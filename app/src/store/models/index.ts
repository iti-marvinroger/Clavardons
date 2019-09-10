import authModel, { AuthModel } from './auth'
import connectionModel, { ConnectionModel } from './connection'
import messagesModel, { MessagesModel } from './messages'
import usersModel, { UsersModel } from './users'

export interface StoreModel {
  auth: AuthModel
  connection: ConnectionModel
  messages: MessagesModel
  users: UsersModel
}

const storeModel: StoreModel = {
  auth: authModel,
  connection: connectionModel,
  messages: messagesModel,
  users: usersModel,
}

export default storeModel
