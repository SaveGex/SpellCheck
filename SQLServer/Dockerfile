FROM mcr.microsoft.com/mssql/server:2025-latest

COPY scripts/ /scripts/

EXPOSE 1433

ENTRYPOINT ["/scripts/entrypoint.sh"]
# ENTRYPOINT ["/bin/bash"]