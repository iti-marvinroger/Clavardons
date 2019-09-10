export interface LoginResponse {
  success: boolean
  token: string
  userId: string
  name: string
}

export interface LogoutResponse {
  success: boolean
}

export interface MessageEvent {
  id: string
  userId: string
  userName: string
  text: string
}

export interface NewUserEvent {
  id: string
  name: string
}

export interface RemoveUserEvent {
  id: string
}
