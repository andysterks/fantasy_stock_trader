CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE accounts (
    id uuid DEFAULT uuid_generate_v4(),
    email_address varchar NOT NULL,
    password varchar NOT NULL,
    first_name varchar NOT NULL,
    last_name varchar NOT NULL,
    created_at timestamp NOT NULL,
    deleted_at timestamp,
    PRIMARY KEY(id)
);

CREATE TABLE sessions (
    id uuid DEFAULT uuid_generate_v4(),
    account_id uuid REFERENCES accounts(id) NOT NULL,
    expires_at timestamp NOT NULL,
    created_at timestamp NOT NULL,
    PRIMARY KEY(id)
);

CREATE TABLE holdings (
    id uuid DEFAULT uuid_generate_v4(),
    PRIMARY KEY(id)
);

CREATE TABLE transactions (
    id uuid DEFAULT uuid_generate_v4(),
    PRIMARY KEY(id)
);