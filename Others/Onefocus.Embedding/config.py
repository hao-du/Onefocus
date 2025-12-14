import os
from pydantic_settings import BaseSettings

class Settings(BaseSettings):
    # Embedding model
    model_name: str = "sentence-transformers/paraphrase-multilingual-MiniLM-L12-v2"

    class Config:
        env_file = ".env"

settings = Settings()