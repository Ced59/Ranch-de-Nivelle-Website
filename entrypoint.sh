#!/bin/bash
set -e
# Wait for SQL Server to be available
until dotnet ef database update; do
  >&2 echo "SQL Server is starting up"
  sleep 1
done
# Run the application
exec "$@"
