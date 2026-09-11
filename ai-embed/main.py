from fastapi import FastAPI
from pydantic import BaseModel
from sentence_transformers import SentenceTransformer
from qdrant_client.models import PointStruct
import uuid

app = FastAPI()

#при запуске не на компе
model = SentenceTransformer("ai-forever/ru-en-RoSBERTa")

#при запуске на компе
#model = SentenceTransformer('C:/Users/Sector/.cache/huggingface/hub/models--ai-forever--ru-en-RoSBERTa/snapshots/89fb1651989adbb1cfcfdedafd7d102951ad0555')

class EmbedRequest(BaseModel):
    texts: list
    prefix: str = "search_document"

@app.post("/embed")
async def embed(request: EmbedRequest):
    result = []
    for text in request.texts:
        prefixed = f"{request.prefix}: {text}"
        embedding = model.encode(prefixed, normalize_embeddings=True)
        point = PointStruct(
            id = str(uuid.uuid4()),
            vector = embedding.tolist(),
            payload = {"text":text}
        )
        result.append(point)
    return result

