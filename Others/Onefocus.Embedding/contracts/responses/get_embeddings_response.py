from pydantic import BaseModel
from typing import List
from contracts.responses.get_embeddings_response_item import GetEmbeddingsResponseItem

class GetEmbeddingsResponse(BaseModel):
    results: List[GetEmbeddingsResponseItem]