CREATE TABLE IF NOT EXISTS users (
    id UUID,
    username text,
    email text,
    first_name text,
    last_name text,
    country text,
    city text,
    post_code text,
    street text,
    house_number text,
    PRIMARY KEY (id, username, email)
);
