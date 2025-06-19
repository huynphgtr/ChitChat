# ChitChat App - Directory Structure

This repository contains the expected directory structure for the ChitChat application project built with C# and .NET 9.

## 📁 Project Structure

```
/ChitChatApp (Solution Root)
├── ChitChatApp.sln                  # Solution file
├── README.md                        # Project documentation
├── .gitignore                       # Git ignore file
├── ChitChatApp.Core/               # Class Library - Core business logic
│   ├── ChitChatApp.Core.csproj
│   └── Class1.cs
├── ChitChatApp.Share/              # Class Library - Shared components for UI
│   ├── ChitChatApp.Share.csproj
│   └── Class1.cs
├── ChitChatApp.AvaloniaUI/         # Avalonia MVVM Project
│   ├── ChitChatApp.AvaloniaUI.csproj
│   ├── Program.cs
│   ├── App.axaml
│   ├── ViewModels/
│   ├── Views/
│   └── Models/
└── ChitChatApp.Blazor/             # Blazor Web App Project
    ├── ChitChatApp.Blazor.csproj
    ├── Program.cs
    ├── Components/
    │   ├── App.razor
    │   ├── Routes.razor
    │   ├── Layout/
    │   └── Pages/
    ├── wwwroot/
    │   └── css/
    │       └── tailwind.css         # Generated Tailwind CSS
    ├── Styles/
    │   └── input.css               # Tailwind CSS source
    ├── package.json                # Node.js dependencies
    ├── tailwind.config.js          # Tailwind configuration
    └── .gitignore                  # Blazor-specific gitignore
```

## 🛠️ Technologies Used

- **.NET 8** - Target framework for all projects
- **C#** - Primary programming language
- **Avalonia UI** - Cross-platform MVVM UI framework
- **Blazor** - Web UI framework
- **Tailwind CSS v4** - Utility-first CSS framework for Blazor project

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js (for Tailwind CSS in Blazor project)
- Visual Studio 2022 or VS Code
- Git

### Building the Solution

1. **Clone the repository**
   ```bash
   git clone https://github.com/huynphgtr/ChitChat.git
   cd ChitChatApp
   ```

2. **Restore .NET packages**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

### Working with Blazor Project

1. **Navigate to Blazor project**
   ```bash
   cd ChitChatApp.Blazor
   ```

2. **Install Node.js dependencies**
   ```bash
   npm install
   ```

3. **Build Tailwind CSS (for development with watch mode)**
   ```bash
   npm run build-css
   ```

4. **Build Tailwind CSS for production**
   ```bash
   npm run build-css-prod
   ```

5. **Run the Blazor application**
   ```bash
   dotnet run
   ```

### Working with Avalonia Project

1. **Navigate to Avalonia project**
   ```bash
   cd ChitChatApp.AvaloniaUI
   ```

2. **Run the Avalonia application**
   ```bash
   dotnet run
   ```

## 📋 Project Description

### ChitChatApp.Core
The heart of the application containing:
- Business logic
- Domain models
- Core services
- Data access interfaces

### ChitChatApp.Share
Shared components and utilities used across different UI projects:
- Shared models
- Common utilities
- Cross-platform components

### ChitChatApp.AvaloniaUI
Cross-platform desktop application using Avalonia UI with MVVM pattern:
- Desktop chat interface
- Real-time messaging
- Cross-platform compatibility (Windows, macOS, Linux)

### ChitChatApp.Blazor
Web-based chat application using Blazor:
- Modern web interface
- Responsive design with Tailwind CSS
- Real-time web chat functionality

## 🎨 Tailwind CSS Setup

The Blazor project includes a complete Tailwind CSS setup with:

- **Custom chat components**: Pre-built CSS classes for chat UI
- **Responsive design**: Mobile-first approach
- **Development workflow**: Watch mode for automatic CSS rebuilding
- **Production optimization**: Minified CSS output

### Available Custom Classes

- `.btn-primary` - Primary button styling
- `.chat-container` - Full-height chat layout
- `.chat-header` - Chat header with shadow
- `.chat-messages` - Scrollable messages area
- `.chat-input` - Input area styling
- `.message-bubble` - Base message bubble
- `.message-sent` - Sent message styling (blue, right-aligned)
- `.message-received` - Received message styling (white, left-aligned)

## 🔧 Development Workflow

1. **Start with Core project** - Implement business logic and models
2. **Develop Shared components** - Create reusable components
3. **Build UI projects** - Implement user interfaces using Core and Share
4. **Test across platforms** - Ensure compatibility

## 📝 Notes

- This is an **expected directory structure** showing the planned organization
- Projects are ready for development with basic scaffolding
- Tailwind CSS is fully configured and ready to use in Blazor project
- All projects target .NET 9 for consistency

## 👥 Team

This project structure was created for the PRN212 + SWP391 course team project.

## 📄 License

This project is for educational purposes as part of the PRN212+ SWP391 course.
