#!/usr/bin/env bash
set -euo pipefail

log_file="/home/$USER/AspNetProjects/BreweryWarehouse/lab-2/agent_log.txt"
mkdir -p "$(dirname "$log_file")"

payload="$(cat || true)"
if [[ -n "$payload" ]]; then
  printf '%s\n' "$payload" >> "$log_file"
fi
