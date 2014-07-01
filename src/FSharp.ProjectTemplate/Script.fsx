// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "DomainTypes.fs"
#load "Client.fs"
open FSharp.Irc.DomainTypes
open FSharp.Irc.Client



let config = {
    Server = "irc.quakenet.org"
    Port = 6667
    User = "CptFirc"
    Nick = "CptFirc"
  }

let client = new ClientConnection(config)

client.InboundMessages
|> Observable.subscribe (printfn "%s")

client.Connect()