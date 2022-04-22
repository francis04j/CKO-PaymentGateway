#!/bin/bash

#This allows some time for the SQL Server database image to start up.

set -e
run_cmd="dotnet WebApi.dll --server.urls http://*:80"


sleep 10



exec $run_cmd