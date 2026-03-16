FROM mcr.microsoft.com/mssql/server:2025-latest

COPY scripts/ /scripts/

RUN chmod +x /scripts/entrypoint.sh /scripts/init-db.sh /scripts/healthcheck.sh


EXPOSE 1433

ENTRYPOINT ["/scripts/entrypoint.sh"]
# ENTRYPOINT ["/bin/bash"]