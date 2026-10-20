# FlashTalk

A real-time chat application built with ASP.NET Core MVC, PostgreSQL, and RabbitMQ. Features user authentication, one-on-one messaging, and group chat functionality.

## Tech Stack

- **Backend**: ASP.NET Core 8.0 (MVC)
- **Database**: PostgreSQL (via Npgsql)
- **Message Broker**: RabbitMQ (for real-time messaging)
- **Frontend**: Razor Views with Kendo UI
- **Dependency Injection**: Built-in .NET DI container

## Project Structure

```
FlashTalk/
├── MVC/                          # Main web application
│   ├── Controllers/
│   │   ├── UserController.cs     # Login, logout
│   │   ├── ChatController.cs     # Send/receive private messages
│   │   ├── GroupController.cs    # Group chat send/receive
│   │   └── HomeController.cs     # Home/error pages
│   ├── Filters/
│   │   └── SessionCheck.cs       # Auth guard for logged-in users
│   ├── Views/
│   │   ├── User/Login.cshtml     # Login page
│   │   ├── Chat/Index.cshtml     # Main chat interface
│   │   ├── Group/Index.cshtml    # Group chat interface
│   │   └── Shared/               # Layout, validation scripts
│   ├── Models/
│   │   └── ErrorViewModel.cs
│   ├── Program.cs                # Service registration, middleware
│   ├── appsettings.json          # Connection strings & config
│   └── MVC.csproj
└── Repositories/                 # Data access layer
    ├── Models/
    │   ├── t_User.cs             # User model
    │   └── t_Messages.cs         # Message model (unused)
    ├── Interfaces/
    │   ├── IUserInterface.cs
    │   ├── IMessageInterface.cs
    │   └── IGroupInterface.cs
    ├── Implementations/
    │   ├── UserRepo.cs           # PostgreSQL user queries
    │   ├── MessageRepo.cs        # RabbitMQ private messaging
    │   └── GroupRepo.cs          # RabbitMQ group messaging
    └── Repositories.csproj
```

## Features

- User login with email & password
- Real-time private messaging (RabbitMQ)
- Group chat with fanout exchanges
- Session-based authentication
- Searchable user dropdown for starting chats
- Responsive chat UI with message bubbles

## Database Schema

### PostgreSQL

The application uses a PostgreSQL database named `first` (see `appsettings.json`).

#### Table: `t_User`

```sql
CREATE TABLE t_User (
    c_userid SERIAL PRIMARY KEY,
    c_username VARCHAR(150) NOT NULL,
    c_email VARCHAR(150) NOT NULL UNIQUE,
    c_password VARCHAR(150) NOT NULL
);
```

> **Note**: The `t_Messages` model exists in code but is never used — messaging is handled entirely by RabbitMQ.

### RabbitMQ

Messages are sent via RabbitMQ queues/exchanges. The application expects a virtual host named `Brijesh` with the `guest` user having full permissions.

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL 14+ (running)
- RabbitMQ 3.12+ (running with management plugin)

## Setup Instructions

### 1. Install Dependencies

```bash
cd MVC
dotnet restore
```

### 2. Configure PostgreSQL

Create the database and table:

```sql
CREATE DATABASE first;

CREATE TABLE t_User (
    c_userid SERIAL PRIMARY KEY,
    c_username VARCHAR(150) NOT NULL,
    c_email VARCHAR(150) NOT NULL UNIQUE,
    c_password VARCHAR(150) NOT NULL
);
```

Add test users (the app has no registration page yet):

```sql
INSERT INTO t_user (c_username, c_email, c_password) VALUES
('Brijesh', 'brijesh@example.com', 'Brijesh@123'),
('Bob', 'bob@example.com', 'bob@123');
```

### 3. Configure RabbitMQ

Create the `Brijesh` virtual host and grant permissions to the `guest` user:

```bash
rabbitmqctl add_vhost Brijesh
rabbitmqctl set_permissions -p Brijesh guest '.*' '.*' '.*'
```

Or via the Management API (http://localhost:15672):

```
PUT /api/vhosts/Brijesh
PUT /api/permissions/Brijesh/guest
```

### 4. Update Configuration

Edit `MVC/appsettings.json` to match your environment:

```json
{
  "ConnectionStrings": {
    "pgconn": "Server=localhost;Port=5432;User Id=postgres;Password=your_password;Database=first;"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest",
    "VirtsualHost": "Brijesh"
  }
}
```

### 5. Run the Application

```bash
cd MVC
dotnet run
```

The app will be available at `http://localhost:5243`.

## Usage

1. Navigate to the login page
2. Log in with your email and password (from the `t_user` table)
3. Once logged in, you'll be redirected to the chat interface
4. Select a user from the dropdown to start a private conversation
5. Type and send messages in real-time

## License

MIT