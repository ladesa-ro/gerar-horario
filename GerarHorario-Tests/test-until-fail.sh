#!/usr/bin/bash 

set -xe

status=0

while [[ "$status" == "0" ]]; do
  (dotnet test --filter Name~AgruparDisciplinasTest; status=$?);
  # aguardar 1 segundo
  sleep 1
done