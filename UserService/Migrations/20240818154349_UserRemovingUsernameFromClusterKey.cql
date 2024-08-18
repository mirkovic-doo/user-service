DROP TABLE IF EXISTS users;

CREATE TABLE IF NOT EXISTS users (
    id UUID,
    firebase_id text,
    username text,
    email text,
    first_name text,
    last_name text,
    country text,
    city text,
    post_code text,
    street text,
    house_number text,
    is_guest boolean,
    PRIMARY KEY (id, firebase_id)
);

CREATE INDEX IF NOT EXISTS users_firebase_id_idx ON users (firebase_id);
CREATE INDEX IF NOT EXISTS users_username_idx ON users (username);