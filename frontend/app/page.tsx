"use client";

import { useEffect, useState } from "react";
import styles from "./page.module.css";

interface Airport {
  code: string;
  name: string;
  city: string;
}

interface SimulationResult {
  aircraftName: string;

  departureAirport: string;
  arrivalAirport: string;

  distanceNauticalMiles: number;

  passengerLoadPercent: number;
  passengerCount: number;
  maximumPassengers: number;

  passengerWeightKg: number;
  checkedBaggageWeightKg: number;

  cargoLoadPercent: number;
  cargoWeightKg: number;
  maximumCargoWeightKg: number;

  payloadWeightKg: number;

  operatingEmptyWeightKg: number;

  zeroFuelWeightKg: number;
  maximumZeroFuelWeightKg: number;

  estimatedFlightTimeMinutes: number;

  tripFuelKg: number;
  reserveFuelKg: number;
  contingencyFuelKg: number;

  requiredFuelKg: number;
  maximumFuelKg: number;

  fuelLoadPercent: number;

  rampWeightKg: number;

  takeoffWeightKg: number;
  maximumTakeoffWeightKg: number;

  landingWeightKg: number;
  maximumLandingWeightKg: number;

  tripFuelGallons: number;
  requiredFuelGallons: number;

  fuelPricePerGallon: number;
  fuelPriceDate: string;
  fuelPriceSource: string;

  fuelBurnCost: number;
  fuelLoadValue: number;

  checkedBagRevenue: number;
}

