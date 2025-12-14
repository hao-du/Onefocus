from pydantic import BaseModel
from typing import List

class GetEmbeddingsRequest(BaseModel):
    texts: List[str]
    normalize: bool = True