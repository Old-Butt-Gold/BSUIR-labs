CREATE TABLE "passenger"(
    "passenger_id" SERIAL NOT NULL,
    "passenger_name" VARCHAR(100) NOT NULL,
    "contact_data" VARCHAR(255) NOT NULL
);
ALTER TABLE
    "passenger" ADD PRIMARY KEY("passenger_id");
CREATE TABLE "country"(
    "country_id" SERIAL NOT NULL,
    "country_name" VARCHAR(100) NOT NULL
);
ALTER TABLE
    "country" ADD PRIMARY KEY("country_id");
ALTER TABLE
    "country" ADD CONSTRAINT "country_country_name_unique" UNIQUE("country_name");
CREATE TABLE "city"(
    "city_id" SERIAL NOT NULL,
    "country_id" INTEGER NOT NULL,
    "city_name" VARCHAR(100) NOT NULL,
    "timezone" VARCHAR(50) NOT NULL,
    "airports_amount" SMALLINT NOT NULL
);
ALTER TABLE
    "city" ADD PRIMARY KEY("city_id");
ALTER TABLE
    "city" ADD CONSTRAINT "city_city_name_unique" UNIQUE("city_name");
CREATE TABLE "airport"(
    "airport_id" SERIAL NOT NULL,
    "city_id" INTEGER NOT NULL,
    "airport_name" VARCHAR(100) NOT NULL,
    "latitude" DECIMAL(9, 6) NOT NULL,
    "longitude" DECIMAL(9, 6) NOT NULL,
    "icao_code" VARCHAR(4) NOT NULL
);
ALTER TABLE
    "airport" ADD PRIMARY KEY("airport_id");
ALTER TABLE
    "airport" ADD CONSTRAINT "airport_icao_code_unique" UNIQUE("icao_code");
CREATE TABLE "boarding_pass"(
    "boarding_pass_id" SERIAL NOT NULL,
    "ticket_id" INTEGER NOT NULL,
    "flight_id" INTEGER NOT NULL,
    "boarding_pass_name" VARCHAR(50) NOT NULL,
    "seat_number" VARCHAR(10) NOT NULL,
    "boarding_owner_name" VARCHAR(100) NOT NULL
);
ALTER TABLE
    "boarding_pass" ADD PRIMARY KEY("boarding_pass_id");
ALTER TABLE
    "boarding_pass" ADD CONSTRAINT "boarding_pass_ticket_id_unique" UNIQUE("ticket_id");
CREATE TABLE "flight_status"(
    "status_id" SERIAL NOT NULL,
    "status_description" VARCHAR(50) NOT NULL
);
ALTER TABLE
    "flight_status" ADD PRIMARY KEY("status_id");
CREATE TABLE "seat"(
    "seat_id" SERIAL NOT NULL,
    "airplane_id" INTEGER NOT NULL,
    "seat_number" VARCHAR(10) NOT NULL,
    "class" VARCHAR(20) NOT NULL,
    "is_available" BOOLEAN NOT NULL DEFAULT 'DEFAULT TRUE'
);
ALTER TABLE
    "seat" ADD CONSTRAINT "seat_airplane_id_seat_number_unique" UNIQUE("airplane_id", "seat_number");
ALTER TABLE
    "seat" ADD PRIMARY KEY("seat_id");
CREATE TABLE "airplane"(
    "airplane_id" SERIAL NOT NULL,
    "airline_id" INTEGER NOT NULL,
    "model" VARCHAR(100) NOT NULL,
    "range" INTEGER NOT NULL,
    "aircraft_code" INTEGER NOT NULL
);
ALTER TABLE
    "airplane" ADD PRIMARY KEY("airplane_id");
ALTER TABLE
    "airplane" ADD CONSTRAINT "airplane_aircraft_code_unique" UNIQUE("aircraft_code");
CREATE TABLE "schedule"(
    "schedule_id" SERIAL NOT NULL,
    "route_id" INTEGER NOT NULL,
    "airplane_id" INTEGER NOT NULL,
    "frequency" VARCHAR(50) NOT NULL,
    "departure_time" TIME(0) WITHOUT TIME ZONE NOT NULL,
    "arrival_time" TIME(0) WITHOUT TIME ZONE NOT NULL
);
ALTER TABLE
    "schedule" ADD PRIMARY KEY("schedule_id");
