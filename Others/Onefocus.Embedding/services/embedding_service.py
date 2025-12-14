from config import settings
from sentence_transformers import SentenceTransformer
import numpy as np
from typing import List
from contracts.requests.get_embeddings_request import GetEmbeddingsRequest
from contracts.responses.get_embeddings_response import GetEmbeddingsResponse
from contracts.responses.get_embeddings_response_item import GetEmbeddingsResponseItem


class EmbeddingService:
    def __init__(self):
        self.model = SentenceTransformer(settings.model_name)

    @staticmethod
    def normalize_vector(vector: List[float]) -> List[float]:
        vetor = np.array(vector)
        norm = np.linalg.norm(vetor)
        if norm == 0:
            return vetor.tolist()
        return (vetor / norm).tolist()

    @staticmethod
    def cosine_similarity(a: List[float], b: List[float]) -> float:
        return float(np.dot(a, b) / (np.linalg.norm(a) * np.linalg.norm(b)))

    def get_embeddings(self, request: GetEmbeddingsRequest) -> GetEmbeddingsResponse:
        embeddings = self.model.encode(request.texts, convert_to_numpy=True)

        if request.normalize:
            embeddings = [self.normalize_vector(emb) for emb in embeddings]
        else:
            embeddings = [emb.tolist() for emb in embeddings]

        results = [
            GetEmbeddingsResponseItem(text=text, embeddings=vectors)
            for text, vectors in zip(request.texts, embeddings)
        ]

        return GetEmbeddingsResponse(results=results)