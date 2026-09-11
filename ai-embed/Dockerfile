# Embedding service (main.py): FastAPI + sentence-transformers, exposes POST /embed.
FROM python:3.12-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY main.py .

EXPOSE 8084

# The script's __main__ binds 127.0.0.1 (loopback only). Run uvicorn directly and
# bind 0.0.0.0 so other containers on the network can reach it.
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8084"]
