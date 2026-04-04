#!/usr/bin/env bash
set -euo pipefail

log_file="/home/$USER/AspNetProjects/BreweryWarehouse/lab-2/agent_log.txt"
mkdir -p "$(dirname "$log_file")"

payload="$(cat || true)"
if [[ -z "$payload" ]]; then
  exit 0
fi

# Only log pre-tool events for subagent calls.
if [[ "$payload" == *'"tool_name":"runSubagent"'* ]]; then
  printf '%s\n' "$payload" >> "$log_file"
fi
