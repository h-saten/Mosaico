# Introduction
ASP.NET SignalR is a library for ASP.NET developers that simplifies the process of adding real-time web functionality to applications. Real-time web functionality is the ability to have server code push content to connected clients instantly as it becomes available, rather than having the server wait for a client to request new data.
[More here](https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/introduction-to-signalr).

# Usage in Mosaico
We use SignalR for scenarios when users would like to automatically receive data on the frontend upon its arrival. Without signalr users have to perform REST API calls within some interval without being sure the new data is available.
SignalR solves such performance issue by using two-way binding via websockets. It keeps connection between client and the server in such way, that server is able to inform clients about new data when it is really needed. Communication through websockets is lightweight and cheap.

## How to create new connection

1. First of all, you need to implement a new Hub which will be responsible for dispatching messages.

```c#
public class SomeHub : HubBase {
    private readonly IHubContext<SomeHub> _hubContext;

    public SomeHub(IHubContext<SomeHub> hubContext, ILogger logger = null) : base(logger)
    {
        _hubContext = hubContext;
    }
}
```
2. Create an interface with desired methods so you can register your distributor in the DI (Autofac)
```c#
public interface ISomeDistributor {
    Task SendSomethingAsync(Something payload);
}

public class SomeHub : HubBase, ISomeDistributor {
    private readonly IHubContext<SomeHub> _hubContext;

    public SomeHub(IHubContext<SomeHub> hubContext, ILogger logger = null) : base(logger)
    {
        _hubContext = hubContext;
    }
    
    public Task SendSomethingAsync(Something payload) {
        ...
    }
}
```
3. Register your hub in `SignalRModule` like this:
```c#
builder.RegisterType<SomeHub>().As<ISomeDistributor>();
```
4. Add a new path to your API server, so frontend can connect using this endpoint. Add new section in `Extensions\EndpointBuilderExtensions.cs`. This extension is called in `Startup.cs` of Core Service.
```c#
endpoints.MapHub<SomeHub>("/hubs/{ENDPOINT_NAME}", options =>
{
    options.Transports = HttpTransportType.LongPolling;
    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
});
```
5. Now you can implement your dispatch method. There are multiple methods you can use: Users, Groups or All. In most cases we will use either All or Users.
   - **Users**: when you want to notify a specific user (by his ID) if he is listening to changes
   - **All**: when you want to notify all users listening to the particular event
Extend your `SomeHub` with a call for dispatch:
   - **Users**: `await _hubContext.Clients.User(userId).SendAsync("somethingUpdated", counter);`
   - **All**: `await _hubContext.Clients.All.SendAsync("somethingUpdated", estimates);`\
   
6. Pay attention to the first parameter of SendAsync. It is a function name. Within one hub you can register many functions. It will reduce amount of listeners on your frontend. Try to group them logically whenever possible.
7. Implement a Hub Service on frontend which usually will have the same structure:
```typescript
@Injectable({
    providedIn: 'root'
})
export class SomeHubService {

    private hubConnection: signalR.HubConnection;
    private baseUrl = '';
    public something$ = new BehaviorSubject<Something>(null);

    constructor(private http: HttpClient, configService: ConfigService, private authClient: OidcSecurityService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
    }

    public startConnection() {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${this.baseUrl}/core/hubs/{ENDPOINT_NAME}`, {
                accessTokenFactory: () => this.authClient.getAccessToken(),
                transport: signalR.HttpTransportType.LongPolling,
                withCredentials: true
            })
            .build();
        this.hubConnection
            .start()
            .then(() => console.log('Connection started'))
            .catch(err => console.log('Error while starting connection: ' + err));
    }

    public addListener() {
        this.hubConnection.on('somethingUpdated', (data) => {
            this.something$.next(data);
        });
    }

    public removeListener() {
        if(this.hubConnection && this.hubConnection.state !== signalR.HubConnectionState.Disconnected || 
            this.hubConnection.state !== signalR.HubConnectionState.Disconnecting) {
            this.hubConnection.stop();
        }
    }
}
```
8. Now you can use this service in your component to connect to the real-time data from backend:
```typescript
@Component({
  selector: 'app-something',
  templateUrl: './something.component.html',
  styleUrls: ['./something.component.scss']
})
export class SomethingComponent implements OnInit, OnDestroy {
    constructor(private hub: SomeHubService) {}

    ngOnInit(): void {
        this.hub.startConnection();
        this.hub.addListener();
        this.hub.something$.subscribe((newSomething) => {
            //DO SOMETHING
        });
    }

    ngOnDestroy(): void {
        this.hub.removeListener();
    }
}
```
9. **DO NOT FORGET TO REMOVE LISTENER** on component destroy.