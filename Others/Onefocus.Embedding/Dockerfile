FROM python:3.11-slim

ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1
ENV TRANSFORMERS_CACHE=/models
ENV HF_HOME=/models
ENV MODEL_NAME=sentence-transformers/paraphrase-multilingual-MiniLM-L12-v2

WORKDIR /app

# install system deps required to build some Python packages and for model download
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
      build-essential \
      git \
      curl \
      && rm -rf /var/lib/apt/lists/*


COPY requirements.txt .
COPY . .

# install Python deps
RUN pip install --no-cache-dir torch torchvision torchaudio \
    --index-url https://download.pytorch.org/whl/cpu
RUN pip install --no-cache-dir -r requirements.txt

# Pre-download the sentence-transformers model into /opt/model_cache at build time.
# This seeds the image with a copy of the model so entrypoint can copy it to the volume on first start.
# Running this at build time avoids redownloading every start (but increases build time & image size).
RUN python - <<'PY'
import os
from sentence_transformers import SentenceTransformer
model_name = os.environ.get("MODEL_NAME")
print("Downloading model:", model_name)
# instantiate once — will download into HF cache (uses HF_HOME/TRANSFORMERS_CACHE env)
SentenceTransformer(model_name)
print("Model downloaded into the image cache.")
PY

# entrypoint will copy the baked cache to the persistent volume /models if empty
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 8000

ENTRYPOINT ["/entrypoint.sh"]
