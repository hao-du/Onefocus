# How to run
1. See [Setup FastAPI](https://dev.azure.com/duchihao/onefocus/_wiki/wikis/onefocus.wiki/33/Setup-FastAPI) for initital setup 
2. Run this to install packages:
```powershell
pip install pydantic-settings
pip install sentence-transformers 
```
or
```powershell
pip install -r requirements.txt
```

3. Run this for localhost:
```powershell
uvicorn main:app --reload       
```

4. Run Swagger:
```powershell
http://127.0.0.1:8000/docs      
```