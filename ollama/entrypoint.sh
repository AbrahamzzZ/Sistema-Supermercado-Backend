#!/bin/bash

# Modelo a descargar (lo define docker-compose con OLLAMA_MODEL)
MODEL="${OLLAMA_MODEL:-phi3.5}"

# Iniciar Ollama en background
/bin/ollama serve &
OLLAMA_PID=$!

# Esperar a que Ollama esté listo
echo "Esperando que Ollama inicie..."
sleep 10

# Descargar modelo
echo "Descargando $MODEL..."
/bin/ollama pull "$MODEL"

# Mantener contenedor corriendo
wait $OLLAMA_PID
