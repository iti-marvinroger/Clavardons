export interface LoginResponse {
  success: boolean
  token: string
  userId: string
  name: string
}

export interface MessageEvent {
  id: string
  userId: string
  userName: string
  text: string
}
