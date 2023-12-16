# README for Supermarket Network Information System

## Introduction
This web application serves as an information system for a network of supermarkets. It offers different functionalities based on the user's access level: unregistered users, registered users (categorized into administrators, managers, and workers), each having distinct privileges and access rights.

## User Roles and Permissions

### Unregistered Users
- Can only view basic information about the application and its functionalities.

### Registered Users
- **Administrators**: Have access to data of all supermarkets, can modify any data, manage user accounts, and acquire statistics about supermarket operations.
- **Managers**: Limited access to the supermarket they work in. They can view their supermarket's statistics, order new goods, assign workers to cash registers, open or close the store, add new employees, and set their positions.
- **Workers**: Can edit their profiles and view their assigned cash register for the day.

### Registration Process
- Users register with their surname, first name, supermarket name, email, and password. By default, they are assigned the role of a standard worker.
- Their profiles appear in the manager's list of the chosen supermarket or the administrator's list, where their role can be changed.
- Users can only change their surname and first name. Any other changes, like supermarket, email, or role, must be done by an administrator or manager.

## Installation and Setup

### Setup
- Requires .NET runtime installed on the system.
- Requires Node.js and NPM installed on the system.

### Oracle Database Connection
- For the current university project version, an Oracle database connection is required.

## Application Usage

### Interface
- After logging in, users see the supermarket statistics or their profile (for workers).
- A navigation panel on the left side of the screen allows switching between different sections: statistics, profile, supermarket list, worker list, products, products in stock, products on shelves, and settings.

### Features
- Settings section allows role modifications, monitoring cash registers, and viewing application logs.
- Managers in their product stock section can order products with a simple button press or make manual orders by entering necessary details.
- A logout button at the bottom left ensures a safe end to the session.

### Design
- Intuitive and user-friendly, ensuring efficient and comfortable system use for all user types.
