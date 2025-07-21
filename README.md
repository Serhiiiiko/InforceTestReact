# URL Shortener - Full-Stack Application

This is a full-stack web application that allows users to shorten long URLs. The project is built with an ASP.NET Core backend and a React frontend, featuring user authentication and URL management.

![Project Diagram](https://mermaid.ink/svg/eyJjb2RlIjoiZ3JhcGggVERcbiAgICBzdWJncmFwaCBcIkZyb250ZW5kIChSZWFjdCkrVml0ZVwiXG4gICAgICAgIEJyKHdpbmRvdykgLS0-IHxSZXF1ZXN0fCBBUChBcHBsaWNhdGlvbik7XG4gICAgICAgIEFQIC0tPiB8QVBJIFNlcnZpY2V8IEFQSUMoQVBJIFNlcnZpY2UpO1xuICAgICAgICBBUElDIC0tPiB8UHJveHkgUmVxdWVzdHwgUHJveHkoVml0ZSBEZXYgU2VydmVyKTtcbiAgICBlbmRcblxuICAgIHN1YmdyYXBoIFwiQmFja2VuZCAoQVNQLk5FVCBDb3JlKVwiXG4gICAgICAgIEJLX0FQSShBUEkgRW5kcG9pbnRzKSAtLT4gQXV0aFNlcnZpY2U7XG4gICAgICAgIEJLX0FQSSAtLT4gVXJsU2VydmljZTtcbiAgICAgICAgQXV0aFNlcnZpY2UgPD09PiBEYihFbnRpdHkgRnJhbWV3b3JrKTtcbiAgICAgICAgVXJsU2VydmljZSA8PT0-IERiO1xuICAgIGVuZFxuXG4gICAgUHJveHkgLS0-IHxIVFRQIFJlcXVlc3R8IEJLX0FQSTtcbiIsIm1lcm1haWQiOnsidGhlbWUiOiJkZWZhdWx0In0sInVwZGF0ZUVkaXRvciI6ZmFsc2UsImF1dG9TeW5jIjp0cnVlLCJ1cGRhdGVEaWFncmFtIjpmYWxzZX0)

---

## ✨ Features

-   **User Authentication**: Secure login system with JWT (JSON Web Tokens). Two roles are available: `Admin` and `User`.
-   **URL Shortening**: Registered users can create short aliases for long URLs.
-   **URL Management**: Users can vihttps://github.com/Serhiiiiko/InforceTestReact.gitew and delete their own shortened URLs.
-   **Admin Privileges**: Admins can view and delete URLs created by any user.
-   **Redirection**: The short URLs will redirect to the original long URL.
-   **Unit & Integration Testing**: The backend includes tests for services, controllers, and API integration.

## 🛠️ Technology Stack

-   **Backend**:
    -   ASP.NET Core 8
    -   Entity Framework Core 8
    -   JWT for Authentication
    -   SQLite (for simplicity, can be swapped)
    -   xUnit, Moq, FluentAssertions for testing
-   **Frontend**:
    -   React 18
    -   TypeScript
    -   Vite as a build tool
    -   Axios for API calls

## 🚀 Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Node.js](https://nodejs.org/) (version 20.x or higher recommended)
-   A code editor like [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation & Running

1.  **Clone the repository**
    ```bash
    git clone https://github.com/Serhiiiiko/InforceTestReact
    cd https://github.com/Serhiiiiko/InforceTestReact
    ```

2.  **Run the Backend (ASP.NET Core)**
    -   **Using Visual Studio:**
        1.  Open the `.sln` file in Visual Studio.
        2.  The first time you run the project, the database will be created and seeded with initial data (users `admin` and `user`).
        3.  Press `F5` to start the backend server. It will open a browser window with the Swagger UI.
    -   **Using .NET CLI:**
        ```bash
        cd InforceTestReact.Server
        dotnet run
        ```

3.  **Run the Frontend (React + Vite)**
    1.  Open a new terminal.
    2.  Navigate to the client app folder:
        ```bash
        cd inforcetestreact.client
        ```
    3.  Install the necessary npm packages:
        ```bash
        npm install
        ```
    4.  Start the development server:
        ```bash
        npm run dev
        ```
    5.  Open your browser and navigate to the URL provided by Vite (usually `https://localhost:52662` or similar).

## 🖥️ Usage

Once both servers are running, you can use the application.

1.  **Login Page**
    The application will first show a login page. You can use the pre-seeded credentials:
    -   **Admin**: `admin` / `admin123`
    -   **User**: `user` / `user123`

    ![Login Page](https://github.com/user-attachments/assets/512e7400-1167-4123-a81b-ac256677d29a)

2.  **Main Application**
    After logging in, you will see the main URL shortener interface.
    -   Enter a long URL in the input field and click "Shorten".
    -   The new short URL will appear in the table below.
    -   You can delete any URL by clicking the "Delete" button.

    ![Main App Page](https://github.com/user-attachments/assets/bfcad680-2463-4476-aea8-6b71fafd9230)


---

## 📂 Project Structure

```text
/
├── InforceTestReact.Server/      # ASP.NET Core Backend Project
│   ├── Controllers/              # API Controllers (Auth, Urls)
│   ├── Data/                     # EF Core DbContext
│   ├── Models/                   # Data Models and DTOs
│   ├── Services/                 # Business Logic (AuthService, UrlService)
│   └── Program.cs                # Main entry point, DI container setup
│
├── InforceTestReact.Tests/       # xUnit Tests Project
│   ├── Controllers/
│   └── Services/
│
├── inforcetestreact.client/      # React Frontend Project
│   ├── src/
│   │   ├── services/             # API service for communicating with backend
│   │   ├── App.tsx               # Main React component
│   │   └── main.tsx              # React entry point
│   └── vite.config.ts            # Vite configuration (including proxy)
│
└── README.md                     # This file
```

![Swagger Api](https://github.com/user-attachments/assets/2abc6df2-8966-45dd-81eb-6e775d4063ab)

