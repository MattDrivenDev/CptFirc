module FSharp.Irc.DomainTypes
  
type ConnectionCommand = 
  | Password of string
  | Server of string
  | Nick of string
  | Service of string
  | Quit of string
  | ServerQuit of string

type ChannelCommand = 
  | Join of string
  | Part of string
  | ChannelMode of string
  | Topic of string
  | Names of string
  | List of string
  | Invite of string
  | Kick of string

type SendMessageCommand = 
  | PrivateMessage of string
  | Notice of string

type ServerQueryCommand = 
  | Motd of string
  | Lusers of string
  | Version of string
  | Stats of string
  | Links of string
  | Time of string
  | Connect of string
  | Trace of string
  | Admin of string
  | Info of string

type ServiceQueryCommand = 
  | Servlist of string
  | Squery of string

type UserCommand = 
  | Who of string
  | Whois of string
  | Whowas of string

type MiscCommand = 
  | Kill of string
  | Ping of string
  | Pong of string
  | Error of string

type Command = 
  | ConnectionCommand of ConnectionCommand
  | ChannelCommand of ChannelCommand
  | SendMessageCommand of SendMessageCommand
  | ServerQueryCommand of ServerQueryCommand
  | ServiceQueryCommand of ServiceQueryCommand
  | UserCommand of UserCommand
  | MiscCommand of MiscCommand