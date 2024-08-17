import os
from datetime import datetime


def create_cql_file(file_name):
    timestamp = datetime.now().strftime("%Y%m%d%H%M%S")
    directory = "Migrations"

    if not os.path.exists(directory):
        os.makedirs(directory)

    base_name, extension = os.path.splitext(file_name)

    new_file_name = os.path.join(directory, f"{timestamp}_{base_name}.cql")

    with open(new_file_name, "w") as new_file:
        print(f"File '{new_file_name}' created successfully.")


if __name__ == "__main__":
    import sys

    if len(sys.argv) != 2:
        print("Usage: python script_name.py <file_name>")
    else:
        file_name = sys.argv[1]
        create_cql_file(file_name)
