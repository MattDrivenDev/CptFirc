module FSharp.Irc.Client

open System
open System.IO
open System.Net
open System.Net.Sockets


/// Simple configuration for a ClientConnection
type ConnectionConfiguration = {
    Server : string
    Port   : int
    User   : string
    Nick   : string
  }


let private createTcpClient (server, port) = 
  new TcpClient (server, port)

let private openStream (client:TcpClient) = 
  client.GetStream()

let private createReaderWriter (stream:Stream) = 
  new StreamReader(stream), new StreamWriter(stream)

let private sendMessage (writer:StreamWriter) (msg:string) = 
  writer.WriteLine(msg)
  writer.Flush()

let private ping (msg:string) =
  match msg with
  | _ when msg.StartsWith("PING") -> Some msg
  | _ -> None


type ClientConnection (config) = 

  let mutable inbound, outbound = Unchecked.defaultof<_>, Unchecked.defaultof<_>

  let openConnection () = 
    let readerWriter = 
      (config.Server, config.Port)
      |> createTcpClient
      |> openStream
      |> createReaderWriter
    inbound <- readerWriter |> fst
    outbound <- readerWriter |> snd

  let inboundEvent = new Event<string>()

  let inboundEventStream = inboundEvent.Publish

  let pingPong() = 
    let pong (ping:string) = 
      ping.Replace("PING", "PONG") |> sendMessage outbound
    inboundEventStream
    |> Observable.choose ping
    |> Observable.subscribe pong
    |> ignore

  member this.Connect() = 
    let workflow = async {
        let! msg = Async.AwaitTask(inbound.ReadLineAsync())
        msg
        |> inboundEvent.Trigger
        |> ignore
      }
    openConnection()
    pingPong()
    sprintf "USER %s 0 * :%s" config.User config.User |> sendMessage outbound
    sprintf "NICK %s" config.Nick |> sendMessage outbound
    while true do
      Async.RunSynchronously workflow
     
  member this.InboundMessages = inboundEventStream