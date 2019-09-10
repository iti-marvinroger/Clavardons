import {
  HubConnectionBuilder,
  JsonHubProtocol,
  LogLevel,
} from '@aspnet/signalr'

export const createConnection = (url: string) => {
  return new HubConnectionBuilder()
    .configureLogging(LogLevel.None)
    .withUrl(url)
    .withHubProtocol(new JsonHubProtocol())
    .build()
}
