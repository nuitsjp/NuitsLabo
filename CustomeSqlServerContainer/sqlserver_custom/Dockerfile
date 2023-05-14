# 基本イメージを指定
FROM mcr.microsoft.com/mssql/server:2022-latest

# 環境変数の設定
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=P@ssw0rd!

# SQLファイルのコピー
COPY ./create_user.sql /tmp/create_user.sql

# AdventureWorksのファイルのコピー
COPY build/ .

# SQLファイルの実行
RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" && \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i /tmp/create_user.sql && \
    (/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i ./adventureworks/instawdb.sql) | grep -q "Completed" && \
    (/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i ./detach.sql) | grep -q "Finished detach." && \
    pkill sqlservr
