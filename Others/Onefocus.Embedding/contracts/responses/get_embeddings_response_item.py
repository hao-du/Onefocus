from pydantic import BaseModel
from typing import List

class GetEmbeddingsResponseItem(BaseModel):
    text: str
    embeddings: List[float]