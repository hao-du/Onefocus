from fastapi import FastAPI
from sentence_transformers import SentenceTransformer
from typing import List
import numpy as np
from contracts.requests.get_embeddings_request import GetEmbeddingsRequest
from contracts.responses.get_embeddings_response import GetEmbeddingsResponse
from services.embedding_service import EmbeddingService

app = FastAPI(title="Onefocus.Embedding", version="1.0.0")

embedding_service = EmbeddingService()

@app.post("/embed")
def generate_embeddings(request: GetEmbeddingsRequest) -> GetEmbeddingsResponse:
    response = embedding_service.get_embeddings(request)
    return response

@app.post("/test/similarity")
def test_similarity(request: GetEmbeddingsRequest):
    response = embedding_service.get_embeddings(request)

    # Compute pairwise cosine similarity
    n = len(response.results)
    similarity_matrix = []
    for i in range(n):
        row = []
        for j in range(n):
            sim = embedding_service.cosine_similarity(response.results[i].embeddings, response.results[j].embeddings)
            row.append(round(sim, 4))
        similarity_matrix.append(row)

    return {"texts": request.texts, "similarity_matrix": similarity_matrix}
@app.post("/test/topksimilarity")
def top_k_similarity(request: GetEmbeddingsRequest):
    k = 3  # top-k
    response = embedding_service.get_embeddings(request)
    results = []
    n = len(response.results)

    for i in range(n):
        sims = []
        for j in range(n):
            if i == j:
                continue  # skip self
            sim = embedding_service.cosine_similarity(response.results[i].embeddings, response.results[j].embeddings)
            sims.append((request.texts[j], round(sim, 4)))
        # Sort by similarity descending and pick top-k
        topk = sorted(sims, key=lambda x: x[1], reverse=True)[:k]
        results.append({"text": request.texts[i], "top_k": topk})
    return {"results": results}
