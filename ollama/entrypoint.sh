#!/bin/bash

# Iniciar Ollama en background
/bin/ollama serve &
OLLAMA_PID=$!

# Esperar a que Ollama esté listo
echo "Esperando que Ollama inicie..."
sleep 10

# Descargar modelo
echo "Descargando phi3.5..."
/bin/ollama pull phi3.5

# Mantener contenedor corriendo
wait $OLLAMA_PID