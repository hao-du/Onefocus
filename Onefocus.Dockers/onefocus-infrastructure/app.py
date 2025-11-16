from fastapi import FastAPI, Depends, HTTPException
from fastapi.security import HTTPBasic, HTTPBasicCredentials
from pydantic import BaseModel
from sentence_transformers import SentenceTransformer

app = FastAPI()
security = HTTPBasic()
model = SentenceTransformer('intfloat/multilingual-e5-base')

class TextInput(BaseModel):
    text: str

def get_current_username(credentials: HTTPBasicCredentials = Depends(security)):
    if credentials.username != "admin" or credentials.password != "R6zntmydkm2?":
        raise HTTPException(status_code=401, detail="Unauthorized")
    return credentials.username

@app.post("/embed")
def embed(input: TextInput, username: str = Depends(get_current_username)):
    embedding = model.encode(input.text)
    return {"embedding": embedding.tolist()}