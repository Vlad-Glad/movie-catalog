# 🎬 Movie Catalog Web Application

## 🛠 Technology Stack

- **ASP.NET Core MVC (C#)**  
  Used as the main web framework for building the server-side application, controllers, routing, and Razor views.  

- **Entity Framework Core**  
  Object–Relational Mapper (ORM) to manage database access.  
  - Database-First approach with migrations.  
  - Separate contexts for **Catalog** data and **Identity** (user authentication/roles).  
  - Many-to-Many relationships modeled via join entities (e.g., `MovieGenre`, `MovieCast`, `DirectedBy`).  

- **ASP.NET Core Identity**  
  Provides user registration, authentication, and role management.  
  - Supports creating admin/user roles.  
  - Separate `IdentityContext` with migrations for Identity schema.  

- **Bootstrap 5**  
  Frontend CSS framework for layout and responsive design.  
  - Navbar, grid system, cards, tables, and buttons.  
  - Custom styling layered with `site.css`.  

- **Razor Views**  
  Used for rendering server-side HTML templates.  
  - Strongly-typed views with `@model`.  
  - Display helpers like `@Html.DisplayFor`, `@Html.DisplayNameFor`.  
  - Custom layouts (`_Layout.cshtml`) with navigation bar.  

- **Google Charts**  
  For rendering dynamic charts (e.g., movies per year, top rated movies).  
  - Data fetched from API endpoints.  
  - Responsive charts embedded in Razor views.  

---

## 📖 Project Overview

The **Movie Catalog** is a web application for managing movies, genres, actors, directors, ratings, and user watchlists.  
It allows administrators and users to browse, add, edit, and organize movie data in a structured catalog.

### ✨ Features

- **Movies Management**
  - Add, edit, delete, and view movie details (title, year, description, length, poster).  
  - Associate movies with genres, actors, and directors.  

- **Genres, Actors, Directors**
  - Manage genres and their associated movies.  
  - Manage actors and directors, with many-to-many relationships to movies.  

- **Ratings**
  - Store and manage ratings (IMDb, Rotten Tomatoes, or custom sources).  
  - User-specific ratings via `UserRatings`.  

- **User Management (Identity)**
  - Secure login & registration.  
  - Roles (Admin/User).  
  - Admins can manage users (list, create, edit, delete).  

- **Lists**
  - `ToWatchList` – movies a user wants to watch.  
  - `WatchedList` – movies a user has already watched.  

- **Charts & Visualization**
  - Google Charts integration for interactive data visualization.  
  - Example: movies per year, top rated movies by source.  

- **Import & Export**
  - Import movie data from files.  
  - Export data to Excel/Word.  

---

## 📂 Project Structure

- `CatalogDomain` – Core domain models (`Movie`, `Genre`, `Actor`, `Director`, `Rating`, `UserRatings`, etc.).  
- `CatalogInfrastructure` –  
  - EF Core DbContexts (`DbCatalogContext`, `IdentityContext`).  
  - Services (e.g., `PasswordService`).  
  - MVC Controllers and Views.  
- `Migrations` – Code-first migrations for EF Core.  
- `wwwroot` – Static files (CSS, JS, Bootstrap, images).  
- `Views` – Razor views for each controller (`Movies`, `Genres`, `Actors`, etc.).  
- `_Layout.cshtml` – Main layout with navigation bar.  

---
