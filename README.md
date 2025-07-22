# EcoSimConsole

EcoSimConsole is a Windows Presentation Foundation (WPF) application that simulates a space-based economy. It features a dynamic universe with various entities such as workers, independents, ships, stations, outposts, and stellar objects. The simulation includes a production chain, resource management, and trade.

## About The Project

The core concept of EcoSimConsole is to create a living economic simulation in a persistent universe. Here are some of the key features:

*   **Persistent Universe:** The state of the universe is saved to JSON files, allowing for a persistent experience.
*   **Dynamic Economy:** The economy is driven by the interactions of various entities, each with their own goals and behaviors.
*   **Production Chains:** A complete production chain is simulated, from mining raw resources to manufacturing complex goods.
*   **Entity Variety:** The universe is populated with a variety of entities, including:
    *   **Workers:** The low-class population, working at and being transported between locations.
    *   **Independents:** The mid and high-class population, with their own means of transport and the ability to perform various tasks.
    *   **Ships and Outposts:** Entities with stats such as health, fuel, cargo space, and combat ability.
    *   **Orgs:** Collections of independents and/or workers with a common goal.
*   **Visualization:** The application provides a visual representation of the universe and its entities.

## Getting Started

To get a local copy up and running, follow these simple steps.

### Prerequisites

*   [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472)
*   [Visual Studio](https://visualstudio.microsoft.com/)

### Installation

1.  Clone the repo
    ```sh
    git clone https://github.com/your_username_/EcoSimConsole.git
    ```
2.  Open `EcoSimConsole.sln` in Visual Studio.
3.  Build the solution. This will restore the necessary NuGet packages.
4.  Run the application from Visual Studio.

## Usage

The application provides several views and editors to interact with the simulation:

*   **Main Window:** The main window displays the star system and the locations within it. You can select locations to view more details.
*   **Location Editor:** Create new locations in the star system.
*   **Production Editor:** Create new production chains.
*   **Module Viewer:** View and edit the modules of a selected location.

## Project Structure

The project is organized as follows:

*   **EcoSimConsole/**: The main project directory.
    *   **Control/**: Contains the main control classes that manage the application's logic.
        *   `MainControl.cs`: The core class that manages the simulation.
    *   **Data/**: Contains the data models for the simulation.
        *   **Resources/**: Contains classes for commodities, production, and recipes.
        *   **World/**: Contains classes for the game world, such as locations and stellar objects.
    *   **Windows/**: Contains the UI windows for the application.
    *   **bin/Debug/**: Contains the compiled application and data files (`.json`).

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1.  Fork the Project
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the Branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request

## License

Distributed under the MIT License. See `LICENSE` for more information.
