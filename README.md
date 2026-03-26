# InfoBoard MAUI Application

A cross platform dashboard application built with .NET MAUI. This project focuses on clean architecture and robust design patterns. It serves as a centralized hub for managing To Do lists, monitoring game servers, and viewing weather or RSS updates.

## Architecture: Onion Architecture
The project is strictly structured according to Onion Architecture principles. This ensures separation of concerns, testability, and maintainability:

* **Domain.Models**: The core kernel. Contains pure entities and base classes like BasePropertyChanged. It has zero dependencies on other layers.
* **InfoBoard.Core**: The application logic layer. Contains interfaces and service implementations for RSS, Weather, and To Do logic.
* **InfoBoard.Presentation**: The outermost layer built with MAUI. Handles UI, XAML, and ViewModels. It depends on Core and Domain but remains decoupled from business logic.

## Design Patterns Applied

### 1. Facade Pattern
To simplify the interaction between the UI and complex subsystems, a Facade is implemented. This provides a simplified interface for the ViewModels by hiding the complexity of underlying services.

### 2. Singleton and Factory Patterns
* **Manual Thread Safe Singleton**: Implemented for critical services to ensure a single instance exists throughout the application lifecycle.
* **Factory Registration**: Utilized in MauiProgram.cs to handle complex object creation. This includes injecting specific file paths and JsonSerializerOptions into services.

### 3. Dependency Injection (DI)
Extensive use of the built in .NET DI container to manage service lifetimes:
* **Singletons**: Used for state persistent services and ViewModels like To Do and Game Server tracking.
* **Transients**: Used for stateless UI components and views to ensure a fresh state upon navigation.

## Technical Features
* **Modern .NET 10** and MAUI framework.
* **MVVM Pattern**: Leveraging CommunityToolkit.Mvvm for clean data binding.
* **Asynchronous Programming**: Fully non blocking UI using async and await with HttpClientFactory.
* **JSON Persistence**: Local data storage using System.Text.Json with customized serialization policies.

## Project Structure
* **Domain.Models**: Core Entities
* **InfoBoard.Core**: Business Logic and Interfaces
* **InfoBoard.Presentation**: MAUI UI and ViewModels
* **InfoBoard.slnx**: Visual Studio Solution Entry

---
*Developed as part of the Advanced .NET Programming course.*
