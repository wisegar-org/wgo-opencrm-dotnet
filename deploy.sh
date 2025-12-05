#!/usr/bin/env bash
set -euo pipefail

# Ensure the script is run with a locale that handles UTF-8 characters
export LC_ALL=C.UTF-8
export LANG=C.UTF-8

require_sudo() {
  if [[ "$EUID" -ne 0 ]]; then
    echo "[INFO] Questo script usa sudo per i comandi che richiedono privilegi elevati."
  fi
}

install_dotnet() {
  echo "[INFO] Installazione di .NET SDK 10.0..."
  local packages_deb="/tmp/packages-microsoft-prod.deb"

  wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O "$packages_deb"
  sudo dpkg -i "$packages_deb"
  rm -f "$packages_deb"

  sudo apt-get update
  sudo apt-get install -y apt-transport-https dotnet-sdk-10.0
}

ensure_dotnet_10() {
  if command -v dotnet &> /dev/null; then
    local version
    version="$(dotnet --version 2>/dev/null || true)"
    local major="${version%%.*}"
    if [[ "$major" == "10" ]]; then
      echo "[OK] Versione .NET già presente: $version"
      return
    fi
    echo "[WARN] Versione .NET rilevata ($version) non è la 10. Installo la 10.0."
  else
    echo "[INFO] .NET non trovato. Procedo con l'installazione della versione 10.0."
  fi

  install_dotnet
}

prepare_opt_directory() {
  local target_dir="/opt/opencrm"
  if [[ -d "$target_dir" ]]; then
    echo "[OK] Directory $target_dir già presente."
  else
    echo "[INFO] Creo la directory $target_dir con sudo."
    sudo mkdir -p "$target_dir"
  fi
  sudo chown -R "$USER":"$USER" "$target_dir"
}

create_systemd_service() {
  local service_path="/etc/systemd/system/opencrm.service"
  echo "[INFO] Creo/aggiorno il servizio systemd in $service_path"
  sudo tee "$service_path" > /dev/null <<'SERVICE'
[Unit]
Description=OpenCRM Service
After=network.target

[Service]
Type=simple
WorkingDirectory=/opt/opencrm
ExecStart=/usr/bin/dotnet /opt/opencrm/OpenCRM.dll
Restart=on-failure
User=root
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
SERVICE

  sudo systemctl daemon-reload
  echo "[INFO] Servizio creato. Per abilitarlo ed avviarlo eseguire:"
  echo "       sudo systemctl enable --now opencrm.service"
}

main() {
  require_sudo
  ensure_dotnet_10
  prepare_opt_directory
  create_systemd_service
  echo "[INFO] Deploy completato."
}

main "$@"
