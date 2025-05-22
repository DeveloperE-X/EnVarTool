# EnVarTool
Useful tool for environment variables

A cross-platform .NET console tool to view and search environment variables from all scopes (Process, User, Machine) with color-coded output.

## Features
- Lists all environment variables from Process, User, and Machine scopes
- Color-codes variables by scope for easy identification
- Supports filtering by substring (`--find`) or regex (`--regex`)
- Paging for large output

## Usage

```
./EnVarTool.Console
```

### Options

- `./EnVarTool.Console`  
  Display all environment variables from all scopes
- `./EnVarTool.Console --find TERM`  
  Filter variables by TERM (case-insensitive, all scopes)
- `./EnVarTool.Console --regex PATTERN`  
  Filter variables by regex pattern (all scopes)
- `./EnVarTool.Console --help`  
  Show help message

### Example Output

```
├─ PATH = /usr/local/bin:/usr/bin:/bin [Machine]
├─ HOME = /home/user [User]
└─ TEMP = /tmp [Process]
```

- Cyan: Process
- Green: User
- Magenta: Machine

## Building

```
dotnet build
```

## Running

```
./EnVarTool.Console --find PATH
```

