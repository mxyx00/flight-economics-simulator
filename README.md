# Flight Economics Simulator ✈️

A full-stack web app that simulates the operating economics of a Boeing 737.

Users can select:
- departure and arrival airports
- passenger load
- cargo load
To estimate:
- aircraft weight
- fuel requirements
- direct operating costs
- the break-even ticket price.

## Features

* Select from a predefined list of airports
* Passenger and cargo load sliders
* Great-circle flight distance calculation
* Weight-dependent fuel burn simulation
* Passenger, baggage, payload, takeoff, and landing weight calculations
* Live jet fuel pricing from the U.S. EIA
* Crew cost estimates
* Airport landing and departure fees
* Checked-bag revenue
* Total modeled trip cost
* Break-even ticket price per passenger

## Tech Stack

* **Frontend:** Next.js, React, TypeScript
* **Backend:** ASP.NET Core, C#
* **Database:** PostgreSQL
* **ORM:** Entity Framework Core
* **External Data:** U.S. Energy Information Administration API
* **Version Control:** Git + GitHub

## Architecture

```text
Next.js Frontend
       ↓
ASP.NET Core API
       ↓
Flight Simulation Services
       ↓
Entity Framework Core
       ↓
PostgreSQL
```

## Simulation Model

The simulator uses a simplified Boeing 737-800 model that accounts for:

* Passenger and checked-baggage weight
* Cargo payload
* Zero-fuel weight
* Fuel weight
* Takeoff and landing weight limits
* Weight-dependent fuel burn
* Taxi, climb, cruise, descent, contingency, and reserve fuel

Fuel requirements are calculated iteratively so that the additional weight of the fuel itself affects fuel burn.

## Economics

The current model estimates direct trip costs including:

* Fuel
* Flight crew and cabin crew
* Landing fees
* Departure fees

Checked-bag revenue is subtracted from the modeled trip cost before calculating the break-even ticket price.

> **Note:** This project is an educational simulation and is not intended for operational flight planning. Some aircraft performance, airport fee, and labor-cost values are simplified assumptions.

## Running Locally

Start the backend:

```bash
cd backend
dotnet run
```

Start the frontend:

```bash
cd frontend
npm run dev
```

Then open:

```text
http://localhost:3000
```

The ASP.NET Core API runs locally on:

```text
http://localhost:5090
```