export default function Home() {
  const [airports, setAirports] = useState<Airport[]>([]);

  const [departure, setDeparture] = useState("");
  const [arrival, setArrival] = useState("");

  const [passengerLoad, setPassengerLoad] = useState(80);
  const [cargoLoad, setCargoLoad] = useState(50);

  const [error, setError] = useState("");

  const [result, setResult] =
    useState<SimulationResult | null>(null);

  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    async function loadAirports() {
      try {
        const response = await fetch(
          "http://localhost:5090/api/airports"
        );

        if (!response.ok) {
          throw new Error("Failed to load airports.");
        }

        const data: Airport[] = await response.json();

        setAirports(data);
      } catch (error) {
        console.error(error);

        setError(
          "Unable to load airports. Make sure the backend is running."
        );
      }
    }

    loadAirports();
  }, []);

  function formatFlightTime(totalMinutes: number) {
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;

    return `${hours}h ${minutes}m`;
  }

  function formatMoney(value: number) {
    return value.toLocaleString("en-US", {
      style: "currency",
      currency: "USD",
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
  }

  async function handleSimulation() {
    setError("");
    setResult(null);

    if (!departure || !arrival) {
      setError("Please select both airports.");
      return;
    }

    if (departure === arrival) {
      setError(
        "Departure and arrival airports must be different."
      );
      return;
    }

    setIsLoading(true);

    try {
      const response = await fetch(
        "http://localhost:5090/api/simulation",
        {
          method: "POST",

          headers: {
            "Content-Type": "application/json",
          },

          body: JSON.stringify({
            departureAirport: departure,
            arrivalAirport: arrival,
            passengerLoadPercent: passengerLoad,
            cargoLoadPercent: cargoLoad,
          }),
        }
      );

      const data = await response.json();

      if (!response.ok) {
        throw new Error(
          data.error || "Simulation failed."
        );
      }

      setResult(data);
    } catch (error) {
      console.error(error);

      if (error instanceof Error) {
        setError(error.message);
      } else {
        setError("Simulation failed.");
      }
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <main className={styles.main}>
      <header className={styles.header}>
        <div className={styles.brand}>
          <span className={styles.logo}>✈️</span>
          <span>Flight Economics</span>
        </div>
      </header>

      <section className={styles.container}>
        <div className={styles.intro}>
          <p className={styles.eyebrow}>
            737 ECONOMICS SIMULATOR
          </p>

          <h1>Plan a flight.</h1>

          <p className={styles.description}>
            Estimate the operating economics of a Boeing 737 flight.
          </p>
        </div>

        <div className={styles.card}>
          <h2>Flight</h2>

          <div className={styles.airportGrid}>
            <div className={styles.field}>
              <label htmlFor="departure">
                Departure
              </label>

              <select
                id="departure"
                value={departure}
                onChange={(event) =>
                  setDeparture(event.target.value)
                }
              >
                <option value="">
                  Select airport
                </option>

                {airports.map((airport) => (
                  <option
                    key={airport.code}
                    value={airport.code}
                  >
                    {airport.code} — {airport.city}
                  </option>
                ))}
              </select>
            </div>

            <div className={styles.field}>
              <label htmlFor="arrival">
                Arrival
              </label>

              <select
                id="arrival"
                value={arrival}
                onChange={(event) =>
                  setArrival(event.target.value)
                }
              >
                <option value="">
                  Select airport
                </option>

                {airports.map((airport) => (
                  <option
                    key={airport.code}
                    value={airport.code}
                  >
                    {airport.code} — {airport.city}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className={styles.sliderSection}>
            <div className={styles.sliderHeader}>
              <div>
                <span className={styles.sliderEmoji}>
                  👤
                </span>

                <span>Passengers</span>
              </div>

              <strong>
                {passengerLoad}%
              </strong>
            </div>

            <input
              type="range"
              min="0"
              max="100"
              value={passengerLoad}
              onChange={(event) =>
                setPassengerLoad(
                  Number(event.target.value)
                )
              }
            />
          </div>

          <div className={styles.sliderSection}>
            <div className={styles.sliderHeader}>
              <div>
                <span className={styles.sliderEmoji}>
                  🧳
                </span>

                <span>Cargo</span>
              </div>

              <strong>
                {cargoLoad}%
              </strong>
            </div>

            <input
              type="range"
              min="0"
              max="100"
              value={cargoLoad}
              onChange={(event) =>
                setCargoLoad(
                  Number(event.target.value)
                )
              }
            />
          </div>

          {error && (
            <div className={styles.error}>
              {error}
            </div>
          )}

          <button
            className={styles.button}
            onClick={handleSimulation}
            disabled={isLoading}
          >
            {isLoading
              ? "Running..."
              : "Run Simulation"}
          </button>
        </div>

        {result && (
          <div className={styles.resultsCard}>
            <div className={styles.resultsHeader}>
              <div>
                <p className={styles.resultsLabel}>
                  SIMULATION RESULT
                </p>

                <h2>
                  {result.departureAirport}

                  <span className={styles.routeArrow}>
                    {" "}→{" "}
                  </span>

                  {result.arrivalAirport}
                </h2>
              </div>

              <span className={styles.resultEmoji}>
                ✈️
              </span>
            </div>

            <p className={styles.sectionLabel}>
              FLIGHT
            </p>

            <div className={styles.resultGrid}>
              <div className={styles.resultItem}>
                <span>Aircraft</span>

                <strong>
                  {result.aircraftName}
                </strong>
              </div>

              <div className={styles.resultItem}>
                <span>Distance</span>

                <strong>
                  {result.distanceNauticalMiles.toLocaleString()}{" "}
                  nm
                </strong>
              </div>

              <div className={styles.resultItem}>
                <span>Estimated Time</span>

                <strong>
                  {formatFlightTime(
                    result.estimatedFlightTimeMinutes
                  )}
                </strong>
              </div>

              <div className={styles.resultItem}>
                <span>Passengers</span>

                <strong>
                  {result.passengerCount} /{" "}
                  {result.maximumPassengers}
                </strong>

                <small>
                  {result.passengerLoadPercent}% load
                </small>
              </div>

              <div className={styles.resultItem}>
                <span>Cargo</span>

                <strong>
                  {result.cargoWeightKg.toLocaleString()} kg
                </strong>

                <small>
                  {result.cargoLoadPercent}% load
                </small>
              </div>
            </div>

            <div className={styles.economicsSection}>
              <p className={styles.sectionLabel}>
                WEIGHT & FUEL
              </p>

              <div className={styles.resultGrid}>
                <div className={styles.resultItem}>
                  <span>Payload</span>

                  <strong>
                    {result.payloadWeightKg.toLocaleString()} kg
                  </strong>

                  <small>
                    Passengers + bags + cargo
                  </small>
                </div>

                <div className={styles.resultItem}>
                  <span>Zero Fuel Weight</span>

                  <strong>
                    {result.zeroFuelWeightKg.toLocaleString()} kg
                  </strong>

                  <small>
                    Max{" "}
                    {result.maximumZeroFuelWeightKg.toLocaleString()}{" "}
                    kg
                  </small>
                </div>

                <div className={styles.resultItem}>
                  <span>Takeoff Weight</span>

                  <strong>
                    {result.takeoffWeightKg.toLocaleString()} kg
                  </strong>

                  <small>
                    Max{" "}
                    {result.maximumTakeoffWeightKg.toLocaleString()}{" "}
                    kg
                  </small>
                </div>

                <div className={styles.resultItem}>
                  <span>Landing Weight</span>

                  <strong>
                    {result.landingWeightKg.toLocaleString()} kg
                  </strong>

                  <small>
                    Max{" "}
                    {result.maximumLandingWeightKg.toLocaleString()}{" "}
                    kg
                  </small>
                </div>

                <div className={styles.resultItem}>
                  <span>Trip Fuel Burn</span>

                  <strong>
                    {result.tripFuelKg.toLocaleString()} kg
                  </strong>

                  <small>
                    {result.tripFuelGallons.toLocaleString()} gal
                  </small>
                </div>

                <div className={styles.resultItem}>
                  <span>Fuel Required</span>

                  <strong>
                    {result.requiredFuelKg.toLocaleString()} kg
                  </strong>

                  <small>
                    {result.fuelLoadPercent}% of capacity
                  </small>
                </div>

                <div className={styles.resultItem}>
                  <span>Reserve Fuel</span>

                  <strong>
                    {result.reserveFuelKg.toLocaleString()} kg
                  </strong>
                </div>

                <div className={styles.resultItem}>
                  <span>Contingency Fuel</span>

                  <strong>
                    {result.contingencyFuelKg.toLocaleString()} kg
                  </strong>
                </div>
              </div>
            </div>

            <div className={styles.economicsSection}>
              <p className={styles.sectionLabel}>
                ECONOMICS
              </p>

              <div className={styles.resultGrid}>
                <div className={styles.resultItem}>
                  <span>Jet Fuel Price</span>

                  <strong>
                    $
                    {result.fuelPricePerGallon.toFixed(
                      3
                    )}
                    /gal
                  </strong>

                  <small>
                    EIA • {result.fuelPriceDate}
                  </small>
                </div>

                <div className={styles.resultItem}>
                  <span>Fuel Burn Cost</span>

                  <strong>
                    {formatMoney(
                      result.fuelBurnCost
                    )}
                  </strong>

                  <small>
                    {result.tripFuelGallons.toLocaleString()}{" "}
                    gal burned
                  </small>
                </div>

                <div className={styles.resultItem}>
                  <span>Fuel Loaded Value</span>

                  <strong>
                    {formatMoney(
                      result.fuelLoadValue
                    )}
                  </strong>

                  <small>
                    {result.requiredFuelGallons.toLocaleString()}{" "}
                    gal loaded
                  </small>
                </div>

                <div className={styles.resultItem}>
                  <span>Checked Bag Revenue</span>

                  <strong>
                    {formatMoney(
                      result.checkedBagRevenue
                    )}
                  </strong>

                  <small>
                    $30 × {result.passengerCount} passengers
                  </small>
                </div>
              </div>
            </div>
          </div>
        )}
      </section>
    </main>
  );
}