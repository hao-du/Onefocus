#!/bin/sh
set -e

echo "ENTRYPOINT: start"

# Try common baked cache paths and copy into persistent /models if empty
if [ ! -d /models ] || [ -z "$(ls -A /models 2>/dev/null)" ]; then
  echo "Initializing model cache at /models from possible baked caches..."
  mkdir -p /models

  if [ -d /opt/model_cache ]; then
    echo "Copying from /opt/model_cache -> /models"
    cp -a /opt/model_cache/. /models/ 2>/dev/null || true
  elif [ -d /opt ]; then
    echo "Copying from /opt -> /models (fallback)"
    cp -a /opt/. /models/ 2>/dev/null || true
  else
    echo "No baked cache found in /opt/* — skipping copy"
  fi
fi

# Debug: list /app and /models contents (first few entries)
echo "Contents of /app (first 50 lines):"
ls -la /app | sed -n '1,50p' || true

echo "Contents of /models (first 50 lines):"
ls -la /models | sed -n '1,50p' || true

# Quick import-check for main to show immediate traceback if there's an import error.
# This will print a Python traceback (if any) before uvicorn starts.
echo "Testing python import of main (will print traceback if import fails)..."
python - <<'PY' || true
import traceback
try:
    import main
    print("IMPORT CHECK: OK - 'main' module loaded, has attribute 'app':", hasattr(main,'app'))
except Exception:
    traceback.print_exc()
PY

echo "Starting uvicorn (app-dir=/app)"
exec uvicorn main:app --host 0.0.0.0 --port 8000 --app-dir /app --log-level info