CREATE TABLE "ticket"(
    "ticket_id" SERIAL NOT NULL,
    "booking_id" INTEGER NOT NULL,
    "fare_conditions" VARCHAR(50) NOT NULL
);
ALTER TABLE
    "ticket" ADD PRIMARY KEY("ticket_id");
ALTER TABLE
    "ticket" ADD CONSTRAINT "ticket_booking_id_unique" UNIQUE("booking_id");
CREATE TABLE "booking"(
    "booking_id" SERIAL NOT NULL,
    "passenger_id" INTEGER NOT NULL,
    "total_booking_amount" DECIMAL(10, 2) NOT NULL,
    "booking_name" VARCHAR(100) NOT NULL,
    "booking_date" DATE NOT NULL
);
ALTER TABLE
    "booking" ADD PRIMARY KEY("booking_id");
CREATE TABLE "flight"(
    "flight_id" SERIAL NOT NULL,
    "schedule_id" INTEGER NOT NULL,
    "status" INTEGER NOT NULL,
    "scheduled_departure" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL,
    "scheduled_arrival" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL,
    "actual_departure" TIMESTAMP(0) WITHOUT TIME ZONE NULL,
    "actual_arrival" TIMESTAMP(0) WITHOUT TIME ZONE NULL
);
ALTER TABLE
    "flight" ADD PRIMARY KEY("flight_id");
CREATE TABLE "route"(
    "route_id" SERIAL NOT NULL,
    "departure_airport_id" INTEGER NOT NULL,
    "arrival_airport_id" INTEGER NOT NULL
);
ALTER TABLE
    "route" ADD PRIMARY KEY("route_id");
CREATE TABLE "airline"(
    "airline_id" SERIAL NOT NULL,
    "airline_name" VARCHAR(100) NOT NULL
);
ALTER TABLE
    "airline" ADD PRIMARY KEY("airline_id");
ALTER TABLE
    "airline" ADD CONSTRAINT "airline_airline_name_unique" UNIQUE("airline_name");
ALTER TABLE
    "flight" ADD CONSTRAINT "flight_status_foreign" FOREIGN KEY("status") REFERENCES "flight_status"("status_id");
ALTER TABLE
    "ticket" ADD CONSTRAINT "ticket_booking_id_foreign" FOREIGN KEY("booking_id") REFERENCES "booking"("booking_id");
ALTER TABLE
    "flight" ADD CONSTRAINT "flight_schedule_id_foreign" FOREIGN KEY("schedule_id") REFERENCES "schedule"("schedule_id");
ALTER TABLE
    "booking" ADD CONSTRAINT "booking_passenger_id_foreign" FOREIGN KEY("passenger_id") REFERENCES "passenger"("passenger_id");
ALTER TABLE
    "boarding_pass" ADD CONSTRAINT "boarding_pass_flight_id_foreign" FOREIGN KEY("flight_id") REFERENCES "flight"("flight_id");
ALTER TABLE
    "airplane" ADD CONSTRAINT "airplane_airline_id_foreign" FOREIGN KEY("airline_id") REFERENCES "airline"("airline_id");
ALTER TABLE
    "schedule" ADD CONSTRAINT "schedule_route_id_foreign" FOREIGN KEY("route_id") REFERENCES "route"("route_id");
ALTER TABLE
    "airport" ADD CONSTRAINT "airport_city_id_foreign" FOREIGN KEY("city_id") REFERENCES "city"("city_id");
ALTER TABLE
    "route" ADD CONSTRAINT "route_departure_airport_id_foreign" FOREIGN KEY("departure_airport_id") REFERENCES "airport"("airport_id");
ALTER TABLE
    "city" ADD CONSTRAINT "city_country_id_foreign" FOREIGN KEY("country_id") REFERENCES "country"("country_id");
ALTER TABLE
    "schedule" ADD CONSTRAINT "schedule_airplane_id_foreign" FOREIGN KEY("airplane_id") REFERENCES "airplane"("airplane_id");
ALTER TABLE
    "boarding_pass" ADD CONSTRAINT "boarding_pass_ticket_id_foreign" FOREIGN KEY("ticket_id") REFERENCES "ticket"("ticket_id");
ALTER TABLE
    "seat" ADD CONSTRAINT "seat_airplane_id_foreign" FOREIGN KEY("airplane_id") REFERENCES "airplane"("airplane_id");
ALTER TABLE
    "route" ADD CONSTRAINT "route_arrival_airport_id_foreign" FOREIGN KEY("arrival_airport_id") REFERENCES "airport"("airport_id");