FROM microsoft/mssql-server-linux:latest

ENV SA_PASSWORD=TripTracker1234$$
ENV ACCEPT_EULA=Y

ADD init.sql /tmp
ADD init.sh /

RUN /bin/bash /init.sh