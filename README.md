# OSI Model Demo

This project demonstrates the 7 layers of the OSI model with a real-world implementation using two separate .NET console applications that communicate over a network.

## Architecture

The solution consists of three projects:

1. **Server** - A .NET console application that listens on port 23673
2. **Client** - A .NET console application that connects to the server
3. **Shared** - A class library containing common OSI layer implementations and services

Both applications automatically detect and use system network information (MAC addresses, IP addresses) rather than using hardcoded values.

## OSI Layers Implemented

1. **Physical Layer** - Converts data to binary representation for transmission
2. **Data Link Layer** - Packages data into frames with MAC addresses
3. **Network Layer** - Handles routing and logical addressing (IP)
4. **Transport Layer** - Ensures complete data transfer with ports
5. **Session Layer** - Manages connections between applications
6. **Presentation Layer** - Translates data between application and network formats
7. **Application Layer** - Provides network services directly to end-user applications

## Features

- Real network communication between two separate processes
- Dynamic detection of system MAC and IP addresses
- Detailed visualization of data transformation at each layer
- Color-coded console output for better readability
- Bidirectional communication showing both encapsulation and de-encapsulation
- JSON serialization for data transmission between client and server

## Ports Used

- Server: 23673
- Client: Dynamically assigned port

## How to Run

1. Start the server:
   ```
   cd src/Server
   dotnet run
   ```

2. In a separate terminal, start the client:
   ```
   cd src/Client
   dotnet run
   ```

3. Enter a message when prompted in the client application

The application will show the data flowing through each OSI layer in both directions with detailed visualization.

## Building the Solution

To build the entire solution:
```
dotnet build
```

## Running with the Batch Script

You can also use the provided batch script to start both applications:
```
start-demo.bat
```