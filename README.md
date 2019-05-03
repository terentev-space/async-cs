# SimpleCS
A library with a simple implementation of an asynchronous client server application. I do not pretend to uniqueness, but maybe someone will come in handy. Important: the server did for itself, no more, for a one-day project, you should not expect quality. Maybe I will finish the project later...

# Example
## Server
```cs
using SimpleCS;
using SimpleCS.Entity;
using SimpleCS.Entity.Config;

//...

    YourSuperCoolServerHandlerClass ServerHandler = new YourSuperCoolServerHandlerClass();
    // Host Port configured via ClientConfig
    SimpleServer Server = new SimpleServer(ServerHandler, new ServerConfig());
    Server.Start();
  
//...
```
## Client
```cs
using SimpleCS;
using SimpleCS.Entity;
using SimpleCS.Entity.Config;

//...

    // Server IP or Port configured via ClientConfig
    SimpleClient Client = new SimpleClient(new ClientConfig());
    string ServerResponse = Client.SendMessage("Hello server!");
  
//...
```
## ServerHandler
```cs
using SimpleCS.Entity;

public class YourSuperCoolServerHandlerClass : ServerHandler
{
  public override string ProcessRequest(string Data)
  {
    // Your logic here...
    return "Hello client!";//or null
  }
}
```
