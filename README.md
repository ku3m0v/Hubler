Overview

This web application is an information system for a supermarket network. It offers different access levels for unregistered users, administrators, managers, and workers.
Unregistered Users

    Limited access: Can only view the app's offerings and features.

Registered Users

    Administrator: Access to all supermarkets' data, modify any data, manage user accounts, and obtain supermarket operation statistics.
    Manager: Access to their supermarket's data, view statistics, order new goods, assign workers to tills, open/close tills, add new employees, and set positions.
    Worker: Can edit their profile and check their assigned till.

Installation

As a web application, no installation is required for end-users. For administrators, backend.exe and frontend.zip are available.
Backend Setup (.NET)

    Requires .NET runtime installed.
    Start the backend application by double-clicking backend.exe or using the command line:

    ./Hubler.exe

    After starting backend.exe, the application will listen to requests from the frontend.

Frontend Setup (Angular)

    Requires Node.js and NPM installed.
    Execute these commands in the terminal in the unzipped Hubler.zip folder:

    arduino

    cd frontend     // Navigate to the frontend folder
    npm install     // Install dependencies
    ng serve        // Start the Angular server

Access Permissions

    During registration, users provide their last name, first name, supermarket name, email, and password.
    Default role: Ordinary worker with profile access.
    Administrators or managers can change user roles.
    Users can change their last name and first name. Other data modifications require administrator or manager intervention.

Application Usage

    Upon login, users see their supermarket statistics (workers see their profiles).
    The left-hand side navigation panel switches between sections: statistics, profile, supermarket list, worker list, products, inventory, shelf products, and settings.
    In the settings section, users can edit roles, monitor tills, and view application logs.
    Managers have additional inventory options, including automatic and manual ordering.
    A logout button in the lower left corner ensures secure application exit.

Additional Requirement for University Project

    To run this project, you need to connect to an Oracle database.
