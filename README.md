# Practica
для практики

ДЛЯ ЗАПУСКА ТАКЖЕ НУЖЕН ДОКЕР ДЕКСТОП (wsl --shutdown)
# Qdrant
docker run -d --name qdrant -p 6333:6333 -p 6334:6334 qdrant/qdrant

# embedding service
cd C:\projects\Practica\ai-embed
docker network create ai-engine
docker compose -f docker-compose.embed.yml up --build -d

# engine — перезапустить после изменения appsettings.Development.json
cd C:\projects\Practica\ai-engine\src
dotnet run --project Engine.Api --launch-profile http

# создать коллекцию в Qdrant
Invoke-WebRequest http://localhost:5154/api/development/create-collections -Method POST