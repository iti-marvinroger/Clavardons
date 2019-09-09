import authModel, { AuthModel } from './auth'
import messagesModel, { MessagesModel } from './messages'
import usersModel, { UsersModel } from './users'

export interface StoreModel {
  auth: AuthModel
  messages: MessagesModel
  users: UsersModel
}

const storeModel: StoreModel = {
  auth: authModel,
  messages: messagesModel,
  users: usersModel,
}

export default storeModel
