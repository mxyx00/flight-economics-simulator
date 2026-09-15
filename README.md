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
* Great-circle flight distance calculation method
* Weight-dependent fuel burn calculation
* Weight calculations
* Live jet fuel pricing from the U.S. EIA
* Misc. cost estimates
* Total modelled trip cost
* Break-even ticket price per passenger

## Tech Stack

* **Frontend:** Next.js, React, TypeScript
* **Backend:** ASP.NET Core, C#
* **Database:** PostgreSQL
* **ORM:** Entity Framework Core

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

