#!/bin/sh
set -e

cd /app/src

# echo "Running migrations..."
# dotnet ef database update

echo "Starting API..."
exec dotnet DbManagerApi.dll