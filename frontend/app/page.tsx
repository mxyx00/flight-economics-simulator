"use client";

import { useEffect, useState } from "react";
import styles from "./page.module.css";

interface Airport {
  code: string;
  name: string;
  city: string;
}

export default function Home() {
  const [airports, setAirports] = useState<Airport[]>([]);
  const [departure, setDeparture] = useState("");
  const [arrival, setArrival] = useState("");
  const [passengerLoad, setPassengerLoad] = useState(80);
  const [cargoLoad, setCargoLoad] = useState(50);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadAirports() {
      try {
        const response = await fetch(
          "http://localhost:5250/api/airports"
        );

        if (!response.ok) {
          throw new Error("Failed to load airports.");
        }

        const data: Airport[] = await response.json();

        setAirports(data);
      } catch (error) {
        console.error(error);
        setError("Unable to load airports.");
      }
    }

    loadAirports();
  }, []);

  function handleSimulation() {
    setError("");

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

    console.log({
      departure,
      arrival,
      passengerLoad,
      cargoLoad,
    });
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
          <p className={styles.eyebrow}>737 ECONOMICS SIMULATOR</p>

          <h1>Plan a flight.</h1>

          <p className={styles.description}>
            Estimate the operating economics of a Boeing 737 flight.
          </p>
        </div>

        <div className={styles.card}>
          <h2>Flight</h2>

          <div className={styles.airportGrid}>
            <div className={styles.field}>
              <label htmlFor="departure">Departure</label>

              <select
                id="departure"
                value={departure}
                onChange={(event) =>
                  setDeparture(event.target.value)
                }
              >
                <option value="">Select airport</option>

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
              <label htmlFor="arrival">Arrival</label>

              <select
                id="arrival"
                value={arrival}
                onChange={(event) =>
                  setArrival(event.target.value)
                }
              >
                <option value="">Select airport</option>

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
                <span className={styles.sliderEmoji}>👤</span>
                <span>Passengers</span>
              </div>

              <strong>{passengerLoad}%</strong>
            </div>

            <input
              type="range"
              min="0"
              max="100"
              value={passengerLoad}
              onChange={(event) =>
                setPassengerLoad(Number(event.target.value))
              }
            />
          </div>

          <div className={styles.sliderSection}>
            <div className={styles.sliderHeader}>
              <div>
                <span className={styles.sliderEmoji}>🧳</span>
                <span>Cargo</span>
              </div>

              <strong>{cargoLoad}%</strong>
            </div>

            <input
              type="range"
              min="0"
              max="100"
              value={cargoLoad}
              onChange={(event) =>
                setCargoLoad(Number(event.target.value))
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
          >
            Run Simulation
          </button>
        </div>
      </section>
    </main>
  );
}